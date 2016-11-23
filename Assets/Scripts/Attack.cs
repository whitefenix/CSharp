using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour {

	private const int NUM_MODES = 2;
	public enum Mode {
		SINGLE = 0,
		AOE_ROW
	}

    private bool knockback = false;
    public float thrust = 100000.0f;
    private UIScript ui;

	//Used collider
	public Collider[] attackCollidersPerMode = new Collider[NUM_MODES];
	//Damage per hit
	public float[] attackDamagePerMode = new float[NUM_MODES] {
		10.0f,
		5.0f
	};
	//Hits per second
	public float[] attackSpeedPerMode = new float[NUM_MODES] {
		2.0f,
		1.0f
	};

	private Mode currentAttack = Mode.SINGLE;

	public Mode currentAttackMode {
		get 
		{  
			return currentAttack; 
		}
		set 
		{
			currentAttack = value;

			//Enable correct collider
			OnSwitchAttackMode ();
		}
	}

	private double attackTimeout;

	private List<GameObject> reachableEnemies;
	private List<GameObject> killedEnemies;

	// Use this for initialization
	void Start () 
	{
		reachableEnemies = new List<GameObject> ();
		killedEnemies = new List<GameObject> ();
        ui = GetComponentInParent<UIScript>();

		OnSwitchAttackMode ();
	}

	void Update ()
	{
		//TODO delete DEBUG
		if (Input.GetKeyDown(KeyCode.X))
		{
			if (currentAttack.Equals (Mode.SINGLE))
				currentAttackMode = Mode.AOE_ROW;
			else
				currentAttackMode = Mode.SINGLE;
		}	

        //Check if we are knockbacking
        if (ui.offHand.instrumentName.Equals("Drum"))
        {
            knockback = true;
        }
        else
        {
            knockback = false;
        }

		if (Time.time >= attackTimeout && AttackInputTriggered()) 
		{
			switch(currentAttack) 
			{
			case Mode.SINGLE:
				
				GameObject closestEnemy = GetNearestEnemy ();
				if (closestEnemy != null) 
				{   
                        if (knockback)
                        {
                            MoveEnemy(closestEnemy);
                        }    
                        DealDamage (closestEnemy);
				}
				break;

			case Mode.AOE_ROW:

				foreach (GameObject enemy in reachableEnemies) 
				{
                        if (knockback)
                        {
                            MoveEnemy(enemy);
                        }
                        DealDamage (enemy);
				}
				break;
			}

			RemoveKilledEnemiesFromList ();
			attackTimeout = Time.time + (1.0f / attackSpeedPerMode[(int)currentAttackMode]);
		}
	}

	private void OnSwitchAttackMode()
	{
		int mode = (int)currentAttack;

		for(int i = 0; i < attackCollidersPerMode.Length; ++i)
		{
			attackCollidersPerMode [i].enabled = (i == mode);
		}
	}

	void OnTriggerEnter (Collider other) 
	{
		if (IsEnemyCollider(other) && !reachableEnemies.Contains (other.gameObject)) 
		{
			reachableEnemies.Add (other.gameObject);

			//Just for debugging
			//Select (other.gameObject, true);
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (IsEnemyCollider(other) && reachableEnemies.Contains (other.gameObject)) 
		{
			reachableEnemies.Remove (other.gameObject);

			//Just for debugging
			//Select (other.gameObject, false);
		}
	}

	private void Select(GameObject enemy, bool select)
	{
		SpriteRenderer sprite = enemy.GetComponentsInChildren<SpriteRenderer> () [0];

		if(select)
			sprite.color = Color.blue;
		else
			sprite.color = Color.white;
	}

    private void MoveEnemy(GameObject enemy)
    {
        Rigidbody body = enemy.GetComponent<Rigidbody>();
        //TODO: Change thrustvector to be the direction opposite of player
        Vector3 thrustvector = new Vector3(thrust, thrust, thrust);
        body.AddForce(thrustvector, ForceMode.Impulse );
    }

	private void DealDamage(GameObject enemy)
	{
		Health enemyHealth = enemy.GetComponent<Health> ();

		enemyHealth.damage (attackDamagePerMode[(int)currentAttackMode]);

		if (enemyHealth.isDead)
			killedEnemies.Add (enemy);//reachableEnemies.Remove (enemy);
	}

	private void RemoveKilledEnemiesFromList()
	{
		if (killedEnemies.Count == 0)
			return;

		foreach (GameObject enemy in killedEnemies) 
		{
			reachableEnemies.Remove (enemy);
		}

		killedEnemies.Clear ();
	}

	private GameObject GetNearestEnemy()
	{
		float minDist = float.MaxValue;
		float curDist;
		GameObject closest = null;

		foreach (GameObject enemy in reachableEnemies) 
		{
			//TODO Check: Comparison between enemy and collider center, compare against player center instead?
			curDist = Vector3.Distance (enemy.transform.position, transform.position);

			if (curDist < minDist) 
			{
				minDist = curDist;
				closest = enemy;
			}
		}

		return closest;
	}

	public static bool IsEnemyCollider(Collider other)
	{
		return other.tag == "Attackable" && !other.isTrigger;
	}

	private bool AttackInputTriggered() 
	{
		return Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (GlobalConstants.XBOX_BTN_A);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour {

	public enum Mode {
		SINGLE = 0,
		MULTI
	}

	public float damage;
	public Mode currentAttack = Mode.SINGLE;
	public float attackSpeed = 0.2f;

	private double attackTimeout;

	private List<GameObject> reachableEnemies;
	private List<GameObject> killedEnemies;

	// Use this for initialization
	void Start () 
	{
		reachableEnemies = new List<GameObject> ();
		killedEnemies = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Time.time >= attackTimeout && Input.GetKeyDown (KeyCode.Space)) 
		{
			switch(currentAttack) 
			{
			case Mode.SINGLE:
				
				GameObject closestEnemy = GetNearestEnemy ();
				if (closestEnemy != null) 
				{
					DealDamage (closestEnemy);
				}
				break;
			case Mode.MULTI:

				foreach (GameObject enemy in reachableEnemies) 
				{
					Debug.Log ("Deal Damage to " + enemy.name);

					DealDamage (enemy);
				}
				break;
			}

			RemoveKilledEnemiesFromList ();
			attackTimeout = Time.time + attackSpeed;
		}
	}

	void OnTriggerEnter (Collider other) 
	{
		if (other.tag == "Attackable" && !other.isTrigger && !reachableEnemies.Contains (other.gameObject)) 
		{
			reachableEnemies.Add (other.gameObject);

			//Just for debugging
			//Select (other.gameObject, true);
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (other.tag == "Attackable" && !other.isTrigger && reachableEnemies.Contains (other.gameObject)) 
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

	private void DealDamage(GameObject enemy)
	{
		Health enemyHealth = enemy.GetComponent<Health> ();

		enemyHealth.damage (damage);

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
}

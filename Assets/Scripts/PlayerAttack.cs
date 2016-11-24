using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour {

	private const int MAIN_INSTRUMENT_COUNT = 2;
	public enum Type {
		FLUTE = 0,
		VIOLIN
	}

	public enum Mode {
		SINGLE_MEELE = 0,
		SINGLE_RANGED,
		AOE_MEELE_ROW
	}

	public enum Perk {
		KNOCKBACK = 0,
		PIERCE
	}

	[System.Serializable]
	public class MainInstrument
	{
		public float damage;
		public float speed;
		public float knockbackStun;
		public float knockbackStrength;
		public Mode mode;
		public CombatRange.RangeCollider meeleCollider;
	}

	public MainInstrument[] mainHandInstruments = new MainInstrument[MAIN_INSTRUMENT_COUNT];

	private double attackTimeout;

	private CombatRange combatRange;
	private List<GameObject> killedEnemies;

	//[HideInInspector] 
	public Perk offHandPerk;
	public Type mainHandIdx;

	public void SetCurrentInstrument(Type instrument)
	{
		mainHandIdx = instrument;

		this.combatRange.SetCollider (mainHandInstruments[(int)mainHandIdx].meeleCollider);
	}

	// Use this for initialization
	void Start () 
	{
		killedEnemies = new List<GameObject> ();
		combatRange = GetComponentsInChildren<CombatRange>()[0];

		SetCurrentInstrument (Type.VIOLIN);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time >= attackTimeout && AttackInputTriggered()) 
		{
			MainInstrument currentInstrument = mainHandInstruments[(int)mainHandIdx];
			switch(currentInstrument.mode) 
			{
			case Mode.SINGLE_MEELE:

				GameObject closestEnemy = combatRange.GetNearestEnemy ();
				if (closestEnemy != null) 
				{ 
					DealDamage (closestEnemy);
				}
				break;

			case Mode.AOE_MEELE_ROW:

				GameObject[] enemies = combatRange.GetEnemies ();
				foreach (GameObject enemy in enemies) 
				{
					DealDamage (enemy);
				}
				break;
			case Mode.SINGLE_RANGED:
				//TODO
				break;
			}

			combatRange.RemoveEnemies (ref killedEnemies);
			attackTimeout = Time.time + (1.0f / currentInstrument.speed);
		}
	}

	private void KnockbackEnemy(GameObject enemy)
	{
		float stunTime = mainHandInstruments [(int)mainHandIdx].knockbackStun;
		Vector3 dir = (enemy.transform.position - transform.position).normalized;
		dir *= mainHandInstruments [(int)mainHandIdx].knockbackStrength;

		enemy.SendMessage("Knockback", dir);
		enemy.SendMessage("Stun", stunTime);
	}

	private void DealDamage(GameObject enemy)
	{
		if (offHandPerk.Equals (Perk.KNOCKBACK)) 
		{
			KnockbackEnemy (enemy);
		}

		Health enemyHealth = enemy.GetComponent<Health> ();

		enemyHealth.damage (mainHandInstruments[(int)mainHandIdx].damage);

		if (enemyHealth.isDead)
			killedEnemies.Add (enemy);
	}

	private bool AttackInputTriggered() 
	{
		return Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (GlobalConstants.XBOX_BTN_A);
	}
}

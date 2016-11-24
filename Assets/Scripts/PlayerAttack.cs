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
		[Header("General:")]
		public float damage;
		public float speed;
		public Mode mode;

		[Header("Meele only:")]
		public CombatRange.RangeCollider meeleCollider;

		[Header("Ranged only:")]
		public float range;
		public GameObject missile;

		[Header("PERK Pierce, ranged only:")]
		public int pierceTrough;

		[Header("PERK Pierce, meele only:")]
		public CombatRange.RangeCollider pierceCollider;

		[Header("PERK Knockback:")]
		public float knockbackStun;
		public float knockbackStrength;
	}

	public MainInstrument[] mainHandInstruments = new MainInstrument[MAIN_INSTRUMENT_COUNT];

	private double attackTimeout;

	private CombatRange combatRange;
	private List<GameObject> killedEnemies;

	private Perk offHandPerk;
	private Type mainHandIdx;

	public void SetCurrentPerk(Perk perk)
	{
		offHandPerk = perk;

		//update collider in case the perk influences the range!
		SetCurrentInstrument(mainHandIdx);
	}

	public void SetCurrentInstrument(Type instrument)
	{
		mainHandIdx = instrument;

		if(offHandPerk.Equals (Perk.PIERCE))
			this.combatRange.SetCollider (mainHandInstruments[(int)mainHandIdx].pierceCollider);
		else
			this.combatRange.SetCollider (mainHandInstruments[(int)mainHandIdx].meeleCollider);
	}

	// Use this for initialization
	void Start () 
	{
		killedEnemies = new List<GameObject> ();
		combatRange = GetComponentsInChildren<CombatRange>()[0];

		//TODO do somewhere else... init problems
		SetCurrentInstrument (Type.VIOLIN);
		SetCurrentPerk(Perk.KNOCKBACK);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time >= attackTimeout && AttackInputTriggered()) 
		{
			MainInstrument currentInstrument = mainHandInstruments[(int)mainHandIdx];
			switch (currentInstrument.mode) 
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

				int pierce = 1;
				if (offHandPerk.Equals (Perk.PIERCE))
					pierce = currentInstrument.pierceTrough;

				Vector3 position = transform.position + new Vector3 (0, 1, 0) + transform.forward;
				GameObject missile = (GameObject)Instantiate (
					currentInstrument.missile, position, 
					Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0,90,0));
				Missile m = missile.GetComponent<Missile> ();

				m.attack = this;
				m.direction = transform.forward * 0.1f;
				m.maxRange = currentInstrument.range;
				m.pierce = pierce;

//				if (offHandPerk.Equals (Perk.PIERCE)) 
//				{
//					GameObject[] rangedEnemies = combatRange.GetEnemiesInRange (
//						                             transform.position + new Vector3 (0, 1, 0), 
//						                             transform.forward, 
//						                             currentInstrument.range, 
//						                             currentInstrument.pierceTrough + 1);
//
//					foreach (GameObject enemy in rangedEnemies) 
//					{
//						DealDamage (enemy);
//					}
//				} 
//				else 
//				{
//					GameObject rangedEnemy = combatRange.GetEnemyInRange (
//						                         transform.position + new Vector3 (0, 1, 0), 
//						                         transform.forward, 
//						                         currentInstrument.range);
//
//					if (rangedEnemy != null) 
//					{ 
//						DealDamage (rangedEnemy);
//					}
//				}

				break;
			}
			combatRange.RemoveEnemies (ref killedEnemies);
			attackTimeout = Time.time + (1.0f / currentInstrument.speed);
		}

		Debug.DrawRay (transform.position + new Vector3(0,1,0), transform.forward * mainHandInstruments[(int)mainHandIdx].range);
	}

	private void KnockbackEnemy(GameObject enemy)
	{
		float stunTime = mainHandInstruments [(int)mainHandIdx].knockbackStun;
		Vector3 dir = (enemy.transform.position - transform.position).normalized;
		dir *= mainHandInstruments [(int)mainHandIdx].knockbackStrength;

		enemy.SendMessage("Knockback", dir);
		enemy.SendMessage("Stun", stunTime);
	}

	public void DealDamage(GameObject enemy)
	{
		if (offHandPerk.Equals (Perk.KNOCKBACK)) 
		{
			KnockbackEnemy (enemy);
		}

		Health enemyHealth = enemy.GetComponent<Health> ();
		enemyHealth.damage (mainHandInstruments[(int)mainHandIdx].damage);

		if (enemyHealth.isDead) 
		{
			killedEnemies.Add (enemy);
		}
		else
		{
			//Move to the origin of attack (if not already following player)
			enemy.SendMessage ("SetMoveOrder", transform.position);
		}
	}

	private bool AttackInputTriggered() 
	{
		return Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (GlobalConstants.XBOX_BTN_A);
	}
}

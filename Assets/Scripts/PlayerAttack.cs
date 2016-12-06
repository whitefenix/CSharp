using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour {

	private const int MAIN_INSTRUMENT_COUNT = 2;
    private int counter = 0;

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
		[Range(0,1)]
		public float criticalProbability;
        public AudioClip[] hitClip;

        [Header("Meele only:")]
		public CombatRange.RangeCollider meeleCollider;

		[Header("Ranged only:")]
		public float range;
		public GameObject missile;

		[Header("PERK Pierce, ranged only:")]
		public int pierceTrough;
		[Range(0,1)]
		public float pierceProbability;

		[Header("PERK Pierce, meele only:")]
		public CombatRange.RangeCollider pierceCollider;

		[Header("PERK Knockback:")]
		public float knockbackStrength;
		[Range(0,1)]
		public float knockbackProbability;
		public float knockbackStun;
		[Range(0,1)]
		public float knockbackStunProbability;
	}

	public float criticalDamageMultiplicator = 3.0f;

	public MainInstrument[] mainHandInstruments = new MainInstrument[MAIN_INSTRUMENT_COUNT];

	private double attackTimeout;

	private Animator animator;
	private CombatRange combatRange;
	private List<GameObject> killedEnemies;

	private Perk offHandPerk;
	private Type mainHandIdx;

	private bool talkingToNPC = false;
	private PlayerQuests quests;
	private NPCTalk talkingNPC;

	private Vector3 missileSpawnHeight = Vector3.up;

	private PlayerSkills ps;

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
		animator = GetComponentsInChildren<Animator>()[0];
		quests = GetComponent<PlayerQuests> ();

		ps = GetComponent<PlayerSkills> ();

		//TODO do somewhere else... init problems
		SetCurrentInstrument (Type.VIOLIN);
		SetCurrentPerk(Perk.KNOCKBACK);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (talkingToNPC && AttackInputTriggered ()) 
		{
			if (!talkingNPC.Talk ()) 
			{
				if (talkingNPC.HasOpenQuest ()) 
				{
					quests.AddQuests (talkingNPC.RequestQuest());
				}

				if (talkingNPC.HasGift ()) 
				{
					ps.EquipBonusItem(talkingNPC.reward);
				}
			}
		} 
		else if (Time.time >= attackTimeout && AttackInputTriggered ()) 
		{
			MainInstrument currentInstrument = GetMainInstrument ();
			switch (currentInstrument.mode) {
			case Mode.SINGLE_MEELE:

				GameObject closestEnemy = combatRange.GetNearestEnemy ();
				if (closestEnemy != null) { 
					DealDamage (closestEnemy);
				}
				break;

			case Mode.AOE_MEELE_ROW:

				GameObject[] enemies = combatRange.GetEnemies ();
				foreach (GameObject enemy in enemies) {
					DealDamage (enemy);
				}
				break;
			case Mode.SINGLE_RANGED:

				int pierce = 1;
				if (offHandPerk.Equals (Perk.PIERCE) && Random.value <= GetMainInstrument ().pierceProbability + ps.biAggr.pierceProbability) 
				{
					pierce = currentInstrument.pierceTrough + ps.biAggr.pierceTrough;
				}

				Vector3 position = transform.position + missileSpawnHeight + transform.forward;

				GameObject missile = (GameObject)Instantiate (
					                     currentInstrument.missile, position, 
					                     Quaternion.LookRotation (transform.forward) * Quaternion.Euler (0, 90, 0));
				
				Missile m = missile.GetComponent<Missile> ();

				m.attack = this;
				m.direction = transform.forward * 0.1f;
				m.maxRange = currentInstrument.range + ps.biAggr.range;
				m.pierce = pierce;

				break;
			}
			combatRange.RemoveEnemies (ref killedEnemies);
			attackTimeout = Time.time + (1.0f / (currentInstrument.speed + ps.biAggr.attackSpeed));
			//animator.SetBool ("isFighting", true);

			animator.Play("Fighting");
		} else {
			//animator.SetBool ("isFighting", false);

		}

		Debug.DrawRay (transform.position + new Vector3(0,1,0), transform.forward * mainHandInstruments[(int)mainHandIdx].range);
	}

	private MainInstrument GetMainInstrument()
	{
		return mainHandInstruments [(int)mainHandIdx];
	}

	private void KnockbackEnemy(GameObject enemy)
	{
		float stunTime = GetMainInstrument().knockbackStun + ps.biAggr.knockbackStun;
		Vector3 dir = (enemy.transform.position - transform.position).normalized;
		dir *= GetMainInstrument().knockbackStrength + ps.biAggr.knockbackStrength;


		enemy.SendMessage ("Knockback", dir);

		if (Random.value <= GetMainInstrument().knockbackStunProbability + ps.biAggr.knockbackStunProbability)
			enemy.SendMessage ("Stun", stunTime);
	}

	public void DealDamage(GameObject enemy)
	{
        AudioSource hitSound = GetComponent<AudioSource>();
        hitSound.PlayOneShot(GetMainInstrument().hitClip[counter]);
        counter++;
        if (counter == 5)
        {
            counter = 0;
        }

		if (offHandPerk.Equals (Perk.KNOCKBACK) && Random.value <= GetMainInstrument().knockbackProbability + ps.biAggr.knockbackProbability) 
		{
			KnockbackEnemy (enemy);
		}

		bool critical = (Random.value <= GetMainInstrument ().criticalProbability + ps.biAggr.criticalProbability);
		float damage = GetMainInstrument ().damage + ps.biAggr.attackDamage;

		if(critical)
			damage *= criticalDamageMultiplicator;

		Health enemyHealth = enemy.GetComponent<Health> ();
		enemyHealth.damage (damage, critical);

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

	void OnTriggerEnter (Collider other) 
	{
		if (other.tag == "NPC") 
		{
			talkingToNPC = true;
			talkingNPC = other.gameObject.GetComponent<NPCTalk> ();
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (other.tag == "NPC") 
		{
			talkingToNPC = false;
			talkingNPC = null;
		}
	}
}

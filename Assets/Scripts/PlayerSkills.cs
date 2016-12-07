using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkills : MonoBehaviour {

	public BonusItem biAggr;
	public List<BonusItem> bonusItems;
    public AudioClip clip;

	// Use this for initialization
	void Start () 
	{
		biAggr = new BonusItem ();
		bonusItems = new List<BonusItem> ();
	}

	public void EquipBonusItem(BonusItem bi)
	{
        AudioSource source = GetComponent<AudioSource>();
        source.PlayOneShot(clip);

		bonusItems.Add (bi);

		biAggr.movementSpeed += bi.movementSpeed;
		biAggr.maximumHealth += bi.maximumHealth;

		biAggr.attackDamage += bi.attackDamage;
		biAggr.attackSpeed += bi.attackSpeed;
		biAggr.criticalProbability += bi.criticalProbability;

		biAggr.range += bi.range;

		biAggr.pierceTrough += bi.pierceTrough;
		biAggr.pierceProbability += bi.pierceProbability;

		biAggr.knockbackStrength += bi.knockbackStrength;
		biAggr.knockbackStunProbability += bi.knockbackStunProbability;
		biAggr.knockbackStun += bi.knockbackStun;
		biAggr.knockbackStunProbability += bi.knockbackStunProbability;

		gameObject.SendMessage ("OnEqipSkillBook", SendMessageOptions.DontRequireReceiver);
	}
}

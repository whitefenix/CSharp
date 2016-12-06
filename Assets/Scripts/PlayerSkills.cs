using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkills : MonoBehaviour {

	public BonusItem biAggr;
	public List<BonusItem> bonusItems;

	// Use this for initialization
	void Start () 
	{
		biAggr = new BonusItem ();
		bonusItems = new List<BonusItem> ();
	}

	public void EquipBonusItem(BonusItem bi, bool unequip = false)
	{
		int sign = 1;
		if (unequip) 
		{
			bonusItems.Remove (bi);
			sign = -1;
		} 
		else 
		{
			bonusItems.Add (bi);

			gameObject.SendMessage ("OnEqipSkillBook", SendMessageOptions.DontRequireReceiver);
		}

		biAggr.movementSpeed += bi.movementSpeed * sign;
		biAggr.maximumHealth += bi.maximumHealth * sign;

		biAggr.attackDamage += bi.attackDamage * sign;
		biAggr.attackSpeed += bi.attackSpeed * sign;
		biAggr.criticalProbability += bi.criticalProbability * sign;

		biAggr.range += bi.range * sign;

		biAggr.pierceTrough += bi.pierceTrough * sign;
		biAggr.pierceProbability += bi.pierceProbability * sign;

		biAggr.knockbackStrength += bi.knockbackStrength * sign;
		biAggr.knockbackStunProbability += bi.knockbackStunProbability * sign;
		biAggr.knockbackStun += bi.knockbackStun * sign;
		biAggr.knockbackStunProbability += bi.knockbackStunProbability * sign;
	}
}

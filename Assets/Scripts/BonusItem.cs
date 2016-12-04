using UnityEngine;
using System.Collections;

[System.Serializable]
public class BonusItem 
{
	[Header("Player:")]
	public float movementSpeed;
	public float maximumHealth;

	[Header("Attack:")]
	public float attackDamage;
	public float attackSpeed;
	[Range(0,1)]
	public float criticalProbability;

	[Header(" - Ranged only:")]
	public float range;

	[Header(" - PERK Pierce, ranged only:")]
	public int pierceTrough;
	[Range(0,1)]
	public float pierceProbability;

	[Header(" - PERK Knockback:")]
	public float knockbackStrength;
	[Range(0,1)]
	public float knockbackProbability;
	public float knockbackStun;
	[Range(0,1)]
	public float knockbackStunProbability;
}

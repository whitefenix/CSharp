using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	public float damage;
	public float attackSpeed = 1f;

	private bool playerInRange = false;

	private GameObject player;

	private double attackTimeout;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Time.time >= attackTimeout && playerInRange) 
		{
			DealDamage (player);

			attackTimeout = Time.time + attackSpeed;
		}
	}

	void OnTriggerEnter (Collider other) 
	{
		if (IsPlayerCollider(other)) 
		{
			playerInRange = true;
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (IsPlayerCollider(other)) 
		{
			playerInRange = false;
		}
	}

	private void DealDamage(GameObject player)
	{
		Health playerHealth = player.GetComponent<Health> ();

		playerHealth.damage (damage);

		if (playerHealth.isDead)
			playerInRange = false;
	}

	public static bool IsPlayerCollider(Collider other)
	{
		return other.tag == "Player" && !other.isTrigger;
	}
}

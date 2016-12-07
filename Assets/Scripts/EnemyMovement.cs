using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour 
{
	private float targetDistance;

	public float enemyLookDistance;

	public float alertRadius = 5;

	private Transform player;
	private NavMeshAgent nav;

	private SpriteRenderer sprite;
	public GameObject alertSprite;

	public bool returnToInitialPosition;
	private Vector3 initialPosition;

	private double moveTimeout;

	private bool moveOrder;
	public Vector3 destinationOrder;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		sprite = this.GetComponentsInChildren<SpriteRenderer> ()[0];
		nav = GetComponent<NavMeshAgent> ();

		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time >= moveTimeout && player != null) 
		{
			//Main priority is to follow the player
			if (IsPlayerInRange()) 
			{
				nav.SetDestination (player.position);

				//reset move order
				moveOrder = false;

				alertSprite.SetActive (false);
			}
			else if (moveOrder) //if no player is in sight, follow order destination
			{
				nav.SetDestination (destinationOrder);

				//if enemy is close to destination stop move order
				if (Vector3.Distance (transform.position, destinationOrder) < 1.5f) 
				{
					moveOrder = false;
					alertSprite.SetActive (false);
				} 
				else 
				{
					alertSprite.SetActive (true);
				}
			}
			else //if no player is in sight and no order given, stand still or go back to initial position
			{
				if(returnToInitialPosition)
					nav.SetDestination (initialPosition);
				else
					nav.SetDestination (transform.position);

				alertSprite.SetActive (false);
			}

			//Obsolete with animated sprite
			if (transform.forward.x < 0)
				sprite.flipX = true;
			else
				sprite.flipX = false;
		}
	}

	private bool IsPlayerInRange()
	{
		return Vector3.Distance (player.position, transform.position) < enemyLookDistance;
	}

	public void InvestigateAttackSource(Vector3 attackSource)
	{
		AlertNearbyEnemies(attackSource);
		SetMoveOrder(attackSource);
	}

	public void AlertNearbyEnemies(Vector3 attackSource)
	{
		Collider[] hitColliders = Physics.OverlapSphere (transform.position, alertRadius);
		EnemyMovement tmp;

		foreach (Collider c in hitColliders) 
		{
			if (CombatRange.IsEnemyCollider (c)) 
			{
				tmp = c.GetComponent<EnemyMovement> ();

				if (tmp) 
				{
					tmp.SetMoveOrder (attackSource);
				}
			}
		}
	}

	/**
	 * Orders the enemy to move to this destination. 
	 * When the destination is reached it either stays there 
	 * or moves back to its initial position depending on returnToInitialPosition
	 */
	public void SetMoveOrder(Vector3 destination)
	{
		moveOrder = true;
		destinationOrder = destination;
	}

	/**
	 * Orders the enemy to move to the current player position
	 * When the destination is reached it either stays there 
	 * or moves back to its initial position depending on returnToInitialPosition
	 */
	public void MoveToCurrentPlayerPosition()
	{
		SetMoveOrder (player.position);
	}

	/**
	 * Interrupts the current movement and pushes
	 * the enemy in the specified direction.
	 * The magnitude of the vector defines the strength.
	 */ 
	public void Knockback (Vector3 direction)
	{
		nav.ResetPath ();
		nav.Move (direction);
	}

	/**
	 * Stuns the enemy for the specified time.
	 * As long as the enemy is stunned it cannot move.
	 */
	public void Stun (float stunTime)
	{
		moveTimeout = Time.time + stunTime;
	}
}

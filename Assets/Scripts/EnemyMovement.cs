using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour 
{
	private float targetDistance;

	public float enemyLookDistance;

	private Transform player;
	private NavMeshAgent nav;

	private SpriteRenderer sprite;

	public bool returnToInitialPosition;
	private Vector3 initialPosition;

	private double moveTimeout;

	private bool moveOrder;
	private Vector3 destinationOrder;

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
		if (Time.time >= moveTimeout) 
		{
			targetDistance = Vector3.Distance (player.position, transform.position);

			//Main priority is to follow the player
			if (targetDistance < enemyLookDistance) 
			{
				nav.SetDestination (player.position);

				//reset move order
				moveOrder = false;
			}
			else if (moveOrder) //if no player is in sight, follow order destination
			{
				nav.SetDestination (destinationOrder);

				//if enemy is close to destination stop move order
				if (Vector3.Distance (transform.position, destinationOrder) < 0.1f)
					moveOrder = false;
			}
			else //if no player is in sight and no order given, stand still or go back to initial position
			{
				if(returnToInitialPosition)
					nav.SetDestination (initialPosition);
				else
					nav.SetDestination (transform.position);
			}

			//Obsolete with animated sprite
			if (transform.forward.x < 0)
				sprite.flipX = true;
			else
				sprite.flipX = false;
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

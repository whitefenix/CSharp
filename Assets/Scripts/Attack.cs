﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour {

	private const int NUM_MODES = 2;
	public enum Mode {
		SINGLE = 0,
		AOE_ROW
	}

	//Used collider
	public Collider[] attackCollidersPerMode = new Collider[NUM_MODES];
	//Damage per hit
	public float[] attackDamagePerMode = new float[NUM_MODES] {
		10.0f,
		5.0f
	};
	//Hits per second
	public float[] attackSpeedPerMode = new float[NUM_MODES] {
		2.0f,
		1.0f
	};

	private Mode currentAttack = Mode.SINGLE;

	public Mode currentAttackMode {
		get 
		{  
			return currentAttack; 
		}
		set 
		{
			currentAttack = value;

			//Enable correct collider
			OnSwitchAttackMode ();
		}
	}

	private double attackTimeout;

	private List<GameObject> reachableEnemies;
	private List<GameObject> killedEnemies;

	// Use this for initialization
	void Start () 
	{
		reachableEnemies = new List<GameObject> ();
		killedEnemies = new List<GameObject> ();
	}

	void Update ()
	{
		//TODO delete DEBUG
		if (Input.GetKeyDown(KeyCode.X))
		{
//			int mode = (int)currentAttackMode;
//			mode ^= 1;
//
//			currentAttackMode = (Mode)mode;

			if (currentAttack.Equals (Mode.SINGLE))
				currentAttackMode = Mode.AOE_ROW;
			else
				currentAttackMode = Mode.SINGLE;
		}	
//	}
//	
//	// Update is called once per frame
//	void FixedUpdate () 
//	{
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

			case Mode.AOE_ROW:

				foreach (GameObject enemy in reachableEnemies) 
				{
					DealDamage (enemy);
				}
				break;
			}

			RemoveKilledEnemiesFromList ();
			attackTimeout = Time.time + (1.0f / attackSpeedPerMode[(int)currentAttackMode]);
		}
	}

	private void OnSwitchAttackMode()
	{
		int mode = (int)currentAttack;

		for(int i = 0; i < attackCollidersPerMode.Length; ++i)
		{
			attackCollidersPerMode [i].enabled = (i == mode);
		}
	}

	void OnTriggerEnter (Collider other) 
	{
		if (IsEnemyCollider(other) && !reachableEnemies.Contains (other.gameObject)) 
		{
			reachableEnemies.Add (other.gameObject);

			//Just for debugging
			//Select (other.gameObject, true);
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (IsEnemyCollider(other) && reachableEnemies.Contains (other.gameObject)) 
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

		enemyHealth.damage (attackDamagePerMode[(int)currentAttackMode]);

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

	public static bool IsEnemyCollider(Collider other)
	{
		return other.tag == "Attackable" && !other.isTrigger;
	}
}
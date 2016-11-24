using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

	private Vector3 initialPosition;

	public PlayerAttack attack;
	public Vector3 direction = Vector3.zero;
	public float maxRange;
	public int pierce;

	void Start ()
	{
		initialPosition = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Vector3.Distance (transform.position, initialPosition) > maxRange || pierce <= 0)
			Destroy (gameObject);

		transform.position += direction;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (CombatRange.IsEnemyCollider (other)) 
		{
			pierce--;

			Debug.Log ("Sphere hit:" + other.gameObject.name);

			attack.DealDamage (other.gameObject);
		} 
//		else if (!EnemyAttack.IsPlayerCollider (other)) 
//		{
//			Destroy (gameObject);
//		}
	}
}

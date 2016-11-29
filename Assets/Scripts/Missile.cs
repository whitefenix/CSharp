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
		//Destroy if it reaches its maximum distance or if it pierced through the state number of enemies
		if (Vector3.Distance (transform.position, initialPosition) > maxRange || pierce <= 0)
			Destroy (gameObject);

		transform.position += direction;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (CombatRange.IsEnemyCollider (other)) 
		{
			//Debug.Log ("Sphere hit:" + other.gameObject.name);
			pierce--;
			attack.DealDamage (other.gameObject);
		}
	}
}

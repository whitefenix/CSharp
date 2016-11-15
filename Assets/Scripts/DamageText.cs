using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {

	public float lifespan = 1.0f;
	public float speed = 0.1f;

	public Vector3 spawnPoint = new Vector3 (0, 3, 0);
	private Vector3 translation;

	// Use this for initialization
	void Start () 
	{
		transform.position += spawnPoint;

		translation = new Vector3 (0, speed, 0);

		GameObject.Destroy (gameObject, lifespan);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		gameObject.transform.position += translation;
	}
}

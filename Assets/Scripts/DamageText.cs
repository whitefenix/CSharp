using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {

	public float lifespan = 1.0f;
	public float speed = 0.1f;

	public Vector3 spawnPoint = new Vector3 (0, 3, 0);

	// Use this for initialization
	void Start () {
		GameObject.Destroy (gameObject, lifespan);

		transform.position += spawnPoint;
	}
	
	// Update is called once per frame
	void Update () {

		gameObject.transform.position += new Vector3(0, speed, 0);
	}
}

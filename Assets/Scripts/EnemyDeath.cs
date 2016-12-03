using UnityEngine;
using System.Collections;

public class EnemyDeath : MonoBehaviour {

	public float deathTime = 10;
	public GameObject death;

	void Start ()
	{
	}

	public void OnDeath()
	{
		GameObject spawned = (GameObject)Instantiate (death, transform.position, Quaternion.identity);
		Destroy (spawned, deathTime);
	}
}

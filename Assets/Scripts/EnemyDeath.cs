using UnityEngine;
using System.Collections;

public class EnemyDeath : MonoBehaviour {

	public float deathTime = 10;
	public GameObject death;

	bool isQuitting;

	void Start ()
	{
	}

	void OnApplicationQuit()
	{
		isQuitting = true;
	}
	void OnDestroy()
	{
		if (!isQuitting)
		{
			Debug.Log ("destroy");

			GameObject spawned = (GameObject)Instantiate (death, transform.position, Quaternion.identity);
			Destroy (spawned, deathTime);
		}
	}
}

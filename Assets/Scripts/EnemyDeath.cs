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
		//DOES NOT WORK ON SCENE RELOAD
		isQuitting = true;
	}

	void OnDestroy()
	{
		//DOES NOT WORK ON SCENE RELOAD
		if (!isQuitting)
		{
//			GameObject spawned = (GameObject)Instantiate (death, transform.position, Quaternion.identity);
//			Destroy (spawned, deathTime);
		}
	}
}

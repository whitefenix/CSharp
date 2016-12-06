using UnityEngine;
using System.Collections;

public class CampfireTalk : MonoBehaviour {

	public GameObject fire;

	// Use this for initialization
	void Start () {
	
	}

	void OnTalk () 
	{
		fire.SetActive (false);
	}
}

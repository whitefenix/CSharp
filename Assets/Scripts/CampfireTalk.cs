using UnityEngine;
using System.Collections;

public class CampfireTalk : MonoBehaviour {

	public GameObject fire;
	public NPCTalk talk;

	// Use this for initialization
	void Start () {
		talk = GetComponent<NPCTalk> ();
	}

	void OnTalk () 
	{
		talk.mute = true;
		fire.SetActive (false);
	}
}

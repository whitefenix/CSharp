using UnityEngine;
using System.Collections;

public class MenuMusic : MonoBehaviour {

    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        source.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (!source.isPlaying)
        {
            source.Play();
        }
	}
}

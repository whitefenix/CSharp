using UnityEngine;
using System.Collections;

public class VideoScript : MonoBehaviour {

    public MovieTexture test;
    AudioSource source;

    void Start()
    {      
        source = GetComponent<AudioSource>();
        source.clip = test.audioClip;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {


            if (test.isPlaying)
            {
                source.Pause();
                test.Pause();
            }
            else
            {
                test.Play();
                source.Play();
            }
        }
    }

}

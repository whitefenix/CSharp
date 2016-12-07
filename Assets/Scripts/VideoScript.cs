using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoScript : MonoBehaviour {

    public MovieTexture test;
    AudioSource source;

    void Start()
    {      
        source = GetComponent<AudioSource>();
        source.clip = test.audioClip;
        test.Play();
        source.Play();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            test.Stop();
            source.Stop();
            SceneManager.LoadScene("Act1_Final_Level");
        }
    }

}

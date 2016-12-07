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
		if (SkipInputTriggered())
        {
            test.Stop();
            source.Stop();
        }

		if (!test.isPlaying) 
		{
			SceneManager.LoadScene("Act1_Final_Level");
		}
    }

	private bool SkipInputTriggered() 
	{
		return Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (GlobalConstants.XBOX_BTN_A);
	}
}

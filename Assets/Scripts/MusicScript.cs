using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour {

    public AudioSource baseSource;
    public AudioSource mainSource;
    public AudioSource offSource;
    public AudioClip baseClip;
    private AudioClip mainClip;
    private AudioClip offClip;

    public float startVolume = 1.0f;
    private float currentVolume = 1.0f;

    private float musictime;
	private int enemiesInCombatRange;

	private float combatSilenceTime = 1.0f;
	private float lastCombatTime;

    private UIScript ui;

    private bool fadeIn = false;

    // Use this for initialization
    void Start () {

        GameObject theplayer = GameObject.Find("Player");
        ui = theplayer.GetComponent<UIScript>();

        musictime = 0.0f;

        //these do fuck all before initialization
        baseSource.clip = baseClip;
        mainClip = ui.mainHand.clip;
        mainSource.clip = mainClip;
        offClip = ui.offHand.clip;
        offSource.clip = offClip;

        baseSource.volume = startVolume;
        mainSource.volume = startVolume;
        offSource.volume = startVolume;

        baseSource.Play(); 
        mainSource.Play();
        offSource.Play();
    }

    //Update is called once per frame
    void Update () {
        
        if (fadeIn)
        {
            if (baseSource.volume < startVolume)
            {
               currentVolume += 0.4f * Time.deltaTime;
                baseSource.volume = currentVolume;
                mainSource.volume = currentVolume;
                offSource.volume = currentVolume;
            }
            else
            {
                fadeIn = false;
            }
        }

		if (Time.time >= lastCombatTime)
			enemiesInCombatRange = 0;

		bool fight = (enemiesInCombatRange > 0);
			
        /*
         * Calculate position in the music
         */
        float freq = (float)baseSource.clip.frequency;
        int sample = baseSource.timeSamples;
        float deltatime = (float)sample / freq;
        
        if (deltatime >= 0.5f && deltatime < baseSource.clip.length) // >= 0.5 
        {
            musictime = deltatime;
        }
        else if (deltatime >= baseSource.clip.length)
        {
            musictime = 0.0f;
            deltatime = 0.0f;
            baseSource.time = musictime;
            mainSource.time = musictime;
            offSource.time = musictime;
            baseSource.Play();
            mainSource.Play();
            offSource.Play();
        }

        //changes when fight is checked
        //TEMPORARY DISABLED: USE FOR BOSS FIGHT ONLY
        if (fight && baseSource.volume > 0.1f) //if we are fighting we get silenced
        {
       //     baseSource.volume -= 0.6f * Time.deltaTime;
       //     mainSource.volume -= 0.6f * Time.deltaTime;
       //     offSource.volume -= 0.6f * Time.deltaTime;
        }
        else if (!fight && baseSource.volume < 1.0f && fadeIn == false) //if we are not fighting we get sound back
        {
        //    currentVolume = 0.1f;
        //    fadeIn = true;
        }

        //Check for wrong clips playing. Not sure if everything here is necessary, someone should probably check. TODO.
        if (ui.mainHand.clip != mainClip) //if we havent equipped the instrument currently playing, change
        {
            mainClip = ui.mainHand.clip;
            mainSource.clip = mainClip;     
            baseSource.time = musictime;
            mainSource.time = musictime;
            offSource.time = musictime;
            baseSource.Play();
            mainSource.Play();
            offSource.Play();
            
        }
        else if (ui.offHand.clip != offClip) //if we change back, change to violin
        {
            offClip = ui.offHand.clip;
            offSource.clip = offClip;
            baseSource.time = musictime;
            mainSource.time = musictime;
            offSource.time = musictime;
            baseSource.Play();
            mainSource.Play();
            offSource.Play();
        }
	}

	void OnTriggerStay(Collider other)
	{
		if (Attack.IsEnemyCollider(other)) 
		{
			lastCombatTime = Time.time + combatSilenceTime;
		}
	}

	void OnTriggerEnter (Collider other) 
	{
		if (Attack.IsEnemyCollider(other)) 
		{
			lastCombatTime = Time.time + combatSilenceTime;

			enemiesInCombatRange++;
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (Attack.IsEnemyCollider(other)) 
		{
			enemiesInCombatRange--;
		}
	}
}
///Author(s): Samuel Ekne
///Date: 11-11-2016
///Last revision: 16-11-2016
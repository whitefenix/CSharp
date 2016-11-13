using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour {

    /*
     *TODO:
     * Crossfades when switching instead of abrupt stops
     */

    public AudioSource source1;
    public AudioSource source2;
    public AudioSource source3;
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;

    private float musictime;
	private int enemiesInCombatRange;

//	private float combatSilenceTime = 1.0f;
//	private float lastCombatTime;

    private UIScript ui;

    // Use this for initialization
    void Start () {

        musictime = 0.0f;
        source1.clip = clip1;
        source2.clip = clip2;
        source3.clip = clip3;
        source1.Play(); //1 is piano, always playing
        source2.Play(); //2 is violin, 3 is drums which are turned on when violin is off

        GameObject theplayer = GameObject.Find("Player");
        ui = theplayer.GetComponent<UIScript>();
    }
	
	//Update is called once per frame
	void Update () {

//		if (Time.time >= lastCombatTime)
//			enemiesInCombatRange = 0;

		bool fight = (enemiesInCombatRange > 0);
			
        /*
         * Calculate position in the music
         */
        float freq = (float)source1.clip.frequency;
        int sample = source1.timeSamples;
        float deltatime = (float)sample / freq;
        if (deltatime > 0.5f)
        {
            musictime = deltatime;
        }

        //changes when fight is checked
        if (fight && source1.isPlaying) //if we are fighting we get silenced
        {
            source1.Stop();
            source2.Stop();
            source3.Stop();
            source1.time = musictime;
            source2.time = musictime;
            source3.time = musictime;
        }
        else if (!fight && !source1.isPlaying) //if we are not fighting we get sound back
        {
            source1.Play();
            source2.Play();
        }

        //changes when we unequip the violin
        else if (!fight && !ui.mainHand.instrumentName.Equals("Violin") && source2.isPlaying) //if we havent equipped te violin, change to drums
        {
            source1.Stop();
            source2.Stop();
            source1.time = musictime;
            source3.time = musictime;
            source1.Play();
            source3.Play();
        }
        else if (!fight && ui.mainHand.instrumentName.Equals("Violin") && !source2.isPlaying ) //if we change back, change to violin
        {
             source1.Stop();
             source3.Stop();
             source1.time = musictime;
             source2.time = musictime;
             source1.Play();
             source2.Play();
        }
	}

	void OnTriggerEnter (Collider other) 
	{
		if (other.tag == "Attackable" && !other.isTrigger) 
		{
//			lastCombatTime = Time.time + combatSilenceTime;

			enemiesInCombatRange++;
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (other.tag == "Attackable" && !other.isTrigger) 
		{
			enemiesInCombatRange--;
		}
	}
}
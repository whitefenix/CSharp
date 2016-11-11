using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour {

    /*
     *TODO:
     * Fix restarting music (resume from where you were instead of restarting every time)
     * Crossfades when switching instead of abrupt stops
     * Maybe consider a different code structure? Not sure if this inheritance system is good or not.
     * Fix fighting to actually do something 
     */

    public AudioSource source1;
    public AudioSource source2;
    public AudioSource source3;
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;

    public PlayerController player;
    public UIScript ui;

    // Use this for initialization
    void Start () {
        source1.clip = clip1;
        source2.clip = clip2;
        source3.clip = clip3;
        source1.Play(); //1 is piano, always playing
        source2.Play(); //2 is violin, 3 is drums which are turned on when violin is off
    }
	
	// Update is called once per frame
	void Update () {
        bool fight = player.fighting;

        //changes when fight is checked
        if (fight && source1.isPlaying) //if we are fighting we get silenced
        {
            source1.Stop();
            source2.Stop();
            source3.Stop();
        }
        if (!fight && !source1.isPlaying) //if we are not fighting we get sound back
        {
            source1.Play();
            source2.Play();
        }

        //changes when we unequip the violin
        if (ui.mainHand != ui.mainInstruments[0] && source2.isPlaying) //if we havent equipped te violin, change to drums
        {
            source1.Stop();
            source2.Stop();
            source1.Play();
            source3.Play();
        }
        else if (ui.mainHand == ui.mainInstruments[0] && !source2.isPlaying ) //if we change back, change to violin
        {
            source1.Stop();
            source3.Stop();
            source1.Play();
            source2.Play();
        }


	
	}
}

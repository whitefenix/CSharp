using UnityEngine;
using System.Collections;

public class BossFight : MonoBehaviour {

    Health health;
    public GameObject littleBoss;
    private bool hasSplit = false;

	public GameObject particleSystem;

    public float waitTimeBeforeSplit = 1.0f;

	// Use this for initialization
	void Start () 
	{
	 	health = GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {

        if (health.currentHealth < health.maximumHealth / 2)
        {
			particleSystem.SetActive (true);

            if (!hasSplit)
            {
                StartCoroutine(split());               
            }
           
            //trigger split
            //destroy
        }
	
	}

    IEnumerator split()
    {
        //Debug.Log("Start");
        health.currentHealth = health.maximumHealth; //just so he doesnt die while transforming
        //TODO: Stop + particle effect
        yield return new WaitForSeconds(waitTimeBeforeSplit); //wait for 1 second before spawning
        //Debug.Log("Go");
        Vector3 position = transform.position;
        GameObject littleBoss1 = (GameObject)Instantiate(littleBoss, position, Quaternion.identity);
        position.x -= 2;
        GameObject littleBoss2 = (GameObject)Instantiate(littleBoss, position, Quaternion.identity);
        position.z -= 2;
        GameObject littleBoss3 = (GameObject)Instantiate(littleBoss, position, Quaternion.identity);
        position.x += 2;
        GameObject littleBoss4 = (GameObject)Instantiate(littleBoss, position, Quaternion.identity);
        position.x += 2;
        GameObject littleBoss5 = (GameObject)Instantiate(littleBoss, position, Quaternion.identity);
        position.z += 2;
        GameObject littleBoss6 = (GameObject)Instantiate(littleBoss, position, Quaternion.identity);
        hasSplit = true;

		health.Die ();
    }
}

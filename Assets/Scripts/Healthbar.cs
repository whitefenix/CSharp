using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {

    public GameObject healthbar;
    public GameObject player;
    Health health;
    private float maximumHealth, barHealth;

	// Use this for initialization
	void Start () {
        health = player.GetComponent<Health>();
        maximumHealth = health.maximumHealth;
    }
	
	// Update is called once per frame
	void Update () {
        barHealth = health.currentHealth / maximumHealth;
        healthbar.transform.localScale = new Vector3(barHealth, healthbar.transform.localScale.y, healthbar.transform.localScale.z);	
	}
}

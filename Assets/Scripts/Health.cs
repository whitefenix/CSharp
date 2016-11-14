﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float maximumHealth = 100;
	public float initialHealth = 100;
	private float currentHealth;

	private float lastHitTime;
	public float visualizationCooldown = 0.1f;
	private SpriteRenderer sprite;

	private bool dead;

	public GameObject valueLabel;

	public float health 
	{
		get { return currentHealth; }
	}

	public bool isDead
	{
		get { return dead; }
	}

	// Use this for initialization
	void Start () {

		sprite = GetComponentsInChildren<SpriteRenderer> () [0];
		currentHealth = Mathf.Min(initialHealth, maximumHealth);

		dead = (health <= 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		VisualizeDamage ();
	}

	public void damage(float value)
	{
		currentHealth -= value;

		Debug.Log (this.name + " is loosing Health (" + currentHealth + "/" + maximumHealth + ")");

		lastHitTime = Time.time;

		if (valueLabel != null)
			SpawnLabel (value, Color.red);

		if (currentHealth <= 0)
			Die ();
	}

	public void heal(float value)
	{
		if (!isDead) 
		{
			currentHealth = Mathf.Min (currentHealth + value, maximumHealth);

			if (valueLabel != null)
				SpawnLabel (value, Color.green);
		}
	}

	private void SpawnLabel(float value, Color color)
	{
		GameObject label = (GameObject)Instantiate (valueLabel, gameObject.transform.position, Quaternion.identity);
		TextMesh text = label.GetComponent<TextMesh> ();

		text.text = value.ToString ();
		text.color = color;
	}

	private void VisualizeDamage()
	{
		if(lastHitTime > 0)
			sprite.color = Color.Lerp (Color.white, Color.red, 1.0f / (Time.time - lastHitTime) * visualizationCooldown);
	}

	private void Die()
	{
		dead = true;

		//Debug.Log (this.name + " died!");

		//TODO play death animation
		gameObject.SetActive (false);
		GameObject.Destroy (this.gameObject, 2);
	}
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public float maximumHealth = 100;
	public float initialHealth = 100;
	private float currentHealth;

	private float lastHitTime;
	private float lastStunTime;
	public float visualizationCooldown = 0.1f;
	public float stunCooldown = 1.0f;
	private SpriteRenderer sprite;

	public Color damageColor = new Color (0.72f, 0f, 0.07f);
	public Color healColor = Color.blue;

	private bool dead;

	public GameObject valueLabel;
	public GameObject lableOrigin;

	private ParticleSystem stunParticles;

	public float health 
	{
		get { return currentHealth; }
	}

	public bool isDead
	{
		get { return dead; }
	}

	// Use this for initialization
	void Start () 
	{
		sprite = GetComponentsInChildren<SpriteRenderer> () [0];

		if (lableOrigin != null) 
		{
			stunParticles = lableOrigin.GetComponent<ParticleSystem> ();
		} 
		else 
		{
			lableOrigin = gameObject;
		}

		currentHealth = Mathf.Min(initialHealth, maximumHealth);

		dead = (health <= 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		VisualizeDamage ();
		VisualizeStun ();
	}

	public void damage(float value, bool critical = false)
	{
		currentHealth -= value;

		//Debug.Log (this.name + " is loosing Health (" + currentHealth + "/" + maximumHealth + ")");

		lastHitTime = Time.time;

		if (valueLabel != null) 
		{
			if(critical)
				SpawnLabel (value, damageColor, "Critical!");
			else
				SpawnLabel (value, damageColor);
		}

		if (currentHealth <= 0)
			Die ();
	}

	public void heal(float value)
	{
		if (!isDead) 
		{
			currentHealth = Mathf.Min (currentHealth + value, maximumHealth);

			if (valueLabel != null)
				SpawnLabel (value, healColor);
		}
	}

	private void SpawnLabel(float value, Color color, string extra = "")
	{
		GameObject label = (GameObject)Instantiate (valueLabel, lableOrigin.transform.position, Quaternion.identity);

		TextMesh text = label.GetComponent<TextMesh> ();

		text.color = color;
		text.text = value.ToString ();

		if (extra.Length > 0)
			text.text += " " + extra;
	}

	private void VisualizeDamage()
	{
		if(lastHitTime > 0)
			sprite.color = Color.Lerp (Color.white, Color.red, 1.0f / (Time.time - lastHitTime) * visualizationCooldown);
	}

	private void VisualizeStun()
	{
		if (stunParticles != null && stunParticles.isPlaying && lastStunTime < Time.time) 
		{
			stunParticles.Stop ();
		}
	}

	private void Die()
	{
		dead = true;

		//Debug.Log (this.name + " died!");

		//TODO play death animation
		gameObject.SetActive (false);
		GameObject.Destroy (this.gameObject, 2);
	}

	public void Stun (float stunDuration)
	{
		lastStunTime = Time.time + stunDuration;

		if (stunParticles != null) 
		{
			stunParticles.Play ();
		}
	}
}

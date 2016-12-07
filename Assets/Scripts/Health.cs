using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public float maximumHealth = 100;
	public float initialHealth = 100;
	[HideInInspector] 
	public float currentHealth;

	private float lastHitTime;
	private float lastStunTime;
	public float visualizationCooldown = 0.1f;
	public float stunCooldown = 1.0f;
	private SpriteRenderer sprite;

	public Color damageColor = new Color (0.72f, 0f, 0.07f);
	public Color healColor = new Color (0.8f, 1.0f, 1.0f);

	private bool dead;

	public bool showHealLabel = false;
	public bool showDamageLabel = true;
	public GameObject valueLabel;
	public GameObject lableOrigin;

	private ParticleSystem stunParticles;

	private PlayerSkills ps;

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

		if (gameObject.tag == "Player") 
		{
			ps = GetComponent<PlayerSkills> ();
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

		if (valueLabel != null && showDamageLabel) 
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


			if (valueLabel != null && showHealLabel)
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

	public void Die()
	{
		dead = true;

		gameObject.SendMessage ("OnDeath", SendMessageOptions.DontRequireReceiver);
        if (gameObject.tag != "Player")
        {
            GameObject.Destroy(gameObject);
        }
		
	}

	public void OnEqipSkillBook()
	{
		Debug.Log ("upgrade healt by " + ps.biAggr.maximumHealth);

		//biAggr is already accumulated, we don't want to double accumulate it on maximumHealth
		maximumHealth = initialHealth + ps.biAggr.maximumHealth;
		currentHealth += ps.biAggr.maximumHealth;
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

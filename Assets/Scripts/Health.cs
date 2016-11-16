using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public float maximumHealth = 100;
	public float initialHealth = 100;
	private float currentHealth;

	private float lastHitTime;
	public float visualizationCooldown = 0.1f;
	private SpriteRenderer sprite;

	public Color damageColor = new Color (0.72f, 0f, 0.07f);
	public Color healColor = Color.blue;

	private bool dead;

	public GameObject valueLabel;
	private Transform cameraCanvas;
	private Camera mainCamera;

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
		currentHealth = Mathf.Min(initialHealth, maximumHealth);

		//needed to intantiate damage labels
		GameObject go = GameObject.FindWithTag ("MainCamera");
		mainCamera =  go.GetComponent<Camera>();
		cameraCanvas = go.transform.FindChild ("Canvas");

		dead = (health <= 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		VisualizeDamage ();
	}

	public void damage(float value)
	{
		currentHealth -= value;

		//Debug.Log (this.name + " is loosing Health (" + currentHealth + "/" + maximumHealth + ")");

		lastHitTime = Time.time;

		if (valueLabel != null)
			SpawnLabel (value, damageColor);

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

	private void SpawnLabel(float value, Color color)
	{
		

		GameObject label = (GameObject)Instantiate (valueLabel, gameObject.transform.position, Quaternion.identity);

		TextMesh text = label.GetComponent<TextMesh> ();

		text.text = value.ToString ();
		text.color = color;

//		GameObject label = Instantiate (valueLabel, mainCamera.WorldToScreenPoint(gameObject.transform.position), Quaternion.identity) as GameObject;
//		label.transform.SetParent (cameraCanvas);
//
//		Text text = label.GetComponent<Text> ();
//
//		text.text = value.ToString ();
//		text.color = color;
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

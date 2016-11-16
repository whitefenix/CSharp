using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {

	public float lifespan = 1.0f;
	public float movementSpeed = 0.1f;
	public float growingSpeed = 0.1f;
	public int sortingOrder = 100;

	public Vector3 spawnPoint = new Vector3 (0, 3, 0);

	private Vector3 translation;
	private Color transparency;
	private TextMesh text;
	private float fontSize;

	// Use this for initialization
	void Start () 
	{
		text = GetComponent<TextMesh> ();

		MeshRenderer mr = GetComponent<MeshRenderer> ();
		mr.sortingOrder = sortingOrder;

		translation = new Vector3 (0, movementSpeed, 0);
		transparency = new Color (0, 0, 0, growingSpeed);

		transform.position += spawnPoint;

		GameObject.Destroy (gameObject, lifespan);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		gameObject.transform.position += translation;

		fontSize += growingSpeed;
		text.fontSize += Mathf.RoundToInt (fontSize);

		text.color -= transparency;
	}
}

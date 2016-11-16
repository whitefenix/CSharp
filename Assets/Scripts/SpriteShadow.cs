using UnityEngine;
using System.Collections;

public class SpriteShadow : MonoBehaviour {
	
	private Quaternion initialRotation;

	// Use this for initialization
	void Start () 
	{
		initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		transform.rotation = initialRotation;
	}
}

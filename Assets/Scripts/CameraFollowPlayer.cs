using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour {

	private Transform player;
	private Vector3 offset;

	public float minX = -100.0f;
	public float minZ = -100.0f;
	public float maxX = 100.0f;
	public float maxZ = 100.0f;

	// Use this for initialization
	void Start () {
	
		player = GameObject.FindWithTag ("Player").transform;
		offset = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if (player != null) 
		{
			Vector3 newPosition = player.position;
			newPosition.y = 0;
			newPosition += offset;

			newPosition.x = Mathf.Clamp (newPosition.x, minX, maxX);
			newPosition.z = Mathf.Clamp (newPosition.z, minZ, maxZ);

			//transform.position = Vector3.Lerp (transform.position, newPosition, (transform.position - newPosition).magnitude);

			transform.position = newPosition;
		}
	}
}

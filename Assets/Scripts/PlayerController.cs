using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;

	private Camera mainCamera;
	private SpriteRenderer sprite;
	public GameObject cursor;
	public GameObject arrow;

	private int layerMask;
	private Vector3 aimDir;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		sprite = this.GetComponentsInChildren<SpriteRenderer> ()[0];
		arrow = GameObject.Find ("Player/Arrow");

		//Use Terrain only
		layerMask = (1 << 8);
	}

	void Update() 
	{
		/*
		 * AIM
		 */
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 50, layerMask)) 
		{
			if(cursor.activeInHierarchy)
				cursor.transform.position = hit.point;
			
			aimDir = hit.point - transform.position;
		}
		arrow.transform.rotation = Quaternion.Euler(90, Quaternion.FromToRotation (Vector3.forward, aimDir).eulerAngles.y, 0);

		/*
		 * MOVEMENT 
		 */
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) 
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;

			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;

			if (moveDirection.x >= 0)
				sprite.flipX = true;
			else
				sprite.flipX = false;

		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
}

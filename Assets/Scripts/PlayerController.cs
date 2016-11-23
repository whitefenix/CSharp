﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 6.0f;
	public float gravity = 20.0f;

	public Vector3 aimDir;

	public bool useMouseAsAimDirection = false;
	public GameObject cursor;

	private Camera mainCamera;
	private SpriteRenderer sprite;
	private Animator animator;

	private int layerMask;

	private Vector3 moveDir = Vector3.zero;
	public Vector3 moveDirection 
	{
		get { return moveDir; }
	}

	// Use this for initialization
	void Start () 
	{
		mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		sprite = this.GetComponentsInChildren<SpriteRenderer> ()[0];
		animator = this.GetComponentsInChildren<Animator>()[0];

		//Use Terrain only, for mouse cursor raycast
		layerMask = (1 << 8);
	}

	void Update() 
	{
		/*
		 * MOVEMENT 
		 */
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) 
		{
			aimDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDir = aimDir * speed;

//			if (moveDir.x >= 0)
//				sprite.flipX = true;
//			else
//				sprite.flipX = false;
		}
		moveDir.y -= gravity * Time.deltaTime;
		controller.Move(moveDir * Time.deltaTime);

		/*
		 * AIM
		 */
		if (useMouseAsAimDirection) 
		{
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 50, layerMask)) {
				if (cursor.activeInHierarchy)
					cursor.transform.position = hit.point;
				
				aimDir = hit.point - transform.position;
				aimDir.y = 0; //to 2D direction
			}
		}

		if (aimDir != Vector3.zero) 
		{
			transform.rotation = Quaternion.Euler (0, Quaternion.FromToRotation (Vector3.forward, aimDir).eulerAngles.y, 0);
		}

		animator.SetFloat ("SpeedX", moveDir.x);
		animator.SetFloat ("SpeedZ", moveDir.z);

		animator.SetFloat ("DirectionX", transform.forward.x);
		animator.SetFloat ("DirectionZ", transform.forward.z);
	}
}

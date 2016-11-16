using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 6.0F;
	public float gravity = 20.0F;
	private Vector3 moveDir = Vector3.zero;
	public Vector3 aimDir;

	public bool useMouseAsAimDirection = false;

	private Camera mainCamera;
	private SpriteRenderer sprite;
	public GameObject cursor;
	public GameObject directionIndicator;

	private int layerMask;

	public Vector3 moveDirection {
		get { return moveDir; }
	}

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		sprite = this.GetComponentsInChildren<SpriteRenderer> ()[0];

		//Use Terrain only, for mouse cursor Raycast
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
			moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDir = transform.TransformDirection(moveDir);
			moveDir *= speed;

			if (moveDir.x >= 0)
				sprite.flipX = true;
			else
				sprite.flipX = false;

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
		else 
		{
			aimDir = moveDir;
			aimDir.y = 0; //to 2D direction
		}

		if(aimDir != Vector3.zero)
			directionIndicator.transform.rotation = Quaternion.Euler (90, Quaternion.FromToRotation (Vector3.forward, aimDir).eulerAngles.y, 0);;
	}
}

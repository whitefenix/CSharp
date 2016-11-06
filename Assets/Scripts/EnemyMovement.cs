using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	private float targetDistance;

	public float enemyLookDistance;

	private Transform player;
	private NavMeshAgent nav;

	private SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		sprite = this.GetComponentsInChildren<SpriteRenderer> ()[0];
		nav = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {

		targetDistance = Vector3.Distance (player.position, transform.position);

		if (targetDistance < enemyLookDistance) {
			nav.SetDestination (player.position);
		} else {
			nav.SetDestination (transform.position);
		}

		if (transform.forward.x < 0)
			sprite.flipX = true;
		else
			sprite.flipX = false;
	}
}

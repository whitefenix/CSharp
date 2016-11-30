using UnityEngine;
using System.Collections;

public class HealthPotion : MonoBehaviour {

	public float restoreHealth = 10;
	public float speed = 0.5f;
	public float intensity = 0.5f;

	public Transform shadow;
	public Transform sprite;

	Vector3 initialPosition;
	Vector3 initialShadowScale;

	void Start()
	{
		initialPosition = sprite.position;
		initialShadowScale = shadow.localScale;
	}

	void FixedUpdate()
	{
		sprite.position = initialPosition + Vector3.up * Mathf.Sin (Time.time * speed) * intensity;

		shadow.localScale = initialShadowScale + Vector3.one * Mathf.Sin (Time.time * speed) * -intensity;
	}

	void OnTriggerEnter(Collider other)
	{
		if (EnemyAttack.IsPlayerCollider (other)) 
		{
			Health otherHealth = other.gameObject.GetComponent<Health> ();
			otherHealth.heal (restoreHealth);

			Destroy (gameObject);
		}
	}
}

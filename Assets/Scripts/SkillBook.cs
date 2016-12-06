using UnityEngine;
using System.Collections;

public class SkillBook : MonoBehaviour {

	public BonusItem bonus;

	public float speed = 0.5f;
	public float intensity = 0.5f;

	public Transform shadow;
	public Transform sprite;

	Vector3 initialPosition;
	Vector3 initialShadowPos;

	void Start()
	{
		initialPosition = sprite.position;
		initialShadowPos = shadow.position;
	}

	void FixedUpdate()
	{
		Vector3 offset = Vector3.up * Mathf.Sin (Time.time * speed) * intensity;

		sprite.position = initialPosition + offset;
		shadow.position = initialShadowPos + offset;
		//shadow.localScale = initialShadowScale + Vector3.one * Mathf.Sin (Time.time * speed) * -intensity;
	}

	void OnTriggerEnter(Collider other)
	{
		if (EnemyAttack.IsPlayerCollider (other)) 
		{
			PlayerSkills otherPlayerSkills = other.gameObject.GetComponent<PlayerSkills> ();
			otherPlayerSkills.EquipBonusItem (bonus);

			gameObject.SendMessage ("OnCollected", SendMessageOptions.DontRequireReceiver);
			Destroy (gameObject);
		}
	}
}

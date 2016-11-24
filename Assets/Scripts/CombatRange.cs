using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatRange : MonoBehaviour {

	private const int NUM_COLLIDERS = 3;
	public enum RangeCollider {
		NONE = -1,
		SPHERE_SMALL = 0,
		BOX_WIDE = 1,
		BOX_XWIDE = 2
	}

	public Collider[] attackColliders = new Collider[NUM_COLLIDERS];
	private List<GameObject> reachableEnemies;

	// Use this for initialization
	void Start () 
	{
		reachableEnemies = new List<GameObject> ();

		SetCollider (RangeCollider.SPHERE_SMALL);
	}

	public void SetCollider (RangeCollider col)
	{
		int colIdx = (int)col;

		for(int i = 0; i < attackColliders.Length; ++i)
		{
			attackColliders [i].enabled = (i == colIdx);
		}
	}

	void OnTriggerEnter (Collider other) 
	{
		if (IsEnemyCollider(other) && !reachableEnemies.Contains (other.gameObject)) 
		{
			reachableEnemies.Add (other.gameObject);
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (IsEnemyCollider(other) && reachableEnemies.Contains (other.gameObject)) 
		{
			reachableEnemies.Remove (other.gameObject);
		}
	}

	public void RemoveEnemies(ref List<GameObject> toRemove)
	{
		if (toRemove.Count == 0)
			return;

		foreach (GameObject enemy in toRemove) 
		{
			reachableEnemies.Remove (enemy);
		}

		toRemove.Clear ();
	}

	public GameObject[] GetEnemies()
	{
		return reachableEnemies.ToArray ();
	}

	public GameObject GetNearestEnemy()
	{
		float minDist = float.MaxValue;
		float curDist;
		GameObject closest = null;

		foreach (GameObject enemy in reachableEnemies) 
		{
			//TODO Check: Comparison between enemy and collider center, compare against player center instead?
			curDist = Vector3.Distance (enemy.transform.position, transform.position);

			if (curDist < minDist) 
			{
				minDist = curDist;
				closest = enemy;
			}
		}

		return closest;
	}

	public GameObject GetEnemyInRange(Vector3 origin, Vector3 direction, float maxDistance)
	{
		Ray ray = new Ray(origin, direction.normalized);

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, maxDistance, LayerMask.NameToLayer("DefaultRaycastLayers"), QueryTriggerInteraction.Ignore)) 
		{
			if (IsAttackable (hit.collider)) 
			{
				Debug.Log (hit.collider.gameObject.name);
				return hit.collider.gameObject;
			}
		}

		return null;
	}

	public GameObject[] GetEnemiesInRange(Vector3 origin, Vector3 direction, float maxDistance, int maxEnemies)
	{
		Ray ray = new Ray(origin, direction.normalized);
		List<GameObject> hitEnemies = new List<GameObject> ();

		RaycastHit[] hits = Physics.RaycastAll (ray, maxDistance, 
			LayerMask.NameToLayer("DefaultRaycastLayers"), 
			QueryTriggerInteraction.Ignore);

		foreach (RaycastHit hit in hits) 
		{
			if (IsAttackable (hit.collider) && hitEnemies.Count < maxEnemies) 
			{
				hitEnemies.Add (hit.collider.gameObject);
			}
		}

		return hitEnemies.ToArray ();
	}

	public static bool IsAttackable(Collider other)
	{
		return other.tag == "Attackable";
	}

	public static bool IsEnemyCollider(Collider other)
	{
		return IsAttackable(other) && !other.isTrigger;
	}
}

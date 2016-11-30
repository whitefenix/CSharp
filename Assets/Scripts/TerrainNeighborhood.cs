using UnityEngine;
using System.Collections;

public class TerrainNeighborhood : MonoBehaviour {

	Terrain terrain;
	public Terrain[] neighbors = new Terrain[] { null, null, null, null };

	// Use this for initialization
	void Start () {
		terrain = GetComponent<Terrain> ();

		terrain.SetNeighbors (
			neighbors [0], 
			neighbors [1], 
			neighbors [2], 
			neighbors [3]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class Visualizer : MonoBehaviour {
	public Material[] colors;
	// Use this for initialization
	Dungeon_Generator dg;
	void Start () {
		dg = GetComponent<Dungeon_Generator> ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < dg.sizeX; i++) {
			for (int j = 0; j < dg.sizeY; j++) {
				int currentColor = dg.dungeon [i, j];
				dg.dungeonTiles [i,j].GetComponent<Renderer> ().material = colors[currentColor];
			}
		}
	}
}

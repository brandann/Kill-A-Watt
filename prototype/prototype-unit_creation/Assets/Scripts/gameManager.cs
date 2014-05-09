using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {

	
	public GameObject[] towers = null;
	public GameObject towerPrefab = null;
	
	//Use this for initialization
	void Start () {
		towers = new GameObject[4];
		for(int i = 0; i < towers.Length; i++) {
			towers[i] = Instantiate(towerPrefab) as GameObject;
		}

		towers[0].transform.position = new Vector3(-10,5,0);
		towers[1].transform.position = new Vector3(10,5,0);
		towers[2].transform.position = new Vector3(-10,-5,0);
		towers[3].transform.position = new Vector3(10,5,0);
	}
	
	// Update is called once per frame
	void Update () {

	}
}

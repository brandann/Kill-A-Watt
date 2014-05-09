using UnityEngine;
using System.Collections;
namespace global {
public class gameManager : MonoBehaviour {

	
	public GameObject[] towers = null;
	public GameObject towerPrefab = null;
	
	//Use this for initialization
	void Start () {
		towers = new GameObject[10];
		for(int i = 0; i < towers.Length; i++) {
			towers[i] = Instantiate(towerPrefab) as GameObject;
		}

		towers[0].transform.position = new Vector3(-27,-13,0);
		towers[1].transform.position = new Vector3(-27,0,0);
		towers[2].transform.position = new Vector3(-27,13,0);
		towers[3].transform.position = new Vector3(-18,6,0);
		towers[4].transform.position = new Vector3(-18,-6,0);

		towers[5].transform.position = new Vector3(27,-13,0);
		towers[6].transform.position = new Vector3(27,0,0);
		towers[7].transform.position = new Vector3(27,13,0);
		towers[8].transform.position = new Vector3(18,6,0);
		towers[9].transform.position = new Vector3(18,-6,0);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
}
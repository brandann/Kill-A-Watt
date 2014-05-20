using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Global{

public class Connection : MonoBehaviour {

	public GameObject connectionPrefab; //set by inspector
	public Dictionary<GameObject,LineRenderer> connections = new Dictionary<GameObject,LineRenderer>();
	public double connectionDistance; // change in inspector
	private ownerShip parentsOwner;
	public bool Visited;


	void Start () {
		//connectionPrefab = Resources.Load("Prefabs/ConnectionLine") as GameObject;
		findAdjacentTowers ();
		buildConnections ();
		//connectionDistance = 200;
		Visited = false;
		
	}

//	void Awake{
//
//	}
	
	// Update is called once per frame
	void Update () {
			parentsOwner = this.transform.parent.gameObject.GetComponent<Tower>().myOwner;
			updateConnectionColors ();
			buildConnections ();
	}


	private void findAdjacentTowers(){

		GameObject[] objects = GameObject.FindGameObjectsWithTag( "Tower" );
		Vector3 from = this.transform.parent.position;

		foreach (GameObject go in objects) {

			
			Vector3 to = go.transform.position;
			float dist = Vector3.Magnitude(from - to);
			if(Mathf.Abs(dist) < connectionDistance && go != this.gameObject){
	
				connections.Add(go,instantiateConnection());
	
			}

		}


	}

	private LineRenderer instantiateConnection(){

		GameObject go = (GameObject)(Instantiate(connectionPrefab, transform.position, Quaternion.Euler(0, 0, 0)));
		LineRenderer line  = go.GetComponent<LineRenderer> ();
		return line;

	}

	private void buildConnections(){

		foreach (var tower in connections) {

				tower.Value.SetPosition(0 , (this.transform.parent.position));
				tower.Value.SetPosition(1, (tower.Key.transform.position));
//				for (int i =1; i< 4; i++) {
//					Vector3 pos = Vector3.Lerp (this.transform.parent.localPosition,tower.Key.transform.localPosition, i / 100f);
//					
//					pos.x += Random.Range (-0.4f, 0.4f);
//					pos.y += Random.Range (-0.4f, 0.4f);
//					
//					tower.Value.SetPosition (i, pos);
//				}
			
			
		}
		
	}

	private void updateConnectionColors(){
			//print ("colors");
		foreach (var tower in connections) {

			if(parentsOwner == tower.Key.GetComponent<Tower>().myOwner && parentsOwner == ownerShip.Player1){
					//print ("red");
					tower.Value.SetColors(Color.red,Color.red);
			}
					//make red

			if(parentsOwner == tower.Key.GetComponent<Tower>().myOwner && parentsOwner == ownerShip.Player2){
					//print ("blue");
					tower.Value.SetColors(Color.blue,Color.blue);
			}
						//make blue

				if(ownerShip.Neutral == tower.Key.GetComponent<Tower>().myOwner || parentsOwner != tower.Key.GetComponent<Tower>().myOwner){
					//print ("grey");
					tower.Value.SetColors(Color.grey,Color.grey);
			}
							//make line grey
		}
		
	}

	
}


}

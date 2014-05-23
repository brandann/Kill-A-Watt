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
		parentsOwner = this.transform.parent.gameObject.GetComponent<Tower>().myOwner;
		findAdjacentTowers ();
		buildConnections ();
		Visited = false;
		
	}


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

				Vector3 start = new Vector3(this.transform.parent.position.x,this.transform.parent.position.y, 1);
				Vector3 end =  new Vector3(tower.Key.transform.position.x,tower.Key.transform.position.y, 1);

				tower.Value.SetPosition(0 , start);
				tower.Value.SetPosition(1, end);

		}
		
	}

	private void updateConnectionColors(){
			//print ("colors");
		foreach (var tower in connections) {

			if(parentsOwner == tower.Key.GetComponent<Tower>().myOwner && parentsOwner == ownerShip.Player1){
					//print ("red");
					tower.Value.SetColors(Color.yellow,Color.yellow);
			}
					//make red

			if(parentsOwner == tower.Key.GetComponent<Tower>().myOwner && parentsOwner == ownerShip.Player2){
					//print ("blue");
					tower.Value.SetColors(Color.magenta,Color.magenta);
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

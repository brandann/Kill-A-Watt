using UnityEngine;
using System.Collections;
namespace Global{

public class ShockTowerShoot : MonoBehaviour {

	// Use this for initialization
	float lastShot;
	float fequency = 1.5f;
	private LineRenderer line;
	private bool Collided;
	ownerShip parentsOwner;
	//private LineRenderer test;


	void Start () {
		lastShot = Time.realtimeSinceStartup;
		line = this.GetComponent<LineRenderer> ();
		//LineRenderer	test = gameObject.AddComponent<LineRenderer>();
		
		
	}
	

	void Update () {
	
		if((Time.realtimeSinceStartup - lastShot) > 1)
		lightning(this.transform.parent);
		
		parentsOwner = this.transform.parent.gameObject.GetComponent<Tower>().myOwner;
	}


   void OnTriggerStay2D(Collider2D other){
		//print ("this is being called");

		 if (parentsOwner != other.gameObject.GetComponent<unitBehavior> ().myOwner) {

				lightning (other.transform);

				if (Time.realtimeSinceStartup - lastShot > fequency) {
						lastShot = Time.realtimeSinceStartup;

						if (Network.isServer) {
								if (tag != other.gameObject.tag && other.gameObject.tag.Contains ("Unit")) {
										Network.Destroy (other.gameObject);
										//DestroyObject(other);
								}
						}

				}
		}
	}

	 private void lightning(Transform minion){
			float parentx = this.transform.parent.localPosition.x;
			float parenty = this.transform.parent.localPosition.y;
			Vector3 parentPos = new Vector3 (parentx, parenty, -1);
			//float parentLocal = 
				line.SetPosition (0, parentPos);

				for (int i =1; i< 4; i++) {
						Vector3 pos = Vector3.Lerp (this.transform.parent.localPosition, minion.localPosition, i / 4.0f);

						pos.x += Random.Range (-0.4f, 0.4f);
						pos.y += Random.Range (-0.4f, 0.4f);

						line.SetPosition (i, pos);
				}

				line.SetPosition (4, minion.localPosition);
		    
		}

}
}

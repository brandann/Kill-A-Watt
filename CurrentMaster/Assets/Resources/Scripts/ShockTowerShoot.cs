using UnityEngine;
using System.Collections;
namespace Global{

public class ShockTowerShoot : MonoBehaviour {

	// Use this for initialization
	float lastShot;
	float fequency = 1.5f;
	private LineRenderer line;
	private bool Collided;
	public ownerShip parentsOwner;
	//private LineRenderer test;


	void Start () {
		lastShot = Time.realtimeSinceStartup;
		line = this.GetComponent<LineRenderer>();
	
	}
	

	void Update () {
	
		if((Time.realtimeSinceStartup - lastShot) > 1)
		lightning(this.transform);
		
		
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
										
								}
						}

				}
		}
	}

	 private void lightning(Transform minion){
		
				line.SetPosition (0, this.transform.localPosition);

				for (int i =1; i< 4; i++) {
						Vector3 pos = Vector3.Lerp (this.transform.localPosition, minion.localPosition, i / 4.0f);

						pos.x += Random.Range (-0.4f, 0.4f);
						pos.y += Random.Range (-0.4f, 0.4f);

						line.SetPosition (i, pos);
				}

				line.SetPosition (4, minion.localPosition);
		    
		}

}
}

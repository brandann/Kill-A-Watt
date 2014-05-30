using UnityEngine;
using System.Collections;

namespace Global{

public class DeathRay : MonoBehaviour {

	public ownerShip myOwner;

		Camera cam;
	void Awake(){
		
	}

	void Start () {
			cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
	}
	
	// Add stuff to deathray later
	void Update () {

			Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//transform.rotation = Quaternion.Euler (0,0, Mathf.Atan2(position.y - transform.position.y,position.x - transform.position.x) * Mathf.Rad2Deg);

			
	}
	

	public void SwitchOwner(ownerShip switchTo)
	{

		

	}
}

}

using UnityEngine;
using System.Collections;

public class SphereBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		#region user movement control
		transform.position += Input.GetAxis ("Vertical")  * transform.up * (1 * Time.smoothDeltaTime);
		transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") * (1 * Time.smoothDeltaTime));
		#endregion
	}
}

using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {

	
	public GameObject[] towers = null;
	
	//Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject GT1Object = GameObject.Find("GT1");
		GUIText GT1Text = GT1Object.GetComponent<GUIText>();
		GameObject tower1Object = GameObject.Find("square1");
		towerBehavior tower1 = tower1Object.GetComponent<towerBehavior>();
		GT1Text.text = "" + Mathf.Abs(tower1.units);

		GameObject GT2Object = GameObject.Find("GT2");
		GUIText GT2Text = GT2Object.GetComponent<GUIText>();
		GameObject tower2Object = GameObject.Find("square2");
		towerBehavior tower2 = tower2Object.GetComponent<towerBehavior>();
		GT2Text.text = "" + Mathf.Abs(tower2.units); 

		GameObject GT3Object = GameObject.Find("GT3");
		GUIText GT3Text = GT3Object.GetComponent<GUIText>();
		GameObject tower3Object = GameObject.Find("square3");
		towerBehavior tower3 = tower3Object.GetComponent<towerBehavior>();
		GT3Text.text = "" + Mathf.Abs(tower3.units); 

		GameObject GT4Object = GameObject.Find("GT4");
		GUIText GT4Text = GT4Object.GetComponent<GUIText>();
		GameObject tower4Object = GameObject.Find("square4");
		towerBehavior tower4 = tower4Object.GetComponent<towerBehavior>();
		GT4Text.text = "" + Mathf.Abs(tower4.units); 
	}
}

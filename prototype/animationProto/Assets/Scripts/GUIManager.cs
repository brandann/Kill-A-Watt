using UnityEngine;
using System.Collections;

//In C# this is the part that confused me most too.
//	
//	But, correct way of using find and getcomponent.
//		
//		GameObject.Find("ObjectName").GetComponent<ScriptN ame>()
//		
//		GameObject Object1 = GameObject.Find("name")
//		Component Script1 = Object1.GetComponent<ScriptName>()
//		
//		Calling a function within the script: Script1.FunctionName()

//GlobalBehavior globalBehavior = GameObject.Find ("Manager").GetComponent<GlobalBehavior>();

namespace Global{

public class GUIManager : MonoBehaviour {

		gameManager Mscript;
		WorldGameState GUIstatus;

	void Start () {
			 GameObject manager = GameObject.Find ("GameManager");
			 Mscript = manager.GetComponent<gameManager>();
			 GUIstatus =  Mscript.status;

	}

	void OnGUI() {

	
		
		
		switch (GUIstatus) 
		{

			case WorldGameState.SplashScreen:
				GUI.Box(new Rect(0,0,100,90), "Test1");
					break;

			case WorldGameState.StartMenu:
				GUI.Box(new Rect(5,5,100,90), "Test");
					break;

			case WorldGameState.InGame:
					break;

			case WorldGameState.EndGame:
					break;

		}



//			if (status == WorldGameState.SplashScreen) {
//				status = WorldGameState.StartMenu;
//				
//				print ("worked");
//			}
	}
}

}

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


//var posVector = Camera.main.WorldToScreenPoint(transform.position);
//var vectorTwo : Vector2 = GUIUtility.ScreenToGUIPoint(new Vector2(posVector.x,posVector.y));

//void OnGUI()
//{
//	// 
//	GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), m_texture, ScaleMode.ScaleToFit, true);
//}

// Position the billboard in the center, 
// but respect the picture aspect ratio
//int textureHeight = GearIcon.height;
//int textureWidth = GearIcon.width;
//int screenHeight = Screen.height;
//int screenWidth = Screen.width;
//
//int screenAspectRatio = (screenWidth / screenHeight);
//int textureAspectRatio = (textureWidth / textureHeight);
//
//int scaledHeight;
//int scaledWidth;
//if (textureAspectRatio <= screenAspectRatio)
//{
//	// The scaled size is based on the height
//	scaledHeight = screenHeight;
//	scaledWidth = (screenHeight * textureAspectRatio);
//}
//else
//{
//	// The scaled size is based on the width
//	scaledWidth = screenWidth;
//	scaledHeight = (scaledWidth / textureAspectRatio);
//}
//float xPosition = screenWidth / 2 - (scaledWidth / 2);
namespace Global{

public class GUIManager : MonoBehaviour {

		#region scripts
		StateManager stateManager;
		GameManager  gameManager;
		NetworkManager networkManager;
		#endregion

		#region gamestates
		WorldGameState GUIstatus;
		WorldGameState lastState;
		#endregion

		#region textures
		public Texture GearIcon;
		public Texture Edison;
		public Texture Tesla;
		public GUISkin MySkin;
		public GUIStyle GearIconStyle;
		public GUIStyle MenuButtons;
		public GUIStyle StartMenu;
		private int selected = -1;
		private bool textBoxSelected = false;
		private bool serverWaiting = false;
		#endregion


		private float ScreenH;
		private float ScreenW;
		private bool SubMenu;


		#region score variables
		public float totalScore = 1000;
		private float currentLeft;
		public float scoreLeft;
		public float scoreRight;
		private float currentRight;
		private float StartRight;
		#endregion


		#region  Test GUI Positions
		public float testposX = 0.25f;
		public float testposY = 0.25f;

		public float testsizeX = 0.25f;
		public float testsizeY = 0.25f;

		public float testposX1 = 0.25f;
		public float testposY1 = 0.25f;
		
		public float testsizeX1 = 0.25f;
		public float testsizeY1 = 0.25f;

		public float testposX2 = 0.25f;
		public float testposY2 = 0.25f;
		
		public float testsizeX2 = 0.25f;
		public float testsizeY2 = 0.25f;
		#endregion




	void Start () {

			GameObject manager = GameObject.FindGameObjectsWithTag("MainCamera")[0];
			stateManager = manager.GetComponent<StateManager>();
			gameManager = manager.GetComponent<GameManager>();
			networkManager = manager.GetComponent<NetworkManager>();

			// need to get score from game mananger

			if(networkManager == null)
				Debug.Log("networkmanager is null");
				
			SubMenu = false;
			currentLeft = 5;
			scoreLeft = 0;
			currentRight = 5;
			scoreRight = 0;
			StartRight = (float)((ScreenW * .53) + (ScreenW * .35));

			ScreenH = Screen.height;
			ScreenW = Screen.width;

		

	}

	void OnGUI() {

		GUI.skin = MySkin;


		ScreenH = Screen.height;
		ScreenW = Screen.width;

		scoreLeft = gameManager.player1Score;
		scoreRight = gameManager.player2Score;

		GUIstatus =  stateManager.status;

		
		switch (GUIstatus) 
		{
			
			case WorldGameState.Pause:
//				GUI.Box(new Rect(testposX * Screen.width,testposY * Screen.height,
//				                 testsizeX * Screen.width,testsizeY * Screen.height), "test");
				PauseButton();
				break;

			case WorldGameState.StartMenu:
				StartMenuGUI();
				
				
//				GUI.Box(new Rect(testposX * Screen.width,testposY * Screen.height,
//				                 testsizeX * Screen.width,testsizeY * Screen.height), "test");
				break;

			case WorldGameState.InGame:

				InGameGUI();
				
//				GUI.Box(new Rect(testposX * Screen.width,testposY * Screen.height,
//				                 testsizeX * Screen.width,testsizeY * Screen.height), "test");
//				GUI.Box(new Rect(testposX1 * Screen.width,testposY1 * Screen.height,
//				                 testsizeX1 * Screen.width,testsizeY1 * Screen.height), "test");
//				GUI.Box(new Rect(testposX2 * Screen.width,testposY2 * Screen.height,
//				                 testsizeX2 * Screen.width,testsizeY2 * Screen.height), "test");
				break;

			case WorldGameState.EndGame:

				break;// not sure if we even need a gui for this state

		}



	
	}


//-----------------------------------------------------------------------------
		// there are two different pause buttons, one for in game and another for start menu
		// b/c they are slightly different. you dont want to be able to quit while you are already in the start menu 

		private void PauseButton(){

			//stateManager.status = WorldGameState.Pause;
			//lastState = stateManager.status;

			if (lastState == WorldGameState.InGame) {

				Rect PauseMenuRect = new Rect ((float)(ScreenW * .38), (float)(ScreenH * .25), (float)(ScreenW * .23), (float)(ScreenH * .27));
				GUI.Box (PauseMenuRect, "Menu",MenuButtons);


				Rect QuitButtonRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .38), (float)(ScreenW * .18), (float)(ScreenH * .05));
				if (GUI.Button (QuitButtonRect, "Quit",MenuButtons)) {
						//quitting returns the player to the start menu
						//probably need logic to exit both players
						stateManager.status = WorldGameState.Quit;
				}

				Rect ExitButtonRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .45), (float)(ScreenW * .18), (float)(ScreenH * .05));
				if (GUI.Button (ExitButtonRect, "Exit",MenuButtons)) {
						//exit the program
						//need logic to exit the other player as well
						stateManager.status = WorldGameState.Exit;
				}

				Rect ReturnButtonRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .31), (float)(ScreenW * .18), (float)(ScreenH * .05));
				if (GUI.Button (ReturnButtonRect, "Resume",MenuButtons)) {
						//returns the user back to the state they were in
						stateManager.status = WorldGameState.InGame;
				}
	
	
			}else if (lastState == WorldGameState.StartMenu) {

				Rect SPauseMenuRect = new Rect ((float)(ScreenW * .38), (float)(ScreenH * .25), (float)(ScreenW * .23), (float)(ScreenH * .23));
				GUI.Box(SPauseMenuRect, "Menu",MenuButtons);
	
				
				Rect SExitButtonRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .38), (float)(ScreenW * .18), (float)(ScreenH * .05));
				if(GUI.Button(SExitButtonRect, "Exit", MenuButtons)) {
					//exit the program
					//need logic to exit the other player as well
					stateManager.status = WorldGameState.Exit;
				}
				
				Rect SReturnButtonRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .31), (float)(ScreenW * .18), (float)(ScreenH * .05));
				if(GUI.Button(SReturnButtonRect, "Resume", MenuButtons)) {
					//returns the user back to the state they were in
					//lastState = stateManager.status;
					stateManager.status = WorldGameState.StartMenu;
				}

			}
		}
	

//-----------------------------------------------------------------------------
		private void InGameGUI(){
			//this code is to make the button a independent of resolution
			Rect MenuButtonRect = new Rect ((float)(ScreenW * .01), (float)(ScreenH - (ScreenH * .01 + (float)(ScreenW * .030))),
				                                (float)(ScreenW * .030), (float)(ScreenW * .030));
			if(GUI.Button(MenuButtonRect,"",GearIconStyle)){
				
				//set the last state to pull up the correct menu
				lastState = stateManager.status;
				stateManager.status = WorldGameState.Pause;
			}

			Rect HeroBoxLeftRect = new Rect ((float)(ScreenW * .01), (float)(ScreenH * .015), (float)(ScreenW * .1), (float)(ScreenH * .15));
			GUI.Box (HeroBoxLeftRect, Edison);

			Rect EnergyBarLeftRect = new Rect ((float)(ScreenW * .12), (float)(ScreenH * .015), (float)(ScreenW * .35), (float)(ScreenH * .06));
			GUI.Box (EnergyBarLeftRect, "Engergy Bar");

			scoreLeft/= totalScore;
			scoreLeft *= (float)(ScreenW * .35);

			float endLeft = (float)(ScreenW * .35);
			
			if (currentLeft < scoreLeft && currentLeft < endLeft) {
				
				currentLeft++;
			} 
			if (currentLeft > scoreLeft && currentLeft < endLeft) {
				
				currentLeft--;
			}

			Rect UnderLeftRect = new Rect ((float)(ScreenW * .12), (float)(ScreenH * .015), currentLeft, (float)(ScreenH * .06));
			GUI.Box (UnderLeftRect, "");

			Rect HeroBoxRightRect = new Rect ((float)(ScreenW * .89), (float)(ScreenH * .015), (float)(ScreenW * .1), (float)(ScreenH * .15));
			GUI.Box (HeroBoxRightRect, Tesla);
			
			Rect EnergyBarRightRect = new Rect ((float)(ScreenW * .53), (float)(ScreenH * .015), (float)(ScreenW * .35), (float)(ScreenH * .06));
			GUI.Box (EnergyBarRightRect, "Engergy Bar");

			scoreRight/= totalScore; //get a percent of the total score
			scoreRight *= (float)(ScreenW * .35); //multiply that percent by the size of the energy bar

			float endRight = (float)(ScreenW * .35);

			StartRight = (float)((ScreenW * .53) + (ScreenW * .35));
			StartRight -=  currentRight;

			if (currentRight < scoreRight && currentRight < endRight && !(currentRight >= endRight)) {

				currentRight++;
				//StartRight++;
			} 
			else if (currentRight > scoreRight) {

				//StartRight = (float)((ScreenW * .53) + (ScreenW * .35));
				currentRight--;
				//StartRight +=  currentRight;
				//StartRight--;
			}
			StartRight = (float)((ScreenW * .53) + (ScreenW * .35));
			StartRight -=  currentRight;

			Rect UnderRightRect = new Rect (StartRight, (float)(ScreenH * .015), currentRight, (float)(ScreenH * .06));
			GUI.Box (UnderRightRect, "");
	
		}

//-----------------------------------------------------------------------------
		private void StartMenuGUI(){


			Rect PlayButtonRect = new Rect ((float)(ScreenW * .03), (float)(ScreenH * .76),
			                                (float)(ScreenW * .2), (float)(ScreenH * .1));

			SubMenu =  GUI.Toggle(PlayButtonRect, SubMenu, "Play",MenuButtons);
			if (SubMenu && !serverWaiting) {
				StartSubMenu();
			}

			Rect MenuButtonRect = new Rect ((float)(ScreenW * .03), (float)(ScreenH - (ScreenH * .03 + ScreenW * .05)),
			                                (float)(ScreenW * .05), (float)(ScreenW * .05));
			if(GUI.Button (MenuButtonRect,"",GearIconStyle)){
				
				//set the last state to pull up the correct menu
				lastState = stateManager.status;
				stateManager.status = WorldGameState.Pause;
			}

			if (serverWaiting) {
				Rect EditBoxRect = new Rect ((float)(ScreenW * .395), (float)(ScreenH * .31), (float)(ScreenW * .2), (float)(ScreenH * .25));
				GUI.Box(EditBoxRect,"waiting for someone to join");
			}
		}

//-----------------------------------------------------------------------------
		private void StartSubMenu(){
			Rect SubMenuBox = new Rect ((float)(ScreenW * .03), (float)(ScreenH * .22), (float)(ScreenW * .4), (float)(ScreenH * .54));
			GUI.Box (SubMenuBox,"",MenuButtons);

			Rect SubSubMenuBox = new Rect ((float)(ScreenW * .06), (float)(ScreenH * .32), (float)(ScreenW * .33), (float)(ScreenH * .37));
			GUI.Box (SubSubMenuBox,"");


			Rect CreateButtonRect = new Rect ((float)(ScreenW * .06), (float)(ScreenH * .25),
			                                (float)(ScreenW * .15), (float)(ScreenH * .05));

			if (GUI.Button (CreateButtonRect, "Create",MenuButtons) && textBoxSelected == false) {
				stateManager.status = WorldGameState.InGame;
				networkManager.StartServer();
				textBoxSelected = true;

			}

			if (textBoxSelected) {
				Rect EditBoxRect = new Rect ((float)(ScreenW * .395), (float)(ScreenH * .31), (float)(ScreenW * .2), (float)(ScreenH * .25));
				GUI.Box(EditBoxRect,"",MenuButtons);

				Rect TextFieldRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .38), (float)(ScreenW * .18), (float)(ScreenH * .05));
				//print(networkManager.gameName);
				networkManager.gameName = GUI.TextField(TextFieldRect,networkManager.gameName,25);

				Rect OkayBoxRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .45), (float)(ScreenW * .07), (float)(ScreenH * .05));
				if(GUI.Button(OkayBoxRect,"OK",MenuButtons)){
					//networkManager.StartServer();
					stateManager.status = WorldGameState.InGame;
					textBoxSelected = false;
					//serverWaiting = true;
				}

				Rect CancelBoxRect = new Rect ((float)(ScreenW * .49), (float)(ScreenH * .45), (float)(ScreenW * .07), (float)(ScreenH * .05));
				if(GUI.Button(CancelBoxRect,"Cancel",MenuButtons)){
					textBoxSelected = false;
				}

			}

		


			Rect JoinButtonRect = new Rect ((float)(ScreenW * .24), (float)(ScreenH * .25),
			                                  (float)(ScreenW * .15), (float)(ScreenH * .05));


			if(networkManager.hostList != null){
			//Debug.Log("hostList != null");
			for (int i = 0; i < networkManager.hostList.Length; i++)
			{
				Rect hostButtonRect = new Rect((float)(ScreenW * .06),(float)((ScreenH * .32) + (ScreenH * .05 * i)), 
					                               (float)(ScreenW * .33), (float)(ScreenH * .05));

				if(i == selected){
					GUI.Button(hostButtonRect, networkManager.hostList[i].gameName,"selected");
				}
				else if (GUI.Button(hostButtonRect, networkManager.hostList[i].gameName)){
						selected = i;
				}
				if (GUI.Button (JoinButtonRect, "Join",MenuButtons) && textBoxSelected == false) {
						networkManager.JoinServer(networkManager.hostList[selected]);
				}
			}
			}
		

		}
		
}



}

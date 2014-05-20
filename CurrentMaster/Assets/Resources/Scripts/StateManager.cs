using UnityEngine;
using System.Collections;


namespace Global{


	public enum WorldGameState {
		SplashScreen,
		//possible story before start menu that can be quit out of 
		StartMenu,
		InGame,
		Quit,
		Exit,
		EndGame,
		Pause
		//possible stat screen at the end,
	}

	public class StateManager : MonoBehaviour {

	#region variables for splash
	private Color color1;
	private Color color2;
	public float duration = 100.0f;
	private float deltaTime  = 0.0f;
	private float startTime;  //2.0f
	#endregion
	//player 1
	//player 2
	// networking passed to networkManager?
	// global score
	// current game state
	// switch game state function


	//player class is a container class for all the objects manager needs to know about
	// ID?
	// their score
	// number of towers they have
	// number of minions?
	 //private player player1;
	// private player player2;
	 private Camera MainCamera;
	 public WorldGameState status;


	// this controls camera pos and passes the state to the GUI manager

	void Start () {
        status = WorldGameState.StartMenu;
		//player1 = new player();
		//player2 = new player();
		MainCamera = Camera.main;
		color1 = MainCamera.backgroundColor;
		color2 = Color.white;


	}
	

	void Update () {

			switch (status) 
			{
				
			case WorldGameState.SplashScreen:
				SplashScreenState();
				break;
				
			case WorldGameState.StartMenu:
				StartMenuState();
				break;
				
			case WorldGameState.InGame:
				InGameState();
				break;
				
			case WorldGameState.EndGame:
				EndGameState();
				break;

			case WorldGameState.Quit:
				QuitState();
				break;

			case WorldGameState.Exit:
				ExitState();
				break;

			case WorldGameState.Pause:
				PauseState();
				break;
			
				
			}
	
	}

//---------------------------------------------------------------------------
// plays the logo animation and fades the camera color from grey to white for 6.25 seconds
	private void SplashScreenState(){
			MainCamera.transform.position = new Vector3 (0, 0, -10);

			deltaTime += Time.deltaTime * 0.08f;
			if(deltaTime < duration)
			{
				camera.backgroundColor = Color.Lerp(color1, color2, deltaTime);
			}
			if (Time.realtimeSinceStartup > 5.45) {
				status = WorldGameState.StartMenu;
			}

	}

//---------------------------------------------------------------------------

	private void StartMenuState(){
			MainCamera.transform.position = new Vector3 (100, 0, -10);
			//can we just have a play button and internally decide who start the sever?
		
		
	}

//---------------------------------------------------------------------------

	private void InGameState(){
			MainCamera.transform.position = new Vector3 (200, 0, -10);
			MainCamera.orthographicSize = 18;
			//gui needs to know about the score of each player
			//also about they number of minions in each tower
		
	}

//---------------------------------------------------------------------------
	
	private void EndGameState(){
		
		
		
	}

//---------------------------------------------------------------------------
	
	private void QuitState(){
		
			StartMenuState();
		
	}

//---------------------------------------------------------------------------
	
	private void ExitState(){
		
		
		
	}

//---------------------------------------------------------------------------
	
	private void PauseState(){
		//stop all logic, maybe turn the rest of the screen grey
		
		
	}


}

}

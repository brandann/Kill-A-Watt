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
		Tutorial,
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
	private bool started = true;
	public bool tutorialStarted = true;
	#endregion

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

			case WorldGameState.Tutorial:
				TutorialState();
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
			camera.backgroundColor = Color.grey;
			//can we just have a play button and internally decide who start the sever?
		
		
	}

//---------------------------------------------------------------------------

	private void InGameState(){
			if (started) {
				MainCamera.transform.position = new Vector3 (200, 0, -10);
				MainCamera.orthographicSize = 18;
				started = false;

			}
			ScrollCamera();
	
	}

//---------------------------------------------------------------------------
	
	private void EndGameState(){
			MainCamera.transform.position = new Vector3 (200, 0, -10);
		
		
	}

//---------------------------------------------------------------------------
	
	private void QuitState(){
		
			StartMenuState();
		
	}

//---------------------------------------------------------------------------
	
	private void ExitState(){
		
			Application.Quit();
		
	}

//---------------------------------------------------------------------------
	
	private void PauseState(){
		//stop all logic, maybe turn the rest of the screen grey
		
		
	}

	private void TutorialState(){
			if (tutorialStarted) {
				MainCamera.transform.position = new Vector3 (0, -100, -10);
				tutorialStarted = false;
			}
		
		
	}
	public void CameraPosTutorial(int pos){
			//if(MainCamera.transform.position.x 
			float currentPos = MainCamera.transform.position.x;
			print ("current pos  = " + currentPos);
			currentPos += pos;
			MainCamera.transform.position = new Vector3 (currentPos, -100, -10);
	}
	
	public float scrollSpeed;
	public float scrollDistance;

	private void ScrollCamera(){
			float wheelInput = MainCamera.transform.position.y;
			 wheelInput+= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
			float newWhellPos = Mathf.Clamp(wheelInput, -scrollDistance, scrollDistance);
			MainCamera.transform.position = new Vector3 (200, newWhellPos, -10);
	}


}

}

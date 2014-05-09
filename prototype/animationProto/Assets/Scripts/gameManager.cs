using UnityEngine;
using System.Collections;


namespace Global{


	public enum WorldGameState {
		SplashScreen,
		//possible story before start menu that can be quit out of 
		StartMenu,
		InGame,
		EndGame
		//possible stat screen at the end,
	}

	public class gameManager : MonoBehaviour {


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
	 private player player1;
	 private player player2;
	 private Camera MainCamera;
	 public WorldGameState status;


	// this controls camera pos and passes the state to the GUI manager

	void Start () {
		status = WorldGameState.StartMenu;
		player1 = new player();
		player2 = new player();
		MainCamera = Camera.main;
	}
	

	void Update () {

			switch (status) 
			{
				
			case WorldGameState.SplashScreen:
				break;
				
			case WorldGameState.StartMenu:
				break;
				
			case WorldGameState.InGame:
				break;
				
			case WorldGameState.EndGame:
				break;
				
			}
	
	}


}

}

using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {

	private enum GameState{menu,game,pause};
	private GameState CurrentGameState;

	// Start
	// Use this for initialization
	//------------------------------------------------------------------------------------
	void Start () {
		StartMenu();
	}

	// Update
	// Update is called once per frame
	//------------------------------------------------------------------------------------
	void Update () {

		switch (CurrentGameState) { 
		case GameState.menu:
			UpdateMenu();
			break;
		case GameState.game:
			UpdateGame();
			break ; 
		case GameState.pause:
			UpdatePause();
			break ; 
		}

	}

	// UpdateMenu
	// update to call when gamestate is in menu
	//------------------------------------------------------------------------------------
	private void UpdateMenu() {

	}

	// StartMenu
	// initilize the menu items and game state
	//------------------------------------------------------------------------------------
	public void StartMenu() {
		CurrentGameState = GameState.menu;
		this.transform.position = new Vector3 (-666, -666, -10);

	}

	// UpdateGame
	// update to call when gamestate is in game
	//------------------------------------------------------------------------------------
	private void UpdateGame() {

		//set tower unit values
		towerValue ();
	}

	// StartGame
	// initilize the game items and game state
	//------------------------------------------------------------------------------------
	public void StartGame() {
		CurrentGameState = GameState.game;

	}

	// UpdatePause
	// update to call when gamestate is paused
	//------------------------------------------------------------------------------------
	private void UpdatePause() {

	}

	// StartPause
	// initilize the Pause items and game state
	//------------------------------------------------------------------------------------
	public void StartPause() {
		CurrentGameState = GameState.pause;

	}

	// towerValue
	// display a text value to reprensent each towers current capacity
	//------------------------------------------------------------------------------------
	private void towerValue() {

	}
}

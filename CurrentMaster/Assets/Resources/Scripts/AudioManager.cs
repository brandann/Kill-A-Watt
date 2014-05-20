using UnityEngine;
using System.Collections;
namespace Global {
	public class AudioManager : MonoBehaviour {

		private WorldGameState currentGameState;
		private StateManager stateManager;

		//-----------------------------------------------------------------------------
		// Use this for initialization
		void Start () {
			GameObject manager = GameObject.FindGameObjectsWithTag("MainCamera")[0];
			stateManager = manager.GetComponent<StateManager>();
			currentGameState = stateManager.status;
		}

		#region BackGround Music
		//-----------------------------------------------------------------------------
		// Update is called once per frame
		void Update () {

			// gradually increment the volume to bring the player into the experince 
			// withour startling them!
			if (currentVolume < setVolume) {
				currentVolume += volumeIncrement;
				audio.volume = currentVolume;
			}

			if (currentGameState == stateManager.status) {
				// if currentGameState == stateManager.status then the game state has not
				// changed, so the audio does not need to change. return to break out of
				// update function.
				return;
			}

			// if the gamestate changes then my music will always change!
			// set the volume to zero and change my gamestate.
			currentGameState = stateManager.status;
			Debug.Log (currentGameState);
			audio.volume = 0;
			currentVolume = 0;

			switch (currentGameState) {
			case(WorldGameState.EndGame):
				playEndGame();
				break;
			case(WorldGameState.Exit):
				playExit();
				break;
			case(WorldGameState.InGame):
				playInGame();
				break;
			case(WorldGameState.Pause):
				playPause();
				break;
			case(WorldGameState.Quit):
				playQuit();
				break;
			case(WorldGameState.SplashScreen):
				playSplashScreen();
				break;
			case(WorldGameState.StartMenu):
				playStartMenu();
				break;
			}
		}

		// music tracks
		public AudioClip musicLightBackground;
		public AudioClip musicMediumBackground;

		// volume levels
		private float volumeLight = .03f;
		private float volumeMedium = .2f;
		private float currentVolume;
		private float setVolume;
		private float volumeIncrement = .001f;

		//-----------------------------------------------------------------------------
		// switch to end game background music
		private void playEndGame(){
			// what is this state used for? during the death ray shot?
		}

		//-----------------------------------------------------------------------------
		// switch to exit sound effect
		private void playExit(){
			// what iis this state used for?
		}

		//-----------------------------------------------------------------------------
		// switch to game background music
		private void playInGame(){
			// play medium background music @ 50% during gameplay
			audio.clip = musicMediumBackground;
			setVolume = volumeMedium;
			audio.Play();
		}

		//-----------------------------------------------------------------------------
		// switch to pause menu background music
		private void playPause(){
			// play medium background music @ small% during pause
			audio.clip = musicMediumBackground;
			setVolume = volumeLight;
			audio.Play();
		}

		//-----------------------------------------------------------------------------
		// switch to quit sound effect
		private void playQuit(){
			// what is this state used for?
		}

		//-----------------------------------------------------------------------------
		// switch to splash screen background music
		private void playSplashScreen(){
			// play light background music at 50% during the splash screen
			audio.clip = musicLightBackground;
			setVolume = volumeLight;
			audio.Play();
		}

		//-----------------------------------------------------------------------------
		// switch to start menu background music
		private void playStartMenu(){
			// if the menu is entered from the splash screen do not change the music
			if (currentGameState == WorldGameState.SplashScreen) {
				return;
			}

			// if the menu is entered from any other screen, change the music to
			// light background music @ 50%
			audio.clip = musicLightBackground;
			setVolume = volumeLight;
			audio.Play();
		}
		#endregion
		
		#region SoundClips

		public AudioClip clipMinionCollision;
		public AudioClip clipTower0TakeOver;
		public AudioClip clipTower1TakeOver;
		public AudioClip clipTower2TakeOver;
		
		//-----------------------------------------------------------------------------
		// play a sound via the network when 2 minions collide
		public void playMinionCollision() {
			Debug.Log ("playMinionCollision");
			networkView.RPC("RPCplayMinionCollision", RPCMode.AllBuffered);
		}
		[RPC] private void RPCplayMinionCollision() {
			Debug.Log ("RPCplayMinionCollision");
			audio.PlayOneShot (clipMinionCollision);
		}
		
		//-----------------------------------------------------------------------------
		// play a sound via the network when the tower becomes neutural controlled
		public void playTowerTakeover0(){
			Debug.Log ("playTowerTakeover0");
			networkView.RPC("RPCplayTowerTakeover0", RPCMode.AllBuffered);
		}
		[RPC] public void RPCplayTowerTakeover0() {
			Debug.Log ("RPCplayTowerTakeover0");
			audio.PlayOneShot (clipTower0TakeOver);
		}
		
		//-----------------------------------------------------------------------------
		// play a sound via the network when the tower becomes player1 controlled
		public void playTowerTakeover1(){
			Debug.Log ("playTowerTakeover1");
			networkView.RPC("RPCplayTowerTakeover1", RPCMode.AllBuffered);
		}
		[RPC] public void RPCplayTowerTakeover1() {
			Debug.Log ("RPCplayTowerTakeover1");
			audio.PlayOneShot (clipTower1TakeOver);
		}
		
		//-----------------------------------------------------------------------------
		// play a sound via the network when the tower becomes player2 controlled
		public void playTowerTakeover2(){
			Debug.Log ("playTowerTakeover2");
			networkView.RPC("RPCplayTowerTakeover2", RPCMode.AllBuffered);
		}
		[RPC] public void RPCplayTowerTakeover2() {
			Debug.Log ("RPCplayTowerTakeover2");
			audio.PlayOneShot (clipTower2TakeOver);
		}
		
		#endregion
	}
}
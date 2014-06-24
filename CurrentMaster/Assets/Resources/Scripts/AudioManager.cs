using UnityEngine;
using System.Collections;

namespace Global {

    public class AudioManager : MonoBehaviour {

        private WorldGameState currentGameState;
        private StateManager stateManager;

        #region system
        void Start () {
            GameObject manager = GameObject.FindGameObjectsWithTag("MainCamera")[0];
            stateManager = manager.GetComponent<StateManager>();
            currentGameState = WorldGameState.SplashScreen;
        }

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
        #endregion

        #region BackGround Music
        #region AudioClips
        // music tracks
        public AudioClip musicLightBackground;
        public AudioClip musicMediumBackground;
        public AudioClip musicHardBackground;
        #endregion

        #region volume
        // volume levels
        private float volumeLight = .1f;
        private float volumeMedium = .3f;
        private float currentVolume;
        private float setVolume;
        private float volumeIncrement = .001f;
        #endregion

        // switch to end game background music
        private void playEndGame(){
            // what is this state used for? during the death ray shot?
        }

        // switch to exit sound effect
        private void playExit(){
            // what iis this state used for?
        }

        // switch to game background music
        private void playInGame(){
            // play medium background music @ 50% during gameplay
            audio.clip = musicHardBackground;
            setVolume = volumeMedium;
            currentVolume = 0;
            audio.Play();
        }

        // switch to pause menu background music
        private void playPause(){
            // play medium background music @ small% during pause
        }

        // switch to quit sound effect
        private void playQuit(){
            // what is this state used for?
        }

        // switch to splash screen background music
        private void playSplashScreen(){
            // play light background music at 50% during the splash screen
        }

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
        #region soundClips
        public AudioClip clipMinionCollision;
        public AudioClip clipTower0TakeOver;
        public AudioClip clipTower1TakeOver;
        public AudioClip clipTower2TakeOver;
        public AudioClip clipMagnet;
        public AudioClip clipBomb;
        public AudioClip clipShield;
        public AudioClip clipShockTower;
        public AudioClip clipGUI;
        #endregion

        #region GUIclick
        public void playGUI() {
            Debug.Log ("playGUI");
            audio.PlayOneShot (clipGUI);
        }
        #endregion

        #region ShockTower
        public void playShockTower() {
            Debug.Log ("playShockTower");
            networkView.RPC("RPCplayShockTower", RPCMode.AllBuffered);
        }
        
        [RPC] private void RPCplayShockTower() {
            Debug.Log ("RPCplayShockTower");
            audio.PlayOneShot (clipShockTower);
        }
        #endregion

        #region Shield
        public void playShield() {
            Debug.Log ("playShield");
            networkView.RPC("RPCplayShield", RPCMode.AllBuffered);
        }
        
        [RPC] private void RPCplayShield() {
            Debug.Log ("RPCplayShield");
            audio.PlayOneShot (clipShield);
        }
        #endregion

        #region Bomb
        public void playBomb() {
            Debug.Log ("playBomb");
            networkView.RPC("RPCplayBomb", RPCMode.AllBuffered);
        }
        
        [RPC] private void RPCplayBomb() {
            Debug.Log ("RPCplayBomb");
            audio.PlayOneShot (clipBomb);
        }
        #endregion

        #region Magnet
        public void playMagnet() {
            Debug.Log ("playMagnet");
            networkView.RPC("RPCplayMagnet", RPCMode.AllBuffered);
        }
        
        [RPC] private void RPCplayMagnet() {
            Debug.Log ("RPCplayMagnet");
            audio.PlayOneShot (clipMagnet);
        }
        #endregion

        #region MinionCollision
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
        #endregion

        #region TowerTakeOver
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
        #endregion
    }
}
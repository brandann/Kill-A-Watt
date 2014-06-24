using UnityEngine;
using System.Collections;

namespace Global{
    
    public enum WorldGameState {
        SplashScreen,
        StartMenu,
        InGame,
        Quit,
        Exit,
        EndGame,
        Tutorial,
        Purgatory,
        Pause
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
            MainCamera = Camera.main;
            color1 = MainCamera.backgroundColor;
            color2 = Color.white;
        }
	
        void Update () {
            switch (status) {
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
                case WorldGameState.Purgatory:
                    PurgatoryState();
                    break;
            }
        }

        //---------------------------------------------------------------------------
        // plays the logo animation and fades the camera color from grey to white for 6.25 seconds
        private void SplashScreenState(){
            MainCamera.transform.position = new Vector3 (0, 0, -10);
            deltaTime += Time.deltaTime * 0.08f;
            if(deltaTime < duration) {
                camera.backgroundColor = Color.Lerp(color1, color2, deltaTime);
            }
            if (Time.realtimeSinceStartup > 5.45) {
                status = WorldGameState.StartMenu;
            }
        }

        //---------------------------------------------------------------------------
        private void StartMenuState(){
            MainCamera.orthographicSize = 18;
            MainCamera.transform.position = new Vector3 (100, 0, -10);
            camera.backgroundColor = Color.grey;
        }

        //---------------------------------------------------------------------------
        private void InGameState(){
            if (started) {
                MainCamera.transform.position = new Vector3 (200, 0, -10);
                MainCamera.orthographicSize = 19;
                started = false;
            }
            ScrollCamera();
        }

        //---------------------------------------------------------------------------
        private void EndGameState(){
        
        }

        //---------------------------------------------------------------------------
        private void QuitState(){
            status = WorldGameState.StartMenu;
        }

        //---------------------------------------------------------------------------
        private void ExitState(){
            Application.Quit();
        }

        //---------------------------------------------------------------------------
        private void PauseState(){

        }

        private void TutorialState(){
            if (tutorialStarted) {
                MainCamera.transform.position = new Vector3 (0, -100, -10);
                tutorialStarted = false;
            }
        }

        private void PurgatoryState(){

        }

        public void CameraPosTutorial(int pos){
            float currentPos = MainCamera.transform.position.x;
            print ("current pos  = " + currentPos);
            currentPos += pos;
            MainCamera.transform.position = new Vector3 (currentPos, -100, -10);
        }
        
        private float scrollSpeed = .5f;
        public float scrollDistance;

        private void ScrollCamera(){
            float wheelInput = MainCamera.transform.position.y;

            if(Input.GetKey(KeyCode.W))
                wheelInput+= 1 * scrollSpeed;
            else if(Input.GetKey(KeyCode.S))
                wheelInput-= 1 * scrollSpeed;
            float newWhellPos = Mathf.Clamp(wheelInput, -scrollDistance, scrollDistance);
            MainCamera.transform.position = new Vector3 (200, newWhellPos, -10);
        }
    }
}
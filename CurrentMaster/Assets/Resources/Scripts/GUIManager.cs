using UnityEngine;
using System.Collections;


namespace Global{

    public class GUIManager : MonoBehaviour {

        #region scripts
        StateManager stateManager;
        GameManager  gameManager;
        NetworkManager networkManager;
        ScientistAbility scientistAbility;
        #endregion

        #region gamestates
        WorldGameState GUIstatus;
        WorldGameState lastState;
        #endregion

        #region textures
        public Texture GearIcon;
        public Texture Edison;
        public Texture Tesla;
        public Texture recticle; //used to aim magnet ability
        public GUIStyle LightningBolt;
        public GUIStyle LeftEnergyBar;
        public GUIStyle RightEnergyBar;
        public GUIStyle EnergyBarBorder;
        public GUIStyle GearIconStyle;
        public GUIStyle MenuButtons;
        public GUIStyle StartMenu;
        public GUIStyle PlayButton;
        public GUIStyle JoinButton;
        public GUIStyle CreateButton;
        public GUIStyle MenuBox;
        public GUIStyle Sub_MenuBox;
        public GUIStyle DropDown;
        public GUIStyle selectedStyle;
        public GUIStyle LeftEnergyBarFilled;
        public GUIStyle RightEnergyBarFilled;
        public GUIStyle CountRight;
        public GUIStyle CountLeft;
        public GUIStyle CountDownBox;
        public GUIStyle CountDownScreen;
        public GUIStyle ShieldButton;
        public GUIStyle BombButton;
        public GUIStyle MagnetButton;
        public GUIStyle Crosshairs;
        public GUIStyle cancelAbility;
        public GUIStyle tutorialButton;
        private int selected = -1;
        private bool textBoxSelected = false;
        private bool serverWaiting = false;
        private bool serverSelected = false;
        public Vector2 scrollPosition = Vector2.zero;
        #endregion

        //audio
        AudioManager audioManager;

        private float ScreenH;
        private float ScreenW;
        private bool SubMenu;

        #region score variables
        private float totalScore = 750;
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
            scientistAbility = manager.GetComponent<ScientistAbility> ();
            audioManager = GameObject.Find ("Main Camera").GetComponent<AudioManager> ();

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
            ScreenH = Screen.height;
            ScreenW = Screen.width;
            scoreLeft = gameManager.player1Score;
            scoreRight = gameManager.player2Score;
            if (gameManager.player1Score > totalScore)
              gameManager.player1Score = totalScore;
            if (gameManager.player2Score > totalScore)
              gameManager.player2Score = totalScore;
            GUIstatus =  stateManager.status;
            switch (GUIstatus) {
                case WorldGameState.Tutorial:
                    TutorialButtons();
                    break;
                case WorldGameState.Pause:
                    PauseButton();
                    break;
                case WorldGameState.StartMenu:
                    StartMenuGUI();
                    break;
                case WorldGameState.InGame:
                    InGameGUI();
                    break;
                case WorldGameState.Purgatory:
                    PurgatoryGUI();
                    break;
                case WorldGameState.EndGame:
                    EndGameGUI();
                    break;
            }
        }

        private bool startCountDown = false;
        int count = 1;

        private void PurgatoryGUI(){
            Rect ThingRect = new Rect ((float)(ScreenW * .03), (float)(ScreenH * .25), (float)(ScreenW * .5), (float)(ScreenH * .5));
            GUI.Box (ThingRect, "",CountDownScreen);
            if (!startCountDown) {
              startCountDown = true;
              StartCoroutine(countDown());
            }
            Rect ThingRect2 = new Rect ((float)(ScreenW * .4175), (float)(ScreenH * .58), (float)(ScreenW * .1), (float)(ScreenH * .15));
            GUI.Box (ThingRect2,count.ToString(),CountDownBox);
          }

        IEnumerator countDown() {
            for (int i = 5; i >= 1; i--) {
                count = i;
                yield return new WaitForSeconds(1f);
            }
            if (Network.isServer) {
                gameManager.SpawnTowers ();
            }
            stateManager.status = WorldGameState.InGame;
        }

        //-----------------------------------------------------------------------------
        // there are two different pause buttons, one for in game and another for start menu
        // b/c they are slightly different. you dont want to be able to quit while you are already in the start menu 
        private void PauseButton(){
            if (lastState == WorldGameState.InGame) {
                Rect PauseMenuRect = new Rect ((float)(ScreenW * .38),
                    (float)(ScreenH * .25), (float)(ScreenW * .23), (float)(ScreenH * .27));
                GUI.Box (PauseMenuRect, "",MenuButtons);
                Rect QuitButtonRect = new Rect ((float)(ScreenW * .405),
                    (float)(ScreenH * .38), (float)(ScreenW * .18), (float)(ScreenH * .05));
                if (GUI.Button (QuitButtonRect, "Quit",MenuButtons)) {
                    //quitting returns the player to the start menu
                    //probably need logic to exit both players
                    audioManager.playGUI();
                    Network.Disconnect();
                    serverWaiting = false;
                }
                Rect ExitButtonRect = new Rect ((float)(ScreenW * .405),
                    (float)(ScreenH * .45), (float)(ScreenW * .18), (float)(ScreenH * .05));
                if (GUI.Button (ExitButtonRect, "Exit",MenuButtons)) {
                    //exit the program
                    //need logic to exit the other player as well
                    audioManager.playGUI();
                    stateManager.status = WorldGameState.Exit;
                }
                Rect ReturnButtonRect = new Rect ((float)(ScreenW * .405),
                    (float)(ScreenH * .31), (float)(ScreenW * .18), (float)(ScreenH * .05));
                if (GUI.Button (ReturnButtonRect, "Resume",MenuButtons)) {
                    //returns the user back to the state they were in
                    audioManager.playGUI();
                    stateManager.status = WorldGameState.InGame;
                }
            }
            else if (lastState == WorldGameState.StartMenu) {
                Rect SPauseMenuRect = new Rect ((float)(ScreenW * .38), (float)(ScreenH * .25), (float)(ScreenW * .23), (float)(ScreenH * .23));
                GUI.Box(SPauseMenuRect, "",MenuButtons);
                Rect SExitButtonRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .38), (float)(ScreenW * .18), (float)(ScreenH * .05));
                if(GUI.Button(SExitButtonRect, "Exit", MenuButtons)) {
                    //exit the program
                    //need logic to exit the other player as well
                    audioManager.playGUI();
                    stateManager.status = WorldGameState.Exit;
                }
                Rect SReturnButtonRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .31), (float)(ScreenW * .18), (float)(ScreenH * .05));
                if(GUI.Button(SReturnButtonRect, "Resume", MenuButtons)) {
                    //returns the user back to the state they were in
                    //lastState = stateManager.status;
                    audioManager.playGUI();
                    stateManager.status = WorldGameState.StartMenu;
                }
            }
        }
      
        //-----------------------------------------------------------------------------
        private void InGameGUI(){
            //this code is to make the button a independent of resolution
            if(Network.isServer && gameManager.player1Score == totalScore)
                abilityButtons ();
            if (Network.isClient && gameManager.player2Score == totalScore)
                abilityButtons ();
            Rect MenuButtonRect = new Rect ((float)(ScreenW * .01), (float)(ScreenH - (ScreenH * .01 + (float)(ScreenW * .030))),
                                            (float)(ScreenW * .030), (float)(ScreenW * .030));
            if(GUI.Button(MenuButtonRect,"",GearIconStyle)){
                //set the last state to pull up the correct menu
                audioManager.playGUI();
                lastState = stateManager.status;
                stateManager.status = WorldGameState.Pause;
            }

            scoreLeft/= totalScore;
            scoreLeft *= (float)(ScreenW * .35);
            float endLeft = (float)(ScreenW * .35);
            if (currentLeft < scoreLeft && currentLeft < endLeft) {
                currentLeft++;
            } 
            if (currentLeft > scoreLeft ) {
                currentLeft--;
            }
            Rect UnderLeftRect = new Rect ((float)(ScreenW * .02),
                    (float)(ScreenH * .015), currentLeft, (float)(ScreenH * .05));
            
            // change the style if the energy bar is filled
            if(gameManager.player1Score < totalScore)
                GUI.Box (UnderLeftRect, "" , LeftEnergyBar);
            else if( gameManager.player1Score == totalScore)
                GUI.Box (UnderLeftRect, "" , LeftEnergyBarFilled);
            Rect ScoreCountLeftRect = new Rect ((float)(ScreenW * .025),
                                               (float)(ScreenH * .07), (float)(ScreenW * .1), (float)(ScreenH * .05));
            if(gameManager.player1Score != totalScore)
                GUI.Box (ScoreCountLeftRect,gameManager.player1Score.ToString()+ " / "+totalScore.ToString(), CountLeft);
            else if(gameManager.player1Score == totalScore)
                GUI.Box (ScoreCountLeftRect,"Ready!", CountLeft);
            Rect EnergyBarLeftRect = new Rect ((float)(ScreenW * .02),
                                                (float)(ScreenH * .015), (float)(ScreenW * .35), (float)(ScreenH * .05));
            GUI.Box (EnergyBarLeftRect, "",EnergyBarBorder);
            scoreRight/= totalScore; //get a percent of the total score
            scoreRight *= (float)(ScreenW * .35); //multiply that percent by the size of the energy bar
            float endRight = (float)(ScreenW * .35);
            StartRight = (float)((ScreenW * .53) + (ScreenW * .35));
            StartRight -=  currentRight;
            if (currentRight < scoreRight && currentRight < endRight && !(currentRight >= endRight)) {
                currentRight++;
            } 
            else if (currentRight > scoreRight) {
                currentRight--;
            }
            StartRight = (float)((ScreenW * .63) + (ScreenW * .35));
            StartRight -=  currentRight;

            Rect UnderRightRect = new Rect (StartRight, (float)(ScreenH * .015), currentRight, (float)(ScreenH * .05));
            // change the style if the energy bar is filled
            if(gameManager.player2Score < totalScore)
                GUI.Box (UnderRightRect, "",RightEnergyBar);
            else if(gameManager.player2Score == totalScore)
                GUI.Box (UnderRightRect, "",RightEnergyBarFilled);
            Rect ScoreCountRightRect = new Rect ((float)(ScreenW * .875),
                                                (float)(ScreenH * .07), (float)(ScreenW * .1), (float)(ScreenH * .05));
            if(gameManager.player2Score != totalScore)
                GUI.Box (ScoreCountRightRect,gameManager.player2Score.ToString()+" / "+totalScore.ToString(), CountRight);
            else if(gameManager.player2Score == totalScore)
                GUI.Box (ScoreCountRightRect,"Ready!", CountRight);
            Rect EnergyBarRightRect = new Rect ((float)(ScreenW * .63),
                                                (float)(ScreenH * .015), (float)(ScreenW * .35), (float)(ScreenH * .05));
            GUI.Box (EnergyBarRightRect, "", EnergyBarBorder);
        }

        //-----------------------------------------------------------------------------
        private void StartMenuGUI(){
            Rect TutorialStart = new Rect ((float)(ScreenW * .79), (float)(ScreenH - (ScreenH * .03 + ScreenW * .05)),
                                            (float)(ScreenW * .2), (float)(ScreenH * .1));
                                            
            if(GUI.Button (TutorialStart,"",tutorialButton)){
                audioManager.playGUI();
                stateManager.status = WorldGameState.Tutorial;
            }
            Rect PlayButtonRect = new Rect ((float)(ScreenW * .03), (float)(ScreenH * .76),
                                            (float)(ScreenW * .2), (float)(ScreenH * .1));

            SubMenu =  GUI.Toggle(PlayButtonRect, SubMenu, "",PlayButton);
            if (SubMenu && !serverWaiting) {
                StartSubMenu();
            }
            Rect MenuButtonRect = new Rect ((float)(ScreenW * .03), (float)(ScreenH - (ScreenH * .03 + ScreenW * .05)),
                                            (float)(ScreenW * .05), (float)(ScreenW * .05));
                                            
            if(GUI.Button (MenuButtonRect,"",GearIconStyle)){
                //set the last state to pull up the correct menu
                audioManager.playGUI();
                lastState = stateManager.status;
                stateManager.status = WorldGameState.Pause;
            }
            if (serverWaiting) {
                Rect EditBoxRect = new Rect ((float)(ScreenW * .395), (float)(ScreenH * .31), (float)(ScreenW * .2), (float)(ScreenH * .25));
                GUI.Box(EditBoxRect,"waiting for someone to join",MenuButtons);
            }
        }

        //-----------------------------------------------------------------------------
        private void StartSubMenu(){
            Rect SubMenuBox = new Rect ((float)(ScreenW * .03), (float)(ScreenH * .22), (float)(ScreenW * .4), (float)(ScreenH * .54));
            GUI.Box (SubMenuBox,"",MenuBox);

            Rect SubSubMenuBox = new Rect ((float)(ScreenW * .06), (float)(ScreenH * .32), (float)(ScreenW * .33), (float)(ScreenH * .37));
            GUI.Box (SubSubMenuBox,"",Sub_MenuBox);
            
            Rect CreateButtonRect = new Rect ((float)(ScreenW * .06), (float)(ScreenH * .25),
                                            (float)(ScreenW * .15), (float)(ScreenH * .05));

            if (GUI.Button (CreateButtonRect, "",CreateButton) && textBoxSelected == false) {				
                textBoxSelected = true;
                audioManager.playGUI();
            }

            if (textBoxSelected) {
                Rect EditBoxRect = new Rect ((float)(ScreenW * .395), (float)(ScreenH * .31), (float)(ScreenW * .2), (float)(ScreenH * .25));
                GUI.Box(EditBoxRect,"",MenuButtons);

                Rect TextFieldRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .38), (float)(ScreenW * .18), (float)(ScreenH * .05));
              
                networkManager.gameName = GUI.TextField(TextFieldRect,networkManager.gameName,25);

                Rect OkayBoxRect = new Rect ((float)(ScreenW * .405), (float)(ScreenH * .45), (float)(ScreenW * .07), (float)(ScreenH * .05));
                if(GUI.Button(OkayBoxRect,"OK",MenuButtons)){
                    networkManager.StartServer(); // comment out to skip client waiting
                    textBoxSelected = false; // comment out to skip client waiting
                    serverWaiting = true; // comment out to skip client waiting
                    audioManager.playGUI();
                }

                Rect CancelBoxRect = new Rect ((float)(ScreenW * .49), (float)(ScreenH * .45), (float)(ScreenW * .07), (float)(ScreenH * .05));
                if(GUI.Button(CancelBoxRect,"Cancel",MenuButtons)){
                    textBoxSelected = false;
                    audioManager.playGUI();
                }
            }

            Rect JoinButtonRect = new Rect ((float)(ScreenW * .24), (float)(ScreenH * .25),
                                            (float)(ScreenW * .15), (float)(ScreenH * .05));

            if (GUI.Button (JoinButtonRect, "",JoinButton) && textBoxSelected == false && serverSelected == true) {
                networkManager.JoinServer(networkManager.hostList[selected]);
                audioManager.playGUI();
            }
            
            Rect scrollViewRect = new Rect ((float)(ScreenW * .06), (float)(ScreenH * .32), (float)(ScreenW * .33), (float)(ScreenH * .37));
            Rect subScrollView = new Rect ((float)(ScreenW * .06), (float)(ScreenH * .32), (float)(ScreenW * .30), ScreenH);

            scrollPosition = GUI.BeginScrollView(scrollViewRect, scrollPosition, subScrollView, false,true);

            if(networkManager.hostList != null){
                for (int i = 0; i < networkManager.hostList.Length; i++)
                {
                    Rect hostButtonRect = new Rect((float)(ScreenW * .08),(float)((ScreenH * .32) + (ScreenH * .05 * i)), 
                                                     (float)(ScreenW * .31), (float)(ScreenH * .05));

                    if(i == selected){
                        GUI.Button(hostButtonRect, networkManager.hostList[i].gameName,selectedStyle);
                    }
                    else if (GUI.Button(hostButtonRect, networkManager.hostList[i].gameName,DropDown)){
                        selected = i;
                        serverSelected = true;
                        audioManager.playGUI();
                    }
                }
            }
            GUI.EndScrollView();
        }
      
        //-----------------------------------------------------------------------------
        float timer;
        bool once = false;
        private void EndGameGUI(){
            // dumb buttons for end game alpha test 
            if (gameManager.player1HasAllTowers) {
                Rect SPauseMenuRect = new Rect ((float)(ScreenW * .38), 
                                                (float)(ScreenH * .25), (float)(ScreenW * .23), (float)(ScreenH * .23));
                GUI.Box(SPauseMenuRect, "Player 1 Wins! ",MenuButtons);
            }
            if (gameManager.player2HasAllTowers) {
                Rect SPauseMenuRect = new Rect ((float)(ScreenW * .38), 
                                                (float)(ScreenH * .25), (float)(ScreenW * .23), (float)(ScreenH * .23));
                GUI.Box(SPauseMenuRect, "Player 2 Wins! ",MenuButtons);
            }
            if (!once) {
                once = true;
                timer = Time.realtimeSinceStartup;
            }

            if (Time.realtimeSinceStartup > timer + 5)
                backToStart ();
        }

        private void backToStart() {
            Network.Disconnect ();
        }
        
        //-----------------------------------------------------------------------------
        private void TutorialButtons(){
            // dumb buttons for end game alpha test 

            Rect Previous = new Rect ((float)(ScreenW*.47), 
                                      (float)(ScreenH* .005), (float)(ScreenW * .06), (float)(ScreenH * .1));
            if (GUI.Button (Previous, "Previous")) {
                stateManager.CameraPosTutorial(-100);
                audioManager.playGUI();
            }
        
            Rect Next = new Rect ((float)(ScreenW*.47 + (ScreenW * .06)), 
                                  (float)(ScreenH* .005), (float)(ScreenW * .06), (float)(ScreenH * .1));
            if(GUI.Button (Next,"Next")){
                stateManager.CameraPosTutorial(100);
                audioManager.playGUI();
            }

            Rect Finished = new Rect ((float)(ScreenW*.47 + (ScreenW * .12)), 
                                      (float)(ScreenH* .005), (float)(ScreenW * .06), (float)(ScreenH * .1));
            if (GUI.Button (Finished, "Done")) {
                stateManager.status = WorldGameState.StartMenu;
                stateManager.tutorialStarted = true;
                audioManager.playGUI();
            }
        }
    //-----------------------------------------------------------------------------
        private void abilityButtons(){
            if (scientistAbility.currentAbility != ScientistAbility.ability.ability0) 
            {
                Rect Ability0 = new Rect ((float)(ScreenW * .40), 
                                          (float)(ScreenH * .015), (float)(ScreenW * .06), (float)(ScreenH * .09));
                if (GUI.Button (Ability0, "", ShieldButton)|| Input.GetKey("1")) {
                    audioManager.playGUI();
                    if (Network.isServer && gameManager.player1Score == totalScore) {
                        scientistAbility.setAbility0 (ownerShip.Player1);
                    } else if (Network.isClient && gameManager.player2Score == totalScore) {
                        scientistAbility.setAbility0 (ownerShip.Player2);
                    }
                }
            }
            else                         
            {
                Rect Rclick = new Rect ((float)(ScreenW * .40), 
                                        (float)(ScreenH * .12), (float)(ScreenW * .20), (float)(ScreenH * .03));
                GUI.Box(Rclick,"Right Click to Cancel",cancelAbility);
            }

          if (scientistAbility.currentAbility != ScientistAbility.ability.ability1) {
              Rect Ability1 = new Rect ((float)(ScreenW * .47), 
                                        (float)(ScreenH * .015), (float)(ScreenW * .06), (float)(ScreenH * .09));
              if (GUI.Button (Ability1, "", BombButton) || Input.GetKey("2")) {
                  audioManager.playGUI();
                  if (Network.isServer && gameManager.player1Score == totalScore) {
                      scientistAbility.setAbility1 (ownerShip.Player1);
                  } else if (Network.isClient && gameManager.player2Score == totalScore) {
                      scientistAbility.setAbility1 (ownerShip.Player2);
                  }
              }
          }
          else {
              Rect Rclick = new Rect ((float)(ScreenW * .40), 
                                      (float)(ScreenH * .12), (float)(ScreenW * .20), (float)(ScreenH * .03));
              GUI.Box(Rclick,"Right Click to Cancel",cancelAbility);
          }
          if (scientistAbility.currentAbility != ScientistAbility.ability.ability2) {
              Rect Ability2 = new Rect((float)(ScreenW * .54), 
                                      (float)(ScreenH * .015), (float)(ScreenW * .06), (float)(ScreenH * .09));
              if (GUI.Button(Ability2, "",MagnetButton)|| Input.GetKey("3")) {
                  audioManager.playGUI();
                  if (Network.isServer && gameManager.player1Score == totalScore) {
                      scientistAbility.setAbility2(ownerShip.Player1);
                  }
                  else if (Network.isClient && gameManager.player2Score == totalScore) {
                      scientistAbility.setAbility2(ownerShip.Player2);
                  }
              }
              }
              else { //Ability 2 is active so draw targiting recticle
                  Rect Rclick = new Rect ((float)(ScreenW * .40), 
                                          (float)(ScreenH * .12), (float)(ScreenW * .20), (float)(ScreenH * .03));
                  GUI.Box(Rclick,"Right Click to Cancel",cancelAbility);
              }
        }
    }
}
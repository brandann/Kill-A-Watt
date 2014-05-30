using UnityEngine;
using System.Collections;
namespace Global{
	public class Tower : MonoBehaviour
    {
        #region Other GameObjects
        public GameManager Manager;
		private Camera sceneCam;                                        //Needed to draw GUI labels centered in world cordinates
        private SpriteRenderer myRender;
        private Transform myPlusTen; 
        #endregion

        #region Tower Properties
		public bool Visited;
        public ownerShip myOwner;                                       //Player this tower belongs go
        public int units;                                               //Number of garrisoned units should be set at runtime
        public bool selected = false;
		public GUIStyle GUIplayer1;
		public GUIStyle GUIplayer2;
		public GUIStyle GUIneutral;
        
        #endregion
        
        #region Unit Variables
		private const float MAXUNITS = 50;
        public GameObject Player1UnitPrefab = null, Player2UnitPrefab = null;
        public float percentOfUnitsPerAttack = 0.5f;                           
        //When and how long units are added to the garrison
        private float lastUnitGeneratedTime = 0;
        public float unitIncrementRate = 2;
		private float unitSpawnRate = .5f;                    //Time between unit spawns in seconds
		public int attackedDamage;
        
        
        //Used to add particle like unit spawning
		private int randIntervalMin;
		private int randIntervalMax;
		private float randIntervalNorm;
		//private int randSpawnRotation = 3;
		//private float randSizeMin = .5f;
		//private float randSizeMax = 1;
        
        private int unitsToSend;               //Units left to send from last attack
        public Quaternion destination;
        
		#endregion

        #region Sprites 
		private Sprite  neutralSprite = null, 
						player1Sprite = null, 
						player2Sprite = null, 
						player1SelectdSprite = null, 
						player2SelectdSprite = null;
        #endregion	

        void Awake () {
			Visited = false;

			Manager = GameObject.Find ("Main Camera").GetComponent<GameManager>();
			GameObject go = GameObject.FindGameObjectWithTag("MainCamera");
			sceneCam = go.camera;
		

			Player1UnitPrefab = Resources.Load("Prefabs/Player1Unit") as GameObject;
            Player2UnitPrefab = Resources.Load("Prefabs/Player2Unit") as GameObject;
            myRender = (SpriteRenderer)renderer;
            myPlusTen = transform.FindChild("TenPlusPlus");
			if (this.name == "tower(Clone)") {
								
				neutralSprite = Resources.Load ("Textures/Tower/GeneratorNeutral", typeof(Sprite)) as Sprite;
				player1Sprite = Resources.Load ("Textures/Tower/GeneratorYellow", typeof(Sprite)) as Sprite;
				player2Sprite = Resources.Load ("Textures/Tower/GeneratorBlue", typeof(Sprite)) as Sprite;
				player1SelectdSprite = Resources.Load ("Textures/Tower/GeneratorYellowSelected", typeof(Sprite)) as Sprite;
				player2SelectdSprite = Resources.Load ("Textures/Tower/GeneratorBlueSelected", typeof(Sprite)) as Sprite;

			} else if (this.name == "ShockTower(Clone)") {

				neutralSprite = Resources.Load ("Textures/Tower/ShockTowerNeutral", typeof(Sprite)) as Sprite;
				player1Sprite = Resources.Load ("Textures/Tower/ShockTowerYellow", typeof(Sprite)) as Sprite;
				player2Sprite = Resources.Load ("Textures/Tower/ShockTowerBlue", typeof(Sprite)) as Sprite;
				player1SelectdSprite = Resources.Load ("Textures/Tower/ShockTowerYellowSelected", typeof(Sprite)) as Sprite;
				player2SelectdSprite = Resources.Load ("Textures/Tower/ShockTowerBlueSelected", typeof(Sprite)) as Sprite;
			}

			
								
		}


	

//-----------------------------------------------------------------------------------------------------------------------------------------

        void FixedUpdate()
        {
            //Client should not call fixed update units passed via synchronized calls
            if (Network.isClient)
                return;

            //Increment garrisoned units on a constant interval
            if ((Time.realtimeSinceStartup - lastUnitGeneratedTime) > unitIncrementRate)
            {   
                if(myOwner != ownerShip.Neutral){
                    if(units < MAXUNITS)
						units++;
				}
                lastUnitGeneratedTime = Time.realtimeSinceStartup;
            }            
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------

        //Left mouse must go down and up on same collider to call this; used to toggle tower selection
        void OnMouseUpAsButton()
        {
            //Debug.Log("Left Clicked");
            if (Network.isServer && myOwner == ownerShip.Player1)
            {
                ToggleSelect();
                updateSprite();
            }
            else if (Network.isClient && myOwner == ownerShip.Player2)
            {
                networkView.RPC("ToggleSelect", RPCMode.Server);                
            }
            
        }
//-------------------------------------------------------------------------------------------------------------------------------------------------

        [RPC]
        public void ToggleSelect(){
            selected = (selected == true) ? false : true;           
        }
//-------------------------------------------------------------------------------------------------------------------------------------------------

		#region SelectionSprite


        public void updateSprite()
        {
            switch (myOwner)
            {
                case ownerShip.Neutral:
                    myRender.sprite = neutralSprite;
                    break;
                case ownerShip.Player1:
                    if (selected && Network.isServer)
                        myRender.sprite = player1SelectdSprite;
                    else
                        myRender.sprite = player1Sprite;
                    break;
                case ownerShip.Player2:
                    if (selected && Network.isClient)
                        myRender.sprite = player2SelectdSprite;
                    else
                        myRender.sprite = player2Sprite;
                    break;
                default:
                    Debug.LogError("Invalid ownership in updateSprite");
                    break;
            }


        }
//--------------------------------------------------------------------------------------------------------------------------------------------------
		#endregion
		
        void OnMouseOver()
        {
            //Only looking for Right clicks
            //Maybe a point of optomization later on; currently this function is called every time the mouse passes over a tower
            if (Input.GetMouseButtonDown(1))
            {
                if(Network.isServer)                                                              //Server initiates an attack
                    Manager.AttackToward(transform.position, ownerShip.Player1);  
                else if(Network.isClient )                                                       //Client asks the server to initiate an attack
                    networkView.RPC("SpawnAttackForMe",RPCMode.Server, transform.position);
             }                

        }
//--------------------------------------------------------------------------------------------------------------------------------------------------
		
		#region attack
		
        /// <summary>
        /// Spawns units from this tower and sends them toward the target position. This doesn't actually have to be an attack as reinforcements work the same way.
        /// This coroutine waits for unitSpawnRate seconds between spawning units. The number to spawn is determined by the number of units in the tower when called
        /// multiplied by percentOfUnitsPerAttack
        /// </summary>
        /// <param name="targetPos">Position to send units toward</param>
        /// <returns></returns>
        public IEnumerator SpawnAttack(Vector3 targetPos)
        {
            //Debug.Log("spawn Attack, selected: " + selected);


            //Select the proper unit prefab to spawn
            GameObject prefabToSpawn = (myOwner == ownerShip.Player1) ? Player1UnitPrefab : Player2UnitPrefab;

            //Calculate the point at which the units should spawn (just outside the tower in the proper direction)
            Vector3 vecToTarget = targetPos - transform.position;                                 //line between source and target
            Vector3 spawnPoint = vecToTarget;                                                     //a point on the line to target just outside the collider of the tower and unit
            spawnPoint.Normalize();
            spawnPoint *= (transform.localScale.x + prefabToSpawn.transform.localScale.x);        //could be more efficient math here but this is what I came up with to scale         
			spawnPoint *= .5f;
			spawnPoint = new Vector3(spawnPoint.x + transform.position.x, spawnPoint.y + transform.position.y, 0);   //then translate



            int unitsToSend = (int)(units * percentOfUnitsPerAttack);                            //Calculate the number of units to spawn
            
            ownerShip myOwnerWhenStarted = myOwner;                                              //Ownership could change while SpawnAttack is sleeping           
           
            
            //Keep sending till all units are sent or the tower runs out of units or switches sides
            while (unitsToSend > 0 && units > unitsToSend && myOwner == myOwnerWhenStarted)
            {
                //Create a unit and decrement
                GameObject go = (GameObject)Network.Instantiate(prefabToSpawn, spawnPoint, Quaternion.LookRotation(Vector3.forward, vecToTarget), 0);
                unitBehavior spawnedUnit = go.GetComponent<unitBehavior>(); //set the owner of the new unit
				spawnedUnit.destination = targetPos;
                spawnedUnit.myOwner = myOwner;
                unitsToSend--;
                units--;                
                yield return new WaitForSeconds(unitSpawnRate); //wait to spawn another
            }			
        
		}
        /// <summary>
        /// Just calls spawn attack for the client. Could have made spawnattack an rpc but that would have meant passing extra pramaters across the network 
        /// and they're always the same value when called from the client anyway
        /// </summary>
        /// <param name="pos"> Position of the tower that was right clicked</param>
        [RPC]
        void SpawnAttackForMe(Vector3 pos)
        {
            //Hard coding player2 as they are always the client
            Manager.AttackToward(pos, ownerShip.Player2);
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------

		#endregion
		
        void OnTriggerEnter2D(Collider2D other) 
        {
            //This function should only be running on the client as it changes game state
            if (Network.isClient)
                return;

			Vector3 target = other.gameObject.GetComponent<unitBehavior>().destination;
			Vector3 here = this.transform.position;
			Vector3 distance = target - here;
		//	print (distance.magnitude);
			if(distance.magnitude > 2){
				return;
			}

            ownerShip otherOwner = other.gameObject.GetComponent<unitBehavior>().myOwner;
            if (myOwner == otherOwner)                
                units++;
            else
            {
				units -= attackedDamage;
				if (units < 0)                   //Can happen when multiple units hit at same time; might watnt to use math.clamp
                    units = 0;
                if (units == 0)                  //Switch control when all units are lost
                    SwitchOwner(otherOwner);
            }

            if(other.gameObject.tag.Contains("Unit") && Network.isServer){
            	other.gameObject.GetComponent<unitBehavior>().makeBurst();
				Network.Destroy(other.gameObject);
            }
                
        }
//----------------------------------------------------------------------------------------------------------------------------------------------


        public void SwitchOwner(ownerShip switchTo)
        {
            //Selection can't carry over when tower switches owner
            selected = false;

            myOwner = switchTo;
            switch (switchTo)
            {
                case ownerShip.Neutral:
                    myRender.sprite = neutralSprite;
                    break;
                case ownerShip.Player1:
                    myRender.sprite = player1Sprite;
                    break;
                case ownerShip.Player2:
                    myRender.sprite = player2Sprite;
                    break;
                default:
                    Debug.LogError("Switching to invalid owner type");
                    break;
            }
            updateSprite();
        }


//-------------------------------------------------------------------------------------------------------------------------------


        //Called at network sendrate to sync selected variables
        void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {

            int ownedBy = 0;
            if (stream.isWriting)
            {
                
                stream.Serialize(ref units);
                ownedBy = (int)myOwner;              //Cant pass our enum must pass Unity objects or primitives
                stream.Serialize(ref ownedBy);
                stream.Serialize(ref selected);

            }
            else                                    //Client side read units and ownership if changed
            {
                
                stream.Serialize(ref units);
                                
                stream.Serialize(ref ownedBy);
                myOwner = (ownerShip)ownedBy;
                stream.Serialize(ref selected);
                updateSprite();

            }
        }
//--------------------------------------------------------------------------------------------------------------------------------------
        #region EnergyGenAnimation

        /// <summary>
        /// Called from the server's gamemanager to start the plus ten energy animation
        /// </summary>
        [RPC]
        public void PlayPlusTen()
        {
            print("called plus ten");
            myPlusTen.GetComponent<Animator>().enabled = true;
            myPlusTen.GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(turnOffAnimation());


        }
        IEnumerator turnOffAnimation()
        {

            yield return new WaitForSeconds(2f);
            myPlusTen.GetComponent<Animator>().enabled = false;
            myPlusTen.GetComponent<SpriteRenderer>().enabled = false;

        }

        #endregion

        //-------------------------------------------------------------------------------------------
		
        //Display # of garrisoned units above the tower
		void OnGUI()
		{
			Vector3 screenPos = sceneCam.WorldToScreenPoint(transform.position);
                switch (myOwner)
                {
                    case (ownerShip.Neutral):
                        GUI.contentColor = Color.grey;
						GUI.Label(new Rect(screenPos.x - 9, sceneCam.pixelHeight - screenPos.y - 60, 25, 50),  units.ToString(),GUIneutral);
                        break;
                    case (ownerShip.Player1):
                       // GUI.contentColor = Color.yellow;
						GUI.Label(new Rect(screenPos.x - 9, sceneCam.pixelHeight - screenPos.y - 60, 25, 50),  units.ToString(),GUIplayer1);
                        break;
                    case (ownerShip.Player2):
                      //  GUI.contentColor = Color.magenta;
						GUI.Label(new Rect(screenPos.x - 9, sceneCam.pixelHeight - screenPos.y - 60, 25, 50),  units.ToString(),GUIplayer2);
                        break;

                }
                //todo remove these magic numbers
							
		}
	
    }
}

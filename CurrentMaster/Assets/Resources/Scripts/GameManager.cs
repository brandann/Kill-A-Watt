using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Global
{
    public enum ownerShip { Neutral, Player1, Player2 };
    
    public class GameManager : MonoBehaviour
    {
        #region LevelLayoutVariables
        
        //location and type of towermarkers placed on the scene in the heiarchy
		private Dictionary<Vector3, ownerShip> mappedTowers = new Dictionary<Vector3, ownerShip>();
		private Dictionary<Vector3, ownerShip> mappedShocks = new Dictionary<Vector3, ownerShip>();
        private Dictionary<Vector3, ownerShip> mappedRoots = new Dictionary<Vector3, ownerShip>();
        
        //List of actuall ingame towers and their positions
        public Dictionary<Vector3, Tower> towerLookup = new Dictionary<Vector3, Tower>();
        
        //Default unit count for each of the 3 factions        
        public int NeutralStartingUnits = 15;
        public int Player1StartingUnits = 10;
        public int Player2StartingUnits = 10;
        
        private GameObject towerPrefab;
		private GameObject shockPrefab;
		private GameObject rootPrefab;

        #endregion

        //Each Player's score
        public float player1Score = 0;
        public float player2Score = 0;
        //Used to sync scores as network sync runs at diff speed than update
    

        void Start()
        {
			towerPrefab = Resources.Load("Prefabs/tower") as GameObject;
			shockPrefab = Resources.Load("Prefabs/ShockTower") as GameObject;
			rootPrefab = Resources.Load ("Prefabs/RootNode") as GameObject;
            //Find all the locations that towers should spawn at from markers
            BuildTowerLocations();
        }

//---------------------------------------------------------------------------------------------------------------------------------

        #region updateScore
        private Queue<GameObject> TowerQ = new Queue<GameObject>();
        private float regenSpeed = 1f;
        private float lastUpdate;

        public void calculateScore()
        {
            GameObject[] rootArray = GameObject.FindGameObjectsWithTag("RootNode");

            foreach (GameObject go in rootArray)
            {
                if (go != null)
                {
                    //print ("this is being called");
                    BFS(go);
                }
            }

            GameObject[] towerArray = GameObject.FindGameObjectsWithTag("Tower");

            foreach (GameObject go in towerArray)
            {
                go.GetComponentInChildren<Connection>().Visited = false;
            }

        }

        public void BFS(GameObject root)
        {

            TowerQ.Enqueue(root.gameObject);

            while (TowerQ.Count != 0)
            {
                GameObject currentNode = TowerQ.Dequeue(); //remove the first element

                ownerShip myOwner = currentNode.GetComponent<Tower>().myOwner; // get the owner of current
                currentNode.GetComponentInChildren<Connection>().Visited = true; // mark the node as visited

                Dictionary<GameObject, LineRenderer> adjacent = currentNode.GetComponentInChildren<Connection>().connections;

                foreach (var node in adjacent)
                {
                    ownerShip childOwner = node.Key.GetComponent<Tower>().myOwner; // get the owner of the child node

                    if (myOwner == childOwner && node.Key.GetComponentInChildren<Connection>().Visited == false)
                    {
                        // player1Score += node.Key.GetComponent<Tower>().units;
                        //if(Time.realtimeSinceStartup > lastUpdate + regenSpeed){ // calculate every second / not update

                        //lastUpdate = Time.realtimeSinceStartup;

                        // each tower has a flat regen speed of 5 per second
                        if (root.GetComponent<Tower>().myOwner == ownerShip.Player1)
                        {
                            player1Score += .005f;

                        }
                        if (root.GetComponent<Tower>().myOwner == ownerShip.Player2)
                        {
                            player2Score += .005f;
                            //print (player2Score);
                        }
                        //}

                        TowerQ.Enqueue(node.Key);
                    }
                }
            }
            //	print (player1Score);
        }
        #endregion

//---------------------------------------------------------------------------------------------------------------------------------

        //Adds up all of the units in each players' towers to calculate score
        void FixedUpdate()
        {
			if (Network.isClient)
				return;

			calculateScore ();

           // player1Score = player2Score = 0; //start counting from 0


//            foreach (var item in towerLookup)
//            {        
//                
//                if (item.Value.myOwner == ownerShip.Player1)              
//                    player1Score += item.Value.units;
//                else
//                    if (item.Value.myOwner == ownerShip.Player2)
//                        player2Score += item.Value.units;
//            }
           
        }
//--------------------------------------------------------------------------------------------------------------------------------

        //Called from individual towers to notify all of the same player's towers to attack a certain location
        public void AttackToward(Vector3 targetPosition, ownerShip attackingPlayer)
        {
            foreach (KeyValuePair<Vector3, Tower> entry in towerLookup)
            {
                if (entry.Value.selected && entry.Value.myOwner == attackingPlayer)
                    StartCoroutine(entry.Value.SpawnAttack(targetPosition));
            }
        }

        
		//Called from individual towers to notify all of the same player's towers to deselect a certain location
        [RPC]
		public void RPCDeselectTowers(bool isPlayer1)
		{
            print("deselecting towers");
            ownerShip player2Deselect = (isPlayer1 == true) ? ownerShip.Player1 : ownerShip.Player2;

			foreach (KeyValuePair<Vector3, Tower> entry in towerLookup)
			{
                if (entry.Value.selected && entry.Value.myOwner == player2Deselect)
                {
                    entry.Value.ToggleSelect();
                    entry.Value.updateSprite();
                }
			}
		}

        public void DeselectTowers(bool isPlayer1)
        {
            print("calling deselecttowers");
            if(Network.isServer)
                RPCDeselectTowers(isPlayer1);
                else
                networkView.RPC("RPCDeselectTowers", RPCMode.Server, Network.isServer);
        }



//--------------------------------------------------------------------------------------------------------------------


		void BuildTowerLocations()
		{
			GameObject[] towerMarkers = GameObject.FindGameObjectsWithTag("towerMarker");
            
           
            foreach (GameObject tm in towerMarkers)
			{
				switch (tm.name)
				{
				case "towerMarkerNeutral":
					mappedTowers.Add(tm.transform.position, ownerShip.Neutral);
					break;
				case "towerMarkerPlayer1":
					mappedTowers.Add(tm.transform.position, ownerShip.Player1);
					break;
				case "towerMarkerPlayer2":
					mappedTowers.Add(tm.transform.position, ownerShip.Player2);
					break;
				case "shockMarkerPlayer1":
					mappedShocks.Add(tm.transform.position, ownerShip.Player1);
					break;
				case "shockMarkerPlayer2":
					mappedShocks.Add(tm.transform.position, ownerShip.Player2);
					break;
				case "shockMarkerPlayerNeutral":
					mappedShocks.Add(tm.transform.position, ownerShip.Neutral);
					break;
				

				default:
					Debug.LogError("Invalid towerMarker type in buildTowerLocations");
					break;
					
				}

				Destroy(tm);
			}

            GameObject[] rootMarkers = GameObject.FindGameObjectsWithTag("RootMarker");
            foreach (GameObject rm in rootMarkers)
            {
                if (rm.name == "RootMarkerPlayer1")
                    mappedRoots.Add(rm.transform.position, ownerShip.Player1);
                else
                    mappedRoots.Add(rm.transform.position, ownerShip.Player2);
                Destroy(rm);
            }
			
		}
//---------------------------------------------------------------------------------------------------------------------------
       
        //Instanciates the towers in all the locations specified by BuildTowerLocations()
		public void SpawnTowers()
		{
            //GameObject[] rootMarkers = GameObject.FindGameObjectsWithTag("RootMarker");

            //foreach (GameObject root in rootMarkers) {
            //    GameObject rootNode = (GameObject)Network.Instantiate(rootPrefab,root.transform.position, Quaternion.Euler(0, 0, 0), 0);
            //    if(root.name == "RootMarkerPlayer1"){
            //        rootNode.GetComponent<Tower>().myOwner = ownerShip.Player1;
            //    }
            //    if(root.name == "RootMarkerPlayer2"){
            //        rootNode.GetComponent<Tower>().myOwner = ownerShip.Player2;
            //    }
            //    Destroy(root);
            //}
            foreach (KeyValuePair<Vector3, ownerShip> r in mappedRoots)
            {
                GameObject aRoot = (GameObject)Network.Instantiate(rootPrefab, r.Key, Quaternion.Euler(0, 0, 0), 0);
                Tower rScript = aRoot.GetComponent<Tower>();
                rScript.SwitchOwner(r.Value);
            }


			foreach (KeyValuePair<Vector3, ownerShip> entry in mappedTowers)
			{
				GameObject aTower = (GameObject)Network.Instantiate(towerPrefab, entry.Key, Quaternion.Euler(0, 0, 0), 0);
				Tower tScript = aTower.GetComponent<Tower>();
				tScript.SwitchOwner(entry.Value);
				switch (entry.Value)
				{
				case ownerShip.Neutral:
					tScript.units = NeutralStartingUnits;
					break;
				case ownerShip.Player1:
					tScript.units = Player1StartingUnits;
					break;
				case ownerShip.Player2:
					tScript.units = Player2StartingUnits;
					break;
				default:
					Debug.LogError("Invalid Ownership type");
					break;
				}
				towerLookup.Add(entry.Key, aTower.GetComponent<Tower>());
				
			}
			foreach (KeyValuePair<Vector3, ownerShip> entry in mappedShocks)
			{
				GameObject aTower = (GameObject)Network.Instantiate(shockPrefab, entry.Key, Quaternion.Euler(0, 0, 0), 0);
				Tower tScript = aTower.GetComponent<Tower>();
				tScript.SwitchOwner(entry.Value);
				switch (entry.Value)
				{
				case ownerShip.Neutral:
					tScript.units = NeutralStartingUnits;
					break;
				case ownerShip.Player1:
					tScript.units = Player1StartingUnits;
					break;
				case ownerShip.Player2:
					tScript.units = Player2StartingUnits;
					break;
				default:
					Debug.LogError("Invalid Ownership type");
					break;
				}
				towerLookup.Add(entry.Key, aTower.GetComponent<Tower>());
				
			}
			
		}
//--------------------------------------------------------------------------------------------------------------
        void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {
            if (stream.isWriting)
            {
                int p1score = (int)player1Score;
                stream.Serialize(ref p1score);
                int p2score = (int)player2Score;
                stream.Serialize(ref p2score);
            }
            else
            {
                int p1score = 0;
                stream.Serialize(ref p1score);
                player1Score = p1score;
                int p2score = 0;
                stream.Serialize(ref p2score);
                player2Score = p2score;
            }
        }
    }
}
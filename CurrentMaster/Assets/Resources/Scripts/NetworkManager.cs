using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Global{
    public class NetworkManager : MonoBehaviour
    {
        
        public bool debugWithoutClient = false; //When true game will start as soon as server starts otherwise wait for client
        private const string typeName = "KillAWatt";
        public string gameName = "Enter a Server Name";
        public HostData[] hostList;
        private GameManager gameManager;
        private StateManager stateManager;
        public void StartServer()
        {
            Network.InitializeServer(4, 5000, !Network.HavePublicAddress());
            MasterServer.RegisterHost(typeName, gameName, "Open");
        }

        /// <summary>
        /// On initialization
        /// </summary>
        void Awake()
        {
            stateManager = gameObject.GetComponent<StateManager>();
            gameManager = gameObject.GetComponent<GameManager>();
            Network.minimumAllocatableViewIDs = 300;
        }

        void OnServerInitialized()
        {      
            if (debugWithoutClient)
            {
                stateManager.status = WorldGameState.InGame;
                gameManager.SpawnTowers();
            }
        }
        
        void OnPlayerConnected()
        {
            if (!debugWithoutClient)
            {
                stateManager.status = WorldGameState.Purgatory;
            }
            MasterServer.RegisterHost(typeName, gameName, "Closed");
        }
        
        void Update()
        {   //Don't need to update hostlist if already connected
            if (Network.isServer || Network.isClient)
                return;
            if (stateManager.status == WorldGameState.StartMenu)
                RefreshHostList();
        }

        private void RefreshHostList()
        {
            MasterServer.RequestHostList(typeName);
        }

        void OnMasterServerEvent(MasterServerEvent msEvent)
        {
            List<HostData> hostBuffer = new List<HostData>();
            List<HostData> openHosts = new List<HostData>();
            if (msEvent == MasterServerEvent.HostListReceived)
                 hostBuffer = new List<HostData>(MasterServer.PollHostList());
            foreach (HostData hd in hostBuffer)
            {
                if (hd.comment == "Open")
                    openHosts.Add(hd);
            }
            hostList = openHosts.ToArray();
        }

        public void JoinServer(HostData hostData)
        {
            Network.Connect(hostData);
        }

        void OnConnectedToServer()
        {
            stateManager.status = WorldGameState.Purgatory;           
        }

        void OnDisconnectedFromServer(NetworkDisconnection msg) {
            if (Network.isServer) {
                Debug.Log("Local server connection disconnected");
            }
            else {
                if (msg == NetworkDisconnection.LostConnection)
                    Debug.Log("Lost connection to the server");
            else
                Debug.Log("Successfully diconnected from the server");
            }

            switch (stateManager.status) { 
                case WorldGameState.InGame:
                    stateManager.status = WorldGameState.StartMenu;
                    break;
                case WorldGameState.EndGame:
                    stateManager.status = WorldGameState.StartMenu;
                    break;
                case WorldGameState.Pause:
                    stateManager.status = WorldGameState.StartMenu;
                    break;
                default:
                    Debug.Log("Disconnected from invalid state");
                    break;
            }

            MasterServer.UnregisterHost();
            Application.LoadLevel (0);
        }
    }
}
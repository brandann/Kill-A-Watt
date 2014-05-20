using UnityEngine;
using System.Collections;



namespace Global{
    public class NetworkManager : MonoBehaviour
    {
        private const string typeName = "sometypeofGame";
        public string gameName = "Enter a Server Name";
        public HostData[] hostList;

        GameManager gameManager;

        public StateManager stateManager;

        public void StartServer()
        {
            Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
            MasterServer.RegisterHost(typeName, gameName);
        }
        /// <summary>
        /// On initialization
        /// </summary>
        void Awake()
        {
            stateManager = gameObject.GetComponent<StateManager>();
            gameManager = gameObject.GetComponent<GameManager>();
            
            Network.minimumAllocatableViewIDs = 250;
        }

        void OnServerInitialized()
        {
           
            gameManager.SpawnTowers();
        }

        void OnGUI()
        {           //todo figure out when refreshhost list should be called
            if (!Network.isClient && !Network.isServer)
                RefreshHostList();
        }



        private void RefreshHostList()
        {
            MasterServer.RequestHostList(typeName);
        }

        void OnMasterServerEvent(MasterServerEvent msEvent)
        {
            if (msEvent == MasterServerEvent.HostListReceived)
                hostList = MasterServer.PollHostList();
        }



        public void JoinServer(HostData hostData)
        {
            Network.Connect(hostData);
        }

        void OnConnectedToServer()
        {
            //todo should move this to onPlayerConnected
            stateManager.status = WorldGameState.InGame;
        }

        void OnPlayerConnected()
        {
            stateManager.status = WorldGameState.InGame;
            //todo add a pause before game actually starts
        }

    }
}
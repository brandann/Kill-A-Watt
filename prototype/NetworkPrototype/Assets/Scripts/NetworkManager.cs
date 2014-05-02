using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    private const string typeName = "NetworkPrototype";
    private const string gameName = "SomeRoom";
    private HostData[] hostList;
    public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void StartServer()
{
    Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
    MasterServer.RegisterHost(typeName, gameName);
}
    void OnServerInitialized()
    {
        SpawnPlayer();
    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(0, 0, 250, 100), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(275, 0, 250, 100), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(275, 100 + (110 * i), 250, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
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

    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    void OnConnectedToServer()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    }
}

 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SlaveNetworkConnection : MonoBehaviour {

    public bool isAtStartup = true;
    public NetworkClient myClient;

    void Update()
    {
        if (isAtStartup)
        {
            SetupClient();
        }
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect("192.168.0.26", 4444);
        isAtStartup = false;
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHandler : NetworkManager
{

    public bool isHost = false; //is this player hosting
    public Dictionary<int, NetworkConnection> players = new Dictionary<int, NetworkConnection>();
    private NetworkClient myClient;

    // Use this for initialization
    void Start()
    {
        if (isHost)
        {
            StartServer();
            NetworkServer.RegisterHandler(HelloWorld.messageID, OnHelloWorld);
        }
        else
            ConnectToServer();
    }

    private void OnHelloWorld(NetworkMessage netMsg)
    {
        //Do stuff with received info on Server
        HelloWorld t = netMsg.ReadMessage<HelloWorld>();
        Debug.Log(t.test);
    }

    //Setting up client to connect to server
    public void ConnectToServer()
    {
        Debug.LogError("Connecting to server...");
        myClient = new NetworkClient();

        myClient.Connect(this.networkAddress, this.networkPort);
        myClient.RegisterHandler(HelloWorld.messageID, OnHelloWorld);

        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(4f);

        HelloWorld t = new HelloWorld();
        t.test = "Client talks to host!";
        myClient.connection.Send(HelloWorld.messageID, t);
    }


    //Client connects to server
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.LogError("Player connected.");
        HelloWorld t = new HelloWorld();
        t.test = "Yay! Client has received message.";
        conn.Send(HelloWorld.messageID, t);
    }

    //Host succesfully start server
    public override void OnStartServer()
    {
        base.OnStartServer();

        Debug.LogError("Server initialized.");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

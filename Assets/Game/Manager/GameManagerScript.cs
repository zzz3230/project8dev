using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ENet;
using System.Text;
using System;
using OldNetwork;

public enum GameState
{
    Playing,
    Paused,
    MainMenu
}

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] public List<GameUnitInfo> gameUnitInfos = new List<GameUnitInfo> { };

    public GameState gameState { get; private set; }
    public Action<GameState> gameStateChanged = (x) => { };

    /// <summary>
    /// Change Playing and Paused state
    /// </summary>
    /// <param name="newGameState"></param>
    public void ChangeGameState(GameState newGameState)
    {
        var old = gameState;
        gameState = newGameState;

        if (newGameState != old)
            gameStateChanged?.Invoke(gameState);
    }


    private void Awake()
    {
        Global.gameManager = this;
        //print(Global.gameManager);
    }


    /*public BasePlayerScript playerScript;

    bool isServer;
    Host host;
    ENet.Event netEvent;
    bool isReady;

    List<Peer> peers = new List<Peer>{};
    Peer peer;
    public GameObject PREF;
    BasicBuildingGroup group;
    float CRoffset = 1;
    Int32 n1n = 0;
    BaseBuildingPrefabClass CR()
    {
        n1n++;
        CRoffset += 3.2f;
        var obj = Instantiate(PREF, new Vector3(CRoffset, 2, 20), Quaternion.identity);
        obj.name = n1n + "";
        return obj.GetComponent<BaseBuildingPrefabClass>();
        
    }
    private void OnGUI()
    {
        GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperRight });
        if (GUILayout.Button("Spawn"))
        {
            var gr = BasicBuildingGroup.Create();
            group = gr;
            
            gr.Add(CR(), null, default);
            gr.Add(CR(), gr.Debug_GetByIndex(0), default);
            gr.Add(CR(), gr.Debug_GetByIndex(1), default);
            gr.Add(CR(), gr.Debug_GetByIndex(2), default);
            gr.Add(CR(), gr.Debug_GetByIndex(3), default); 
            gr.Add(CR(), gr.Debug_GetByIndex(4), default);
            gr.Add(CR(), gr.Debug_GetByIndex(5), default);
        }
        if (GUILayout.Button("Remove")) 
        {
            var obj = group.Debug_GetByIndex(1);
            group.Remove(obj);
            Destroy(obj.gameObject);
            //group.Debug_PrintAllLinked(group.Debug_GetByIndex(1));
        }
        if (GUILayout.Button("Break"))
        {
            group.Break();
        }
        GUILayout.EndHorizontal();
        //if (GUILayout.Button("Export"))
        //    print(JsonUtility.ToJson(gameUnitInfos[0]));
    }

    private void OnGUI__() {
        if (GUILayout.Button("Server")){
            Invoke("InitServer", 0f);
        }
        if (GUILayout.Button("Client")){
            Invoke("InitClient", 0f);
        }
        if(GUILayout.Button("ToS")){
            Packet packet = default(Packet);
            byte[] data = Encoding.UTF8.GetBytes("FromClient!");

            packet.Create(data);
            //peer.Send(netEvent.ChannelID, ref packet);
            peer.Send(0, ref packet);

        }
        if(GUILayout.Button("ToC")){
            Packet packet = default(Packet);
            byte[] data = Encoding.UTF8.GetBytes("FromServer!");

            packet.Create(data);

            Utils.Log(peers[0].IP);
            host.Broadcast(netEvent.ChannelID, ref packet, new Peer[] {peers[0]});
        }
    }
    void InitServer(){
        host = new Host();
        Address address = new Address();
        address.Port = 8080;
        host.Create(address, 90);

        isReady = true;

        isServer = true;
    }
    void InitClient(){
        host = new Host();
        Address address = new Address();

        address.SetHost("127.0.0.1");
        address.Port = 8080;
        host.Create();

        peer = host.Connect(address);

        isReady = true;
        
        isServer = false;
    }
    [Serializable]
    class TEST{
        public string x;
        public int y;
    }
[Serializable]
    struct TESTS{
        public string x;
        public int y;
    }
    void Spawn()
    {
        
    }
    void Start()
    {

        Global.networkManager = new GameObject("networkManager", typeof(NetworkManager)).GetComponent<NetworkManager>();
        Global.networkService = new GameObject("networkService", typeof(NetworkService)).GetComponent<NetworkService>();
        Global.networkObjectsManager = new GameObject("networkObjectsManager", typeof(NetworkObjectsManager)).GetComponent<NetworkObjectsManager>();
        Global.networkClientsManager = new GameObject("networkClientsManager", typeof(NetworkClientsManager)).GetComponent<NetworkClientsManager>();
        Global.networkPrefabsManager = GetComponent<NetworkPrefabsManager>();

        DontDestroyOnLoad(Global.networkClientsManager);
        DontDestroyOnLoad(Global.networkObjectsManager);
        DontDestroyOnLoad(Global.networkManager);
        DontDestroyOnLoad(Global.networkService);

        Invoke(nameof(Spawn), 5f);


        var x = Resources.LoadAll("Assets/Game/Player");
        for (int i = 0; i < x.Length; i++)
        {
            print(x[i].name);
        }
        //ClientId.FromNickname("zzz3230");

        /*var x = new TEST();
        x.x = "Hello world! Hello wortld kgjpirn4538tif480rrj390yr04whprt9084h08ht084h0eh408tb0843e08gt44ew0e";
        x.y = 9998877;

        var a = new TESTS();
        a.x = "FromServer!";
        
        print("F O:" + Utils.ObjectToByteArray(x).Length);
        print("F S:" + Utils.ObjectToByteArray(a).Length);

        //print("S O:" + Utils.StructureToByteArray(x).Length);
        //print("S S:" + Utils.StructureToByteArray(a).Length);

        Screen.fullScreen = false;
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.SetResolution(640, 480, FullScreenMode.Windowed, 60);

        //Invoke("Init", 0f);
        *9900/
        

    }

    //111
    void any()
    {
        //var cb = Global.networkService.CallOnServer(this, nameof(S_call));
        //cb.Then(this, nameof(C_call));
    }
    void S_call(object dataToServer)
    {

    }
    void C_call(object dataFromServer)
    {

    }

    //222

    void any2()
    {
        //Global.networkService.CallOnServer(this, nameof(S_call2));
    }
    void S_call2(object dataToServer)
    {

        //Global.networkService.CallOnClient(this, default, nameof(C_call2), null);
    }

    void C_call2(object dataFromServer)
    {

    }



    void _Update(){

        bool polled = false;

        if(isReady)
        if(isServer){
            while (!polled) {
                if (host.CheckEvents(out netEvent) <= 0) {
                    if (host.Service(15, out netEvent) <= 0)
                        break;

                    polled = true;
                }

                switch (netEvent.Type) {
                    case ENet.EventType.None:
                        break;

                    case ENet.EventType.Connect:
                        Utils.Log("Client connected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        peers.Add(netEvent.Peer);
                        break;

                    case ENet.EventType.Disconnect:
                        Utils.Log("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        break;

                    case ENet.EventType.Timeout:
                        Utils.Log("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        break;

                    case ENet.EventType.Receive:
                        var byteArray = new byte[netEvent.Packet.Length];
                        netEvent.Packet.CopyTo(byteArray);
                        Utils.Log("Packet received from - ID: " + netEvent.Peer.ID + ", Data: " + Encoding.UTF8.GetString(byteArray));
                        netEvent.Packet.Dispose();
                        break;
                }
            }
            host.Flush();
        }
        else{
            while (!polled) {
                if (host.CheckEvents(out netEvent) <= 0) {
                    if (host.Service(15, out netEvent) <= 0)
                        break;

                    polled = true;
                }

                switch (netEvent.Type) {
                    case ENet.EventType.None:
                    Utils.Log("working!");
                        break;

                    case ENet.EventType.Connect:
                        Utils.Log("Connect");
                        break;

                    case ENet.EventType.Disconnect:
                        Utils.Log("Disconnect");
                        break;

                    case ENet.EventType.Timeout:
                        Utils.Log("Timeout");
                        break;

                    case ENet.EventType.Receive:
                        var byteArray = new byte[netEvent.Packet.Length];
                        netEvent.Packet.CopyTo(byteArray);
                        Utils.Log("from server - Channel ID: " + netEvent.ChannelID + ", Data: " + byteArray);
                        
                        
                        netEvent.Packet.Dispose();
                        break;
                }
            }
        }

        
    }*/


}
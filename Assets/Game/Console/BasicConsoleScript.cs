using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using System.Linq;
using IronPython;

public static class BasicConsole
{
    static Dictionary<string, Action> _attached—om = new Dictionary<string, Action>();

    public static bool ContainsCom(string name)
    {
        return _attached—om.ContainsKey(name);
    }
    public static void InvokeCom(string name)
    {
        if (ContainsCom(name))
            _attached—om[name].Invoke();
    }

    public static void AttachCom(string name, Action action)
    {
        if (!ContainsCom(name))
            _attached—om.Add(name, action);
    }
    public static string param = "";
}


public class BasicConsoleScript : MonoBehaviour
{
    public ItemInfo[] debugItems;

    public static BasicConsoleScript Instance;

    [SerializeField] public ItemInfo test_info;
    public NetworkObject playerOriginal;
    public Texture2D icoTest;
    private void Awake()
    {
        Global.gameSavingManager = new GameSavingManager("new_world");
        //print("created");
        Instance = this;
        //discarded

        /*var item = new ItemInstance();
        item.info = new ItemInfo
        {
            builderId = BuilderID.Def,
            discardedObject = playerOriginal.gameObject.GetPrefab(),
            handObject = null,
            ico = icoTest,
            stack = 32,
            id = SIID.game_testitem_1,
            name = new TEXT { _text = "hello" },
        };
        item.count = 1;
        item.metadata = new ItemMetadata { durability = 50.666f, uuid = "501ff541-b86b-4f82-b42f-7f901eea5dbb" };

        //System.IO.File.WriteAllText(@"D:\unity_project\Project8\test_save\item_ser.json", Newtonsoft.Json.JsonConvert.SerializeObject(item));
        BasicConsole.AttachCom("test", () => { print("test from basicConsole"); });


        var overworldSavingManager = new WorldSavingManager();
        overworldSavingManager.Init("overworld", 0);

        var items = new ItemInstance[1024];
        items = overworldSavingManager.LoadItemSector(0);

        var slot1 = ItemsManager.Allocate(1);
        var slot2 = ItemsManager.Allocate(1);

        slot1.MoveTo(slot2, 10);
        slot1.Swap(slot2);

        Global.gameSavingManager = new GameSavingManager("new_world");

        GameSavingManager.Instance.save.LoadItemSector(0);*/
        //for (int i = 32; i < 44; i++)
        //{
        //    items[i] = item;
        //}
        //overworldSavingManager.SaveItemSector(0, items);



        BasicConsole.AttachCom("ss", () =>
        {
            Global.networkManager.StartServer();
        });
        BasicConsole.AttachCom("sc", () =>
        {
            Global.networkManager.StartClient(BasicConsole.param);
            Global.networkManager.VerifyClient(
                new OldNetwork.NetStructs.ConnectionVerifyingData { nickname = "zzz3230" }
                );
            Debug.LogWarning(Global.networkManager.serverAddress.GetIP());
        });

        //GetComponent<Unity.Netcode.NetworkManager>().StartHost();


        BasicConsole.AttachCom("host", () =>
        {
            GetComponent<Unity.Netcode.NetworkManager>().StartHost();
        });
        BasicConsole.AttachCom("client", () =>
        {
            GetComponent<Unity.Netcode.NetworkManager>().StartClient(); 
            //GetComponent<Unity.Netcode.NetworkManager>().ConnectedClients[0].

        });


        //var x = new[] { 1, 2, 3 };
        //x = x.Select(x => x * 2).ToArray();
        //x = x.Where(x => x % 2 == 0).ToArray();

        //object x = 0;
        //x = "";
        //((dynamic)x).Split();

        BasicConsole.AttachCom("spawn", () =>
        {
            //var client = Global.networkClientsManager.GetClientByIndex(0);
            //var serverPlayer = Utils.Spawn(playerOriginal);
            //var clientPlayer = Utils.Spawn(playerOriginal, client);
            //Global.networkObjectsManager.MirrorToClient(serverPlayer, client);
            //Global.networkObjectsManager.MirrorToClient(clientPlayer, client);
            var player = Instantiate(playerOriginal);
            player.SpawnWithOwnership(1); 
            InitializeClientRPC(1, player.NetworkObjectId);
        });

        //Unity.Netcode.NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
    }

    [ClientRpc]
    private void InitializeClientRPC(ulong clientId, ulong objectId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            GameObject spawnedObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId].gameObject;
        }
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, Unity.Netcode.NetworkManager.ConnectionApprovedDelegate callback)
    {
        print("client: " + clientId);
        callback(false, 0, true, Vector3.zero, Quaternion.identity);
        
    }

    string param = "127.0.0.1";
    string input = "console";
    private void _OnGUI()
    {
        GUI.skin.textArea.fontSize = 30;

        GUILayout.BeginHorizontal();
        GUILayout.Label("PARAM");
        param = GUILayout.TextArea(param, GUILayout.MinWidth(500), GUILayout.Height(60));
        GUILayout.EndHorizontal();

        input = GUILayout.TextArea(input, GUILayout.MinWidth(500), GUILayout.Height(60));
        if (BasicConsole.ContainsCom(input))
        {
            BasicConsole.param = param;
            BasicConsole.InvokeCom(input);
            input = "";
        }
        Unity.Netcode.NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = param;
    }
}

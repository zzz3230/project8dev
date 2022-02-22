using UnityEngine;
using UnityEngine.SceneManagement;

namespace OldNetwork
{
    public class NetworkDebugHUD : MonoBehaviour
    {
        string ipBuffer = "127.0.0.1";
        string hostNameBuffer = "";
        private void OnGUI()
        {
            var nm = Global.networkManager;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Connecting ip: ");
            ipBuffer = GUILayout.TextField(ipBuffer);
            GUILayout.Label(":" + 49110);
            GUILayout.EndHorizontal();
            switch (nm.state)
            {
                case NetworkManager.NetworkState.NotReady:
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Server"))
                    {
                        Utils.Log(nm == null);
                        nm.StartServer(49110);
                    }
                    if (GUILayout.Button("Client"))
                    {
                        nm.StartClient(ipBuffer, 49110);
                    }
                    GUILayout.EndHorizontal();
                    break;

                case NetworkManager.NetworkState.Server:
                    GUILayout.Label("Server");
                    if (hostNameBuffer == "") hostNameBuffer = nm.serverAddress.GetHost();

                    GUILayout.Label("ServerIP: " + hostNameBuffer);
                    Ping();
                    SpawnPlayer();
                    break;

                case NetworkManager.NetworkState.Client:
                    GUILayout.Label("Client");
                    if (hostNameBuffer == "") hostNameBuffer = nm.serverAddress.GetHost();
                    GUILayout.Label("ServerIP: " + hostNameBuffer);
                    Ping();
                    //SpawnPlayer();
                    break;

            }

            ReloadScene();

            if (GUILayout.Button("Create"))
                if (Global.networkManager.isServer)
                    Global.networkObjectsManager.MirrorToClient(
                        Utils.Spawn(Global.networkPrefabsManager.GetPrefab(0)),
                        Global.networkClientsManager.GetClientByIndex(0)
                        );

        }

        void Ping()
        {
            if (GUILayout.Button("Ping!"))
                Global.networkManager.SendPing(Global.networkManager.isServer ? Global.networkManager.DebugGetPeer(0) : default);
        }
        public NetworkPrefab playerPrefOriginal;
        void SpawnPlayer()
        {
            if (GUILayout.Button("Spawn player server"))
            {
                var sp0 = Utils.Spawn(playerPrefOriginal);
                Global.networkObjectsManager.MirrorToAllClients(sp0);
            }
            if (GUILayout.Button("Spawn player client"))
            {

                var player = Global.networkClientsManager.GetClientByIndex(0);
                var sp1 = Utils.Spawn(playerPrefOriginal);
                sp1.owner = player;
                Global.networkObjectsManager.MirrorToAllClients(sp1);
            }
        }
        void ReloadScene()
        {
            if (GUILayout.Button("restart"))
            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }
    }
}
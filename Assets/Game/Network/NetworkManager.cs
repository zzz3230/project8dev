using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using ENet;
using System.Reflection;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;
namespace OldNetwork
{
    public class NetworkManager : MonoBehaviour
    {
        public enum NetworkState { NotReady, Server, Client }

        public NetworkState state { get; private set; }
        public bool isServer { get { return state == NetworkState.Server; } }

        Host _host;
        ENet.Event _netEvent;

        /// <summary>
        /// Pending for veryfying connections; On Client always empty
        /// </summary>
        List<Peer> _unverifiedPeers = new List<Peer> { };

        /// <summary>
        /// Basic verified connections
        /// </summary>
        List<Peer> _peers = new List<Peer> { };

        Dictionary<Peer, ClientId> _clientsInfo = new Dictionary<Peer, ClientId>();

        /// <summary>
        /// Client peer; On server always default
        /// </summary>
        Peer _peer;

        //bool _networkUpdating = false;
        public int tickRate { get; private set; } = 30;

        public Address serverAddress { get; private set; }

        Dictionary<ulong, Action<object>> _callbacks = new Dictionary<ulong, Action<object>> { };

        #region for debug
        public ClientId DebugGetClientId(int index) { return _clientsInfo[_peers[index]]; }
        public Peer DebugGetPeer(int index) { return _peers[index]; }
        #endregion for debug

        #region starting 
        public void StartServer(ushort port = 8080, int maxConnections = 90, int _tickRate = 30)
        {
            _host = new Host();
            Address address = new Address();
            address.Port = port;
            _host.Create(address, maxConnections);

            state = NetworkState.Server;

            serverAddress = address;

            tickRate = _tickRate;

            StartNetworkUpdate();
        }
        public void StartClient(string ip = "127.0.0.1", ushort port = 8080, int _tickRate = 30)
        {
            _host = new Host();
            Address address = new Address();

            address.SetHost(ip);
            address.Port = port;
            _host.Create();

            _peer = _host.Connect(address);

            state = NetworkState.Client;

            serverAddress = address;

            tickRate = _tickRate;

            StartNetworkUpdate();


        }

        void StartNetworkUpdate()
        {

            //Thread x = new Thread(WhileNetworkUpdate);
            //x.Start();
            //StartCoroutine(nameof(WhileNetworkUpdate));
            WhileNetworkUpdate().ConfigureAwait(false);
            //InvokeRepeating(nameof(NetworkUpdate), 0f, 1f/tickRate);
        }
        #endregion starting

        public Action onServerStarted;
        UnityEvent onServerConnect;

        #region netUpdate
        async Task WhileNetworkUpdate()
        {
            while (true)
            {
                NetworkUpdate();

                await Task.Delay((int)(1f / tickRate * 1000));
            }
        }

        void NetworkUpdate()
        {
            bool polled = false;

            if (state != NetworkState.NotReady)
                while (!polled)
                {
                    if (_host.CheckEvents(out _netEvent) <= 0)
                    {
                        if (_host.Service(0, out _netEvent) <= 0)
                            break;

                        polled = true;
                    }
                    if (state == NetworkState.Server)
                    {
                        //BEGIN SERVER CODE
                        switch (_netEvent.Type)
                        {
                            case ENet.EventType.None:
                                break;

                            case ENet.EventType.Connect:
                                ServerOnConnect(_netEvent);
                                break;

                            case ENet.EventType.Disconnect:
                                ServerOnDisconnect(_netEvent);
                                break;

                            case ENet.EventType.Timeout:
                                Utils.Log("Client timeout - ID: " + _netEvent.Peer.ID + ", IP: " + _netEvent.Peer.IP);
                                break;

                            case ENet.EventType.Receive:
                                ServerOnReceive(_netEvent);
                                break;
                        }
                        //END SERVER CODE
                    }
                    else
                    {
                        //BEGIN CLIENT CODE
                        switch (_netEvent.Type)
                        {
                            case ENet.EventType.None:
                                //Utils.Log("working!");
                                break;

                            case ENet.EventType.Connect:
                                ClientOnConnect(_netEvent);
                                break;

                            case ENet.EventType.Disconnect:
                                ClientOnDisconnect(_netEvent);
                                break;

                            case ENet.EventType.Timeout:
                                Utils.Log("Timeout");
                                break;

                            case ENet.EventType.Receive:
                                ClientOnReceive(_netEvent);
                                break;
                        }
                        //END CLIENT CODE
                    }
                }
        }

        void ServerOnConnect(ENet.Event netEvent)
        {
            Utils.Log("Client connected - ID: " + _netEvent.Peer.ID + ", IP: " + _netEvent.Peer.IP);
            _unverifiedPeers.Add(_netEvent.Peer);

        }
        void ServerOnDisconnect(ENet.Event netEvent)
        {
            Utils.Log("Client disconnected - ID: " + _netEvent.Peer.ID + ", IP: " + _netEvent.Peer.IP);
        }
        void ServerOnReceive(ENet.Event netEvent)
        {
            var byteArray = new byte[_netEvent.Packet.Length];
            _netEvent.Packet.CopyTo(byteArray);
            Utils.Log("Packet received from - ID: " + _netEvent.Peer.ID);
            Utils.Log("> DataLen: " + (byteArray).Length);

            CheckReceivedData(byteArray, netEvent);

            _netEvent.Packet.Dispose();
        }
        void ClientOnConnect(ENet.Event netEvent)
        {
            Utils.Log("Im connected to " + serverAddress.GetIP());

            NetStructs.ConnectionVerifyingData verifyingData =
                new NetStructs.ConnectionVerifyingData
                {
                    nickname = "zzz3230"
                };

            ClientSendObject(verifyingData);
        }
        void ClientOnDisconnect(ENet.Event netEvent)
        {
            Utils.Log("Im disconnected from " + serverAddress.GetIP());
            state = NetworkState.NotReady;
        }
        void ClientOnReceive(ENet.Event netEvent)
        {
            var byteArray = new byte[_netEvent.Packet.Length];
            _netEvent.Packet.CopyTo(byteArray);
            Utils.Log("from server - Channel ID: " + _netEvent.ChannelID);
            Utils.Log("> DataLen: " + byteArray.Length);

            CheckReceivedData(byteArray, netEvent);
            _netEvent.Packet.Dispose();
        }
        #endregion netUpdate

        void CheckReceivedData(byte[] data, ENet.Event netEvent)
        {
            if (data == null)
                return;



            object obj = Utils.ByteArrayToObject(data);

            var objType = obj.GetType();

            //Utils.Log("Data name:" + objType.Name);
            //Utils.Log("Struct name:" + nameof(NetStructs.Ping));

            switch (objType.Name)
            {
                case nameof(NetStructs.Ping):
                    ReceivePing((NetStructs.Ping)obj, netEvent);
                    break;
                case nameof(NetStructs.ConnectionVerifyingData):
                    VerifyConnection((NetStructs.ConnectionVerifyingData)obj, netEvent);
                    break;
                case nameof(NetStructs.ConnectionResponseData):
                    ApplyConnectionResponse((NetStructs.ConnectionResponseData)obj);
                    break;
                case nameof(NetStructs.RequestCallMethodData):
                    ApplyRequestCallMethod((NetStructs.RequestCallMethodData)obj, netEvent);
                    break;
                case nameof(NetStructs.RequestCallbackData):
                    ApplyRequestCallback((NetStructs.RequestCallbackData)obj);
                    break;
                case nameof(NetStructs.RequestMirrorObjectData):
                    MirrorObject((NetStructs.RequestMirrorObjectData)obj);
                    break;
            }
        }

        public void VerifyClient(NetStructs.ConnectionVerifyingData data)
        {
            ClientSendObject(data);
        }

        void VerifyConnection(NetStructs.ConnectionVerifyingData data, ENet.Event netEvent)
        {
            if (!isServer)
                return;

            var peer = netEvent.Peer;
            if (!_unverifiedPeers.Contains(peer))
                return;
            Utils.Log("Connecting client with nickname", 20);
            Utils.Log(">" + data.nickname, 20);


            if (data.nickname.Length < 100)
            {
                //IF OK
                _peers.Add(peer);
                //_clientsInfo.Add(peer, new ClientId
                //{
                //    clientInfo = new ClientInfo
                //    { ip = _peer.IP, nickname = data.nickname, peerId = _peers.IndexOf(peer) }
                //});
                FinishConnection(peer, data);
            }
            else
            {
                //IF BAD
                peer.DisconnectNow(0);
            }
            _unverifiedPeers.Remove(peer);
        }
        void FinishConnection(Peer peer, NetStructs.ConnectionVerifyingData data)
        {
            NetStructs.ConnectionResponseData responseData =
                new NetStructs.ConnectionResponseData
                {
                    connected = true
                };
            AddNewClient(peer, data);
            ServerSendObject(responseData, new Peer[] { peer });
        }
        void AddNewClient(Peer peer, NetStructs.ConnectionVerifyingData data)
        {
            var clientId = new ClientId();
            clientId.clientInfo = new ClientInfo
            {
                nickname = data.nickname,
                ip = peer.IP,
                peerId = _peers.Count - 1
            };
            Global.networkClientsManager.AddClient(peer, clientId);
        }
        void ApplyConnectionResponse(NetStructs.ConnectionResponseData responseData)
        {
            if (isServer)
                return;


            Utils.Log(responseData.connected ? "Connected!" : "Not connected!", 20f);
        }
        void ApplyRequestCallMethod(NetStructs.RequestCallMethodData data, ENet.Event netEvent)
        {
            var obj = Global.networkObjectsManager.GetNetworkMonoBehaviourByIndex(data.objectHash);
            if (obj != null)
            {
                //var res = obj.GetType().GetMethod(data.methodName).Invoke(obj, data.args);
                var t = obj.GetType();
                var m = t.GetMethod(data.methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                var res = m.Invoke(obj, data.args);


                if (data.callbackIndex != 0)
                {
                    NetStructs.RequestCallbackData callbackData = new NetStructs.RequestCallbackData();
                    callbackData.index = data.callbackIndex;
                    callbackData.res = res;

                    if (isServer)
                        ServerSendObject(callbackData, new Peer[] { netEvent.Peer });
                    else
                        ClientSendObject(callbackData);
                }
            }
        }
        void ApplyRequestCallback(NetStructs.RequestCallbackData data)
        {
            if (_callbacks.TryGetValue(data.index, out var val))
            {
                val.Invoke(data.res);
                _callbacks.Remove(data.index);
            }
        }

        public void AddCallbackAction(ulong index, Action<object> action)
        {
            _callbacks.Add(index, action);
        }

        public ulong SendRequestCallMethodToServer(NetStructs.RequestCallMethodData data, bool makeCallbackIndex)
        {
            if (!isServer)
            {
                ulong cbi = 0;
                if (makeCallbackIndex)
                {
                    cbi = Utils.RandomUlong();
                    data.callbackIndex = cbi;
                }
                ClientSendObject(data);
                return cbi;
            }
            return 0;
        }
        public ulong SendRequestCallMethodToOwningClient(NetStructs.RequestCallMethodData data, bool makeCallbackIndex)
        {
            if (isServer)
            {
                var obj = Global.networkObjectsManager.GetNetworkMonoBehaviourByIndex(data.objectHash);
                if (obj.owner != null)
                {
                    ulong cbi = 0;
                    if (makeCallbackIndex)
                    {
                        cbi = Utils.RandomUlong();
                        data.callbackIndex = cbi;
                    }

                    ServerSendObject(data, new Peer[]
                    {
                    Global.networkClientsManager.GetPeerByClient(obj.owner)
                    });
                    return cbi;
                }


            }
            return 0;
        }
        public ulong SendRequestCallMethodToClient(NetStructs.RequestCallMethodData data, ClientId client, bool makeCallbackIndex)
        {
            if (isServer)
            {
                ulong cbi = 0;
                if (makeCallbackIndex)
                {
                    cbi = Utils.RandomUlong();
                    data.callbackIndex = cbi;
                }

                ServerSendObject(data, new Peer[]
                    {
                    Global.networkClientsManager.GetPeerByClient(client)
                    });
                return cbi;
            }
            return 0;
        }

        public void SendMirrorObject(NetStructs.RequestMirrorObjectData data, ClientId client)
        {
            ServerSendObject(data, new Peer[] { _peers[client.clientInfo.peerId] });
        }
        void MirrorObject(NetStructs.RequestMirrorObjectData data)
        {
            Debug.Log(data.prefabId);
            if (!isServer)
            {
                Instantiate(Global.networkPrefabsManager.GetPrefab(data.prefabId).gameObject).
                    GetComponent<NetworkMonoBehaviour>().InitOnClient(data.netHash);
            }
        }

        #region ping
        /// <summary>
        /// if peer default Sending to server
        /// </summary>
        /// <param name="peer"></param>
        public void SendPing(Peer peer = default)
        {
            NetStructs.Ping ping = new NetStructs.Ping();
            ping.sendTime = System.DateTime.Now.Ticks;

            if (isServer)
            {
                ServerSendObject(ping, new Peer[] { peer });
            }
            else
            {
                ClientSendObject(ping);
            }
        }
        void ReceivePing(NetStructs.Ping ping, ENet.Event netEvent)
        {
            if (ping.isBack)
            {
                Utils.Log(
                    "Pong from " +
                    (isServer ? "Client " + netEvent.Peer.IP : "Server") +
                    ":" + (new System.TimeSpan(System.DateTime.Now.Ticks - ping.sendTime)).TotalMilliseconds
                    );
            }
            else
            {
                NetStructs.Ping pong = new NetStructs.Ping();
                pong.isBack = true;
                pong.sendTime = ping.sendTime;

                if (isServer)
                {
                    ServerSendObject(pong, new Peer[] { netEvent.Peer });
                }
                else
                {
                    ClientSendObject(pong);
                }
            }
        }
        #endregion ping

        #region sending
        void ClientSendObject(object data, byte channelID = 0)
        {
            ClientSendBytes(Utils.ObjectToByteArray(data), channelID);
        }
        void ServerSendObject(object data, Peer[] peers, byte channelID = 0)
        {
            ServerSendBytes(Utils.ObjectToByteArray(data), peers, channelID);
        }
        void ServerSendObjectAll(object data, byte channelID = 0)
        {
            ServerSendBytesAll(Utils.ObjectToByteArray(data), channelID);
        }

        void ClientSendBytes(byte[] data, byte channelID = 0)
        {
            Packet packet = default(Packet);
            packet.Create(data);
            _peer.Send(channelID, ref packet);
        }
        void ServerSendBytes(byte[] data, Peer[] peers, byte channelID = 0)
        {
            Packet packet = default(Packet);
            packet.Create(data);
            _host.Broadcast(channelID, ref packet, peers);
        }
        void ServerSendBytesAll(byte[] data, byte channelID = 0)
        {
            Packet packet = default(Packet);
            packet.Create(data);
            _host.Broadcast(channelID, ref packet);
        }
        #endregion sending
    }

    public static class NetStructs
    {
        [System.Serializable]
        public struct Ping { public bool isBack; public long sendTime; }

        [System.Serializable]
        public struct ConnectionVerifyingData
        {
            public string nickname;
        }
        [System.Serializable]
        public struct ConnectionResponseData
        {
            public bool connected;
        }
        [System.Serializable]
        public struct RequestCallMethodData
        {
            public ulong objectHash;
            public string methodName;
            public object[] args;
            public bool callBack;
            public ulong callbackIndex;
        }
        [Serializable]
        public struct RequestCallbackData
        {
            public ulong index;
            public object res;
        }
        [Serializable]
        public struct RequestMirrorObjectData
        {
            public int prefabId;
            public ulong netHash;
        }
    }
}
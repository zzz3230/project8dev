using UnityEngine;


namespace OldNetwork
{

    [System.Serializable]
    public struct ClientInfo
    {
        public string nickname;
        public string ip;
        public int peerId;
    }

    [System.Serializable]
    public class ClientId
    {
        public ClientInfo clientInfo;// { get; set; }

        public static ClientId FromPeer(ENet.Peer peer)
        {
            return null;//new ClientId { clientInfo = new ClientInfo { nickname} };
        }

        public static ClientId FromNickname(string nn)
        {
            Utils.ThrowErrorIf(NetworkManager.NetworkState.Client);

            Utils.ThrowDoIt("ClientId");
            return default;
        }
        public static ClientId FromIndex(int index)
        {
            Utils.ThrowErrorIf(NetworkManager.NetworkState.Client);

            Utils.ThrowDoIt("ClientId");
            return default;
        }
    }
}
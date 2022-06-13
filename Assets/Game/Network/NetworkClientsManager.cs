using ENet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace OldNetwork
{
    public class NetworkClientsManager : MonoBehaviour
    {
        Dictionary<ENet.Peer, ClientId> _clients = new Dictionary<Peer, ClientId> { };

        public void AddClient(ENet.Peer peer, ClientId clientId)
        {
            _clients.Add(peer, clientId);
        }

        public ClientId GetClientByPeer(ENet.Peer peer)
        {
            if (_clients.TryGetValue(peer, out var val))
                return val;
            return null;
        }

        public int GetClientsCount()
        {
            return _clients.Count;
        }

        public ClientId GetClientByIndex(int index)
        {
            return _clients.Values.ToList()[index];
        }

        public Peer GetPeerByClient(ClientId clientId)
        {
            if (_clients.ContainsValue(clientId))
            {
                var val = _clients.FirstOrDefault(x => x.Value == clientId).Key;
                return val;
            }
            throw new System.Exception("client peer not found");
        }
    }
}
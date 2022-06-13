using System.Collections.Generic;
using System.Linq;
//using UnityEditor.PackageManager;
using UnityEngine;
namespace OldNetwork
{
    public class NetworkObjectsManager : MonoBehaviour
    {
        Dictionary<ulong, NetworkMonoBehaviour> _objects =
            new Dictionary<ulong, NetworkMonoBehaviour> { };

        public NetworkMonoBehaviour GetNetworkMonoBehaviourByIndex(ulong index)
        {
            if (_objects.TryGetValue(index, out var val))
                return val;
            return null;
        }

        public ulong AddNetworkMonoBehaviour(NetworkMonoBehaviour obj, ulong index)
        {
            _objects.Add(index, obj);
            return index;
        }

        public ulong InitNetworkMonoBehaviour(NetworkMonoBehaviour obj)
        {
            var newIndex = Utils.NextUlong();
            _objects.Add(newIndex, obj);

            return newIndex;
        }

        public void SetOwner(NetworkMonoBehaviour obj, ClientId owner)
        {
            obj.owner = owner;
        }

        public void MirrorToClient(NetworkMonoBehaviour obj, ClientId client)
        {
            NetStructs.RequestMirrorObjectData data = new NetStructs.RequestMirrorObjectData();
            data.netHash = obj.objectHash;
            data.prefabId = obj.prefab.index;
            Global.networkManager.SendMirrorObject(data, client);

        }

        public void MirrorToAllClients(NetworkMonoBehaviour obj)
        {
            NetStructs.RequestMirrorObjectData data = new NetStructs.RequestMirrorObjectData();
            data.netHash = obj.objectHash;
            data.prefabId = obj.prefab.index;

            for (int i = 0; i < Global.networkClientsManager.GetClientsCount(); i++)
            {
                Global.networkManager.SendMirrorObject(data, Global.networkClientsManager.GetClientByIndex(i));
            }

        }

        public void RemoveNetworkMonoBehaviour(NetworkMonoBehaviour obj)
        {
            if (_objects.ContainsValue(obj))
                _objects.Remove(_objects.FirstOrDefault(x => x.Value == obj).Key);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OldNetwork
{
    public class NetworkPrefabsManager : MonoBehaviour
    {
        [SerializeField] public List<NetworkPrefab> prefabs;

        public NetworkPrefab GetPrefab(int index)
        {
            prefabs[index].index = index;
            return prefabs[index];
        }
        public int GetPrefabIndex(NetworkPrefab prefab)
        {
            return prefabs.IndexOf(prefab);
        }
    }
}
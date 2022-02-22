using System;
using UnityEngine;
namespace OldNetwork
{
    public class NetworkService : MonoBehaviour
    {
        public class NetworkCallback
        {
            public ulong index { get; private set; }
            public NetworkCallback(ulong _index)
            {
                index = _index;
            }

            public object Data;
            public T GetData<T>()
            {
                return (T)Data;
            }
            public void Then(Action action)
            {

            }
            public void Then(Action<object> action)
            {
                Global.networkManager.AddCallbackAction(index, action);
            }
        }
        NetStructs.RequestCallMethodData MakeRequestCallMethodData(NetworkMonoBehaviour sender, string name, params object[] args)
        {
            return new NetStructs.RequestCallMethodData
            {
                args = args,
                methodName = name,
                objectHash = sender.objectHash
            };
        }

        public NetworkCallback CallOnServer(NetworkMonoBehaviour sender, string name, bool callback = true, params object[] args)
        {
            Utils.ThrowErrorIf(NetworkManager.NetworkState.Server);

            var index = Global.networkManager.SendRequestCallMethodToServer(MakeRequestCallMethodData(sender, name, args), callback);

            return new NetworkCallback(index);
        }
        /// <summary>
        /// if client = null calling on owning client
        /// </summary>
        public NetworkCallback CallOnClient(NetworkMonoBehaviour sender, ClientId client, string name, bool callback = true, params object[] args)
        {
            var data = MakeRequestCallMethodData(sender, name, args);
            ulong index = 0;
            if (client == null)
            {
                index = Global.networkManager.SendRequestCallMethodToOwningClient(data, callback);
            }
            else
            {
                index = Global.networkManager.SendRequestCallMethodToClient(data, client, callback);
            }
            return new NetworkCallback(index);
        }
        public NetworkCallback CallMulticast(NetworkMonoBehaviour sender, string name, bool callback = true, params object[] args)
        {
            Utils.ThrowErrorIf(NetworkManager.NetworkState.Client);
            return default;
        }
    }
}
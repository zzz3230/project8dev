using System;
using System.Collections.Generic;
using UnityEngine;
namespace OldNetwork
{
    public class NetworkMonoBehaviour : MonoBehaviour
    {
        [Flags]
        [Obsolete]
        public enum SyncArticle
        {
            Position = 1,
            Rotation = 2,
        }

        public List<BaseSyncArticle> syncArticles = new List<BaseSyncArticle> { };// { get; set; }
        public NetworkPrefab prefab { get; private set; }
        public ulong objectHash { get; private set; }
        public ClientId owner;//{ get; set; }

        public bool isOwner { get { return _isOwner; } }
        [SerializeField] bool _isOwner;
        public bool isServer { get; private set; }

        public virtual void GameStart() { }
        public virtual void GameUpdate() { }
        public virtual void GameFixedUpdate() { }

        private void Start()
        {
            print(owner);
            _isOwner = (isServer && owner == null) | (!isServer && owner != null);
            GameStart();
        }
        private void Update()
        {
            GameUpdate();
        }
        private void FixedUpdate()
        {
            if (isServer)
                SyncArticles(syncArticles);

            GameFixedUpdate();
        }

        #region article system
        /*
        Dictionary<SyncArticle, object> lastSynced = new Dictionary<SyncArticle, object> { };
        T LastSyncedArticleGet<T>(SyncArticle article)
        {
            if (lastSynced.ContainsKey(article))
                return (T)lastSynced[article];
            else
                return default;
        }
        void LastSyncArticleSet(SyncArticle article, object value)
        {
            if(lastSynced.ContainsKey(article))
                lastSynced[article] = value;
            else
                lastSynced.Add(article, value);
    }
        void UpdateArticle<T>(SyncArticle articles, SyncArticle art, T val)
    {
            if (!LastSyncedArticleGet<T>(art).Equals(val))
            {
                if (articles.HasFlag(SyncArticle.ToOwner))
                    CallOnOwningClient(nameof(C_ApplyArticle), false, art, val);
                else if (articles.HasFlag(SyncArticle.ToAll))
                    CallOnAllClients(nameof(C_ApplyArticle), false, art, val);
                LastSyncArticleSet(art, val);
            }
        }*/
        void SyncArticles(List<BaseSyncArticle> articles)
        {
            for (int i = 0; i < articles.Count; i++)
            {
                if (articles[i].S_ValueUpdated())
                {
                    if (articles[i].syncType.HasFlag(BaseSyncArticle.SyncType.ToAll))
                        CallOnAllClients(nameof(C_ApplyArticle), false, i, articles[i].S_GetValue());
                    else if (articles[i].syncType.HasFlag(BaseSyncArticle.SyncType.ToOwner))
                        CallOnOwningClient(nameof(C_ApplyArticle), false, i, articles[i].S_GetValue());
                    else
                        Debug.LogWarning("sync type not define");
                }
            }
        }

        void C_ApplyArticle(int articleIndex, object value)
        {
            syncArticles[articleIndex].C_SetValue(value);
        }
        #endregion

        private void Init()
        {
            isServer = Global.networkManager.isServer;
            //isOwner = 
            prefab = GetComponent<NetworkPrefab>();
        }
        public void InitOnClient(ulong hash)
        {
            Init();
            if (!isServer)
                objectHash = Global.networkObjectsManager.AddNetworkMonoBehaviour(this, hash);
        }
        public void InitOnServer()
        {
            Init();
            if (isServer)
                objectHash = Global.networkObjectsManager.InitNetworkMonoBehaviour(this);
        }

        //protected void OnStart() { }
        //public void Start()
        //{

        //}

        void OnDestroy()
        {
            Global.networkObjectsManager.RemoveNetworkMonoBehaviour(this);
        }

        public NetworkService.NetworkCallback CallOnServer(string name, bool callback = true, params object[] args)
        {
            return Global.networkService.CallOnServer(this, name, callback, args);
        }
        public NetworkService.NetworkCallback CallOnOwningClient(string name, bool callback = true, params object[] args)
        {
            return Global.networkService.CallOnClient(this, owner, name, callback, args);
        }
        public NetworkService.NetworkCallback CallOnAllClients(string name, bool callback = true, params object[] args)
        {
            return Global.networkService.CallMulticast(this, name, callback, args);
        }
    }
}
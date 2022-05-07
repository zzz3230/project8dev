using UnityEngine;
using OldNetwork;

public static class Global {

    public static GameManagerScript gameManager
    {
        get { return _gameManager; }
        set { if (_gameManager == null) _gameManager = value; }
    }
    static GameManagerScript _gameManager;

    public static GameSavingManager gameSavingManager
    {
        get { return _gameSavingManager; }
        set { if (_gameSavingManager == null) _gameSavingManager = value; }
    }
    static GameSavingManager _gameSavingManager;


    public static GameResourcesManager resourcesManager
    {
        get { return _resourcesManager; }
        set { if (_resourcesManager == null) _resourcesManager = value; }
    }
    static GameResourcesManager _resourcesManager;

    public static PrefabManager prefabManager
    {
        get { return _prefabManager; }
        set { if (_prefabManager == null) _prefabManager = value; }
    }
    static PrefabManager _prefabManager;


    public static NetworkManager networkManager {
        get {return _networkManager;}
        set {if(_networkManager == null) _networkManager = value;} 
        }
    static NetworkManager _networkManager;


    public static NetworkService networkService {
        get {return _networkService;}
        set {if(_networkService == null) _networkService = value;} 
        }
    static NetworkService _networkService;

    public static NetworkObjectsManager networkObjectsManager
    {
        get { return _networkObjectsManager; }
        set { if (_networkObjectsManager == null) _networkObjectsManager = value; }
    }
    static NetworkObjectsManager _networkObjectsManager;

    public static NetworkClientsManager networkClientsManager
    {
        get { return _networkClientsManager; }
        set { if (_networkClientsManager == null) _networkClientsManager = value; }
    }
    public static NetworkClientsManager _networkClientsManager;

    public static NetworkPrefabsManager networkPrefabsManager
    {
        get { return _networkPrefabsManager; }
        set { if (_networkPrefabsManager == null) _networkPrefabsManager = value; }
    }
    public static NetworkPrefabsManager _networkPrefabsManager;
}
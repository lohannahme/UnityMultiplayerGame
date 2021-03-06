using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.IO;

public class NetworkManagerLobby : NetworkManager
{
    private int minPlayers = 1;

    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")] [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

    [Header("Game")] 
    [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;
    


    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    

    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();

    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().name != Path.GetFileNameWithoutExtension(menuScene))
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        
        if (SceneManager.GetActiveScene().name == Path.GetFileNameWithoutExtension(menuScene))
        {
            bool isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if(conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

  

    public override void OnStopServer()
    {
       

        RoomPlayers.Clear();
        GamePlayers.Clear();
    }
    
    public void NotifyPlayersOfReadyState()
    {
        foreach(var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if(numPlayers < minPlayers) { return false; }

        foreach(var player in RoomPlayers)
        {
            if(!player.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if(SceneManager.GetActiveScene().name == Path.GetFileNameWithoutExtension(menuScene))
        {
            if (!IsReadyToStart()) { return; }

            ServerChangeScene("Scene_Map_01");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        //menu to game

        if(SceneManager.GetActiveScene().name == Path.GetFileNameWithoutExtension(menuScene) && newSceneName.StartsWith("Scene_Map"))
        {
            for(int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Scene_Map"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);

        }
    }
}

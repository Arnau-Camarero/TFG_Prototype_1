using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Spawner : NetworkBehaviour
{
    // Prefab to spawn
    public GameObject objectToSpawn;

    // Spawn interval in seconds
    public float spawnInterval = 0.75f;
    public int maxEnemies = 15;
    public List<GameObject> enemyCounter;

    // Minimum and maximum values for random z-coordinate offset
    public float minZOffset = -5.0f;
    public float maxZOffset = 5.0f;

    private NetworkVariable<int> connectedPlayers = new NetworkVariable<int>(0);
    private int requiredPlayers = 2;

    public override void OnNetworkSpawn(){
        if(!IsOwner){
            enabled=false;
            return;
        }
        if(IsServer){
            connectedPlayers.OnValueChanged += OnConnectedPlayersChanged;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

    }

    private void OnClientConnected(ulong clientId)
    {
        connectedPlayers.Value++;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        connectedPlayers.Value--;
    }

    private void OnConnectedPlayersChanged(int oldValue, int newValue){
        if(newValue >= requiredPlayers){
            invokeEnemies();
        }
    }

    void SpawnEnemy()
    {
        if(enemyCounter.Count <= maxEnemies){
            Vector3 spawnPosition = transform.position;
            float randomZOffset = Random.Range(minZOffset, maxZOffset);
            spawnPosition.z += randomZOffset;

            GameObject enemy = Instantiate(objectToSpawn, spawnPosition, transform.rotation, transform);
            enemy.GetComponent<EnemyBehaviour>().spawner = this;
            enemy.GetComponent<NetworkObject>().Spawn(true);

            enemyCounter.Add(enemy);
        }
    }

    public void invokeEnemies(){
        if (IsServer)
        {
            InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
            InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
        }
    }

    public override void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }

        if (IsServer)
        {
            connectedPlayers.OnValueChanged -= OnConnectedPlayersChanged;
        }
    }
}

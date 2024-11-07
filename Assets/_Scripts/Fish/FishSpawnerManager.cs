using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{
    private float Height => Camera.main.orthographicSize;
    private float Width => 16f / 9f * Height;

    [SerializeField] private Fish commonFish;
    [SerializeField] private Fish rareFish;
    [SerializeField] private Fish epicFish;
    
    
    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += StartSpawnFish;
    }

    private void StartSpawnFish()
    {
        NetworkManager.Singleton.OnServerStarted -= StartSpawnFish;
        StartCoroutine(SpawnFishOverTime());
    }
    
    private IEnumerator SpawnFishOverTime()
    {
        yield return new WaitForSeconds(3f);
        
        while (NetworkManager.Singleton.ConnectedClients.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
            var fishGo = Instantiate(RandomFish(), FishSpawnPoint(), Quaternion.identity);
            NetworkObject networkObject = fishGo.GetComponent<NetworkObject>();
            networkObject.Spawn(true);
        }
    }

    private Fish RandomFish()
    {
        var r = Random.Range(0, 100);
        return r switch
        {
            < 10 => epicFish,
            >= 10 and < 40 => rareFish,
            >= 40 => commonFish
        };
    }
    
    private Vector3 FishSpawnPoint()
    {
        var spawnSide = Random.Range(0, 4);

        float x = 0;
        float y = 0;

        switch (spawnSide)
        {
            case 0:
                x = Random.Range(-(Width + 0.5f), (Width + 0.5f));
                y = Height + 0.5f;
                break;
            case 1:
                x = Width + 0.5f;
                y = Random.Range(-(Height + 0.5f), (Height + 0.5f));
                break;
            case 2:
                x = Random.Range(-(Width + 0.5f), (Width + 0.5f));
                y = -Height - 0.5f;
                break;
            case 3:
                x = -Width - 0.5f;
                y = Random.Range(-(Height + 0.5f), (Height + 0.5f));
                break;
        }

        return new Vector3(x, y, 0f);
    }
    
}

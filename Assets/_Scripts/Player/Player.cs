using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    public static Action<Fish.FishStats> AddKilledFishEvent = null;
    
    public override void OnNetworkSpawn()
    {
        if(!IsOwner) return;
        
        if (OwnerClientId % 4 == 0)
        {
            transform.position = new Vector3(-3f, -5f, 0f);
        }
        if (OwnerClientId % 4 == 1)
        {
            transform.position = new Vector3(3f, -5f, 0f);
        }
        if (OwnerClientId % 4 == 2)
        {
            transform.position = new Vector3(-3f, 5f, 0f);
        }
        if (OwnerClientId % 4 == 3)
        {
            transform.position = new Vector3(3f, 5f, 0f);
        }
        
    }
    
    public void AddStats(Fish.FishStats stats)
    {
        AddKilledFishEvent?.Invoke(stats);
    }
}

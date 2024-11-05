using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
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
}

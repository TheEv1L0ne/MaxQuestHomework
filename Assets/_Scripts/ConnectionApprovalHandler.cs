using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionApprovalHandler : MonoBehaviour
{
    private const int MaxPlayer = 4;

    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest arg1, NetworkManager.ConnectionApprovalResponse arg2)
    {
        arg2.Approved = true;
        arg2.CreatePlayerObject = true;
        arg2.PlayerPrefabHash = null;
        if (NetworkManager.Singleton.ConnectedClients.Count >= MaxPlayer)
        {
            arg2.Approved = false;
            arg2.Reason = "Server Full";
        }

        arg2.Pending = false;
    }
}

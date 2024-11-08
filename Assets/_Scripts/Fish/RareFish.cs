using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareFish : Fish
{
    private float _timeSinceLastChange = float.MinValue;
    private const float TimeBeforeJump = 1f;
    private const float JumpDistance = 2f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _timeSinceLastChange = Time.time;
    }

    protected override void ChangeFishDirection()
    {
        if (!FirstTimeEntered) return;

        if (_timeSinceLastChange + TimeBeforeJump >= Time.time) return;
        
        _timeSinceLastChange = Time.time;
        transform.position += MoveVector * JumpDistance;
    }
}

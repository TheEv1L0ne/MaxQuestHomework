using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicFish : Fish
{
    private float _timeSinceLastChange = float.MinValue;
    private const float TimeBeforeStop = 1f;
    private const float StopTime = 1f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _timeSinceLastChange = Time.time;
    }

    protected override void ChangeFishDirection()
    {
        if (!FirstTimeEntered) return;

        if (_timeSinceLastChange + TimeBeforeStop >= Time.time) return;
        
        MoveVector = Vector3.zero;

        if (_timeSinceLastChange + TimeBeforeStop + StopTime >= Time.time) return;
        
        _timeSinceLastChange = Time.time;
        MoveVector = transform.up.normalized;
        RandomizeDirection();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFish : Fish
{
    private float _timeSinceLastChange = float.MinValue;
    private const float ChangeFrequency = 2f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _timeSinceLastChange = Time.time;
    }
    
    protected override void ChangeFishDirection()
    {
        if(!FirstTimeEntered)
            return;
        
        if(_timeSinceLastChange + ChangeFrequency >= Time.time) return;

        _timeSinceLastChange = Time.time;
        RandomizeDirection();

    }
}

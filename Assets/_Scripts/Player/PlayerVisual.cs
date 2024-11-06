using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerVisual : NetworkBehaviour
{
    private readonly NetworkVariable<Color> _netColor = new();
    private readonly Color[] _colors = { Color.red, Color.blue, Color.green, Color.yellow };
    private int _index;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        _netColor.OnValueChanged += OnValueChanged;
    }
    
    public override void OnDestroy()
    {
        base.OnDestroy();
        _netColor.OnValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(Color previousValue, Color newValue)
    {
        spriteRenderer.color = newValue;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            _index = (int)OwnerClientId;
            CommitNetworkColorServerRpc(GetNextColor());
        }
        else
        {
            spriteRenderer.color = _netColor.Value;
        }
    }

    [ServerRpc]
    private void CommitNetworkColorServerRpc(Color color)
    {
        _netColor.Value = color;
    }

    private Color GetNextColor()
    {
       return _colors[_index++ % _colors.Length];
    }
}

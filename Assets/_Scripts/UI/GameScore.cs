using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameScore : MonoBehaviour
{
    private Dictionary<ulong, int> _scores = new();
    
    private Transform _root;
    void Start()
    {
        _root = transform;
        foreach (Transform t in _root)
        {
            t.gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        Fish.FishKilledEvent += OnFishKilled;
    }
    
    private void OnDestroy()
    {
        Fish.FishKilledEvent -= OnFishKilled;
    }
    
    private void OnFishKilled(Fish.FishStats obj)
    {
        var player = obj.killerId;
        if (_scores.ContainsKey(player))
        {
            _scores[player] += obj.score;
        }
        else
        {
            _scores.Add(obj.killerId, obj.score);
        }

        var i = 0;
        var orderedScores = _scores.OrderByDescending(score => score.Value);
        foreach (var pScore in orderedScores)
        {
            var go =  _root.GetChild(i++).GetComponent<TextMeshProUGUI>();
            go.text = $"Player {pScore.Key}: {pScore.Value}";
            go.gameObject.SetActive(true);
        }
    }
}

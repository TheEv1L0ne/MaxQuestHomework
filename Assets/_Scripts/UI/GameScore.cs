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
    
    //This will not show correct value for players that join late
    //If we want to make this always show correct scores we need to make this 
    //to receive scores from server via some function or make scores network variable
    private void OnFishKilled(Fish.FishStats obj)
    {
        var player = obj.KillerId;
        if (_scores.ContainsKey(player))
        {
            _scores[player] += obj.Score;
        }
        else
        {
            _scores.Add(obj.KillerId, obj.Score);
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

using System;
using Unity.Netcode;
using UnityEngine;

public class SelfScore : MonoBehaviour
{
    [SerializeField] private Transform scoreRoot;
    
    // Start is called before the first frame update
    private void Start()
    {
        HideAllScores();
    }
    
    private void HideAllScores()
    {
        foreach (Transform scoreItem in scoreRoot)
        {
            scoreItem.gameObject.SetActive(false);
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
        AddNewScore(obj);
    }
    
    private void AddNewScore(Fish.FishStats stats)
    {
        var item = GetFirstInactive();

        SelfScoreItem scoreItem;
        
        if (item == null)
        {
            item = scoreRoot.GetChild(0);
            scoreItem = item.GetComponent<SelfScoreItem>();
            item.SetSiblingIndex(scoreRoot.childCount-1);
        }
        else
        {
            scoreItem = item.GetComponent<SelfScoreItem>();
        }

        var fishColor = stats.FishColor;
        var score = stats.Score;
        var fishType = stats.FishType;

        if (stats.KillerId != NetworkManager.Singleton.LocalClient.ClientId)
        {
            fishColor = Color.white;
            score = 0;
            fishType = "None";
        }
        
        scoreItem.SetData($"{fishType}: {score}", fishColor);
    }

    private Transform GetFirstInactive()
    {
        foreach (Transform scoreItem in scoreRoot)
        {
            if (scoreItem.gameObject.activeSelf) continue;
            
            scoreItem.gameObject.SetActive(true);
            return scoreItem;
        }

        return null;
    }
}

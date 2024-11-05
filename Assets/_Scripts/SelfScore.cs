using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SelfScore : MonoBehaviour
{
    [SerializeField] private Transform scoreRoot;

    [SerializeField] private TextMeshProUGUI totalScore;

    private int myScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        HideAllScores();
    }

    private void OnEnable()
    {
        Fish.FishKilledEvent += Fishyfihs;
    }

    private void Fishyfihs(Fish.FishStats obj)
    {
        AddNewScore(obj);
    }

    private void OnDisable()
    {
    }

    private void HideAllScores()
    {
        foreach (Transform scoreItem in scoreRoot)
        {
            scoreItem.gameObject.SetActive(false);
        }
    }
    
    private void AddNewScore(Fish.FishStats obj)
    {
        SetNewScore(obj.fishColor, obj.score, obj.fishType);
    }

    private void SetNewScore(Color fishColor, int score = 0, string fishType = "")
    {
        Debug.Log(fishType);
        
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

        myScore += score;
        totalScore.text = $"Score: {myScore}";
        
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

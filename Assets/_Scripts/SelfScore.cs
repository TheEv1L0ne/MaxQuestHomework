using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfScore : MonoBehaviour
{
    [SerializeField] private Transform scoreRoot;
    // Start is called before the first frame update
    void Start()
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

    public void SetNewScore(Color fishColor, int score = 0, string fishType = "")
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

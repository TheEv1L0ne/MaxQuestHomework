using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelfScoreItem : MonoBehaviour
{
    [SerializeField] private TMP_Text score;
    [SerializeField] private Image backgroundImage;

    public void SetData(string score, Color color)
    {
        this.score.text = score;
        this.backgroundImage.color = color;
    }
}

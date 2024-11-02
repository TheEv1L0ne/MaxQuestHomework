using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    //TODO: Change this to something more normal...
    //Camera can be null. Move to Utils if you have time
    public float Height => Camera.main.orthographicSize;
    public float Width => 16f / 9f * Height;

    [SerializeField] private Toggle autoFireToggle;

    public int playerScore = 0;

    public bool IsAutoOn => autoFireToggle.isOn;

    [SerializeField] private GameObject fish;
    [SerializeField] private Fish commonFish;
    [SerializeField] private Fish rareFish;
    [SerializeField] private Fish epicFish;

    [SerializeField] private SelfScore selfScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        playerScore = 0;
        StartCoroutine(SpawnFish());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnFish();
        }
    }

    private IEnumerator SpawnFish()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            var fishGo = Instantiate(RandomFish(), FishSpawnPoint(), Quaternion.identity);
        }
    }

    private Fish RandomFish()
    {
        var r = Random.Range(0, 100);
        switch (r)
        {
            case < 10:
                return epicFish;
            case >= 10 and < 40:
                return rareFish;
            case >= 40:
                return commonFish;
        }
    }
    
    private Vector3 FishSpawnPoint()
    {
        var spawnPos = Vector3.zero;

        var spawnSide = Random.Range(0, 4);

        float x = 0;
        float y = 0;

        switch (spawnSide)
        {
            case 0:
                x = Random.Range(-(Width + 0.5f), (Width + 0.5f));
                y = Height + 0.5f;
                break;
            case 1:
                x = Width + 0.5f;
                y = Random.Range(-(Height + 0.5f), (Height + 0.5f));
                break;
            case 2:
                x = Random.Range(-(Width + 0.5f), (Width + 0.5f));
                y = -Height - 0.5f;
                break;
            case 3:
                x = -Width - 0.5f;
                y = Random.Range(-(Height + 0.5f), (Height + 0.5f));
                break;
        }

        spawnPos = new Vector3(x, y, 0f);
        
        return spawnPos;
    }

    public void AddScore(Color fishColor, int score = 10, string fishType = "common")
    {
        selfScore.SetNewScore(fishColor,score, fishType);
    }
}

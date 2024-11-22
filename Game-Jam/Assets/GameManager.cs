using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int score = 0;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void IncrementScore()
    {
        score++;  
        UpdateScoreUI();  
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score : " + score;
    }
}

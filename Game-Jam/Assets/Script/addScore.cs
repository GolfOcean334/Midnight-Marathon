using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class addScore : MonoBehaviour
{
    private int score;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Score : " + score;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            incrementScore();
        }
    }

    // function to increment the score
    private void incrementScore()
    {
        score++;
        scoreText.text = "Score : " + score;
    }
}

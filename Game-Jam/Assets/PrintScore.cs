using TMPro;
using UnityEngine;

public class PrintScore : MonoBehaviour
{
    [SerializeField] private int playerScore = 0; // A modifier pour récupérer le vrai score
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + playerScore.ToString();
        }
    }
}

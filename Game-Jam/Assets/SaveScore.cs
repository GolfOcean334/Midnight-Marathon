using UnityEngine;

public class SaveScore : MonoBehaviour
{
    public static SaveScore Instance { get; private set; }

    private int playerScore;

    private void Awake()
    {
        // Vérifier si une instance existe déjà, sinon la créer
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garder cet objet entre les scènes
        }
        else
        {
            Destroy(gameObject); // Détruire l'objet s'il existe déjà
        }
    }

    public void SetScore(int score)
    {
        playerScore = score;
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void ResetScore()
    {
        playerScore = 0; // Remettre le score à zéro
    }
}

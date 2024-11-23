using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Référence au SaveScore (GameManager)
    public SaveScore saveScore;

    public void GoMainScene()
    {
        // Réinitialiser le score avant de charger la scène principale
        if (saveScore != null)
        {
            saveScore.ResetScore();  // Appeler la méthode pour réinitialiser le score
        }

        // Charger la scène "MainScene"
        SceneManager.LoadScene("MainScene");
    }

    public void GoMainMenu()
    {
        // Charger la scène "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}

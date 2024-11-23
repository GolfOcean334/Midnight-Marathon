using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // R�f�rence au SaveScore (GameManager)
    public SaveScore saveScore;

    public void GoMainScene()
    {
        // R�initialiser le score avant de charger la sc�ne principale
        if (saveScore != null)
        {
            saveScore.ResetScore();  // Appeler la m�thode pour r�initialiser le score
        }

        // Charger la sc�ne "MainScene"
        SceneManager.LoadScene("MainScene");
    }

    public void GoMainMenu()
    {
        // Charger la sc�ne "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}

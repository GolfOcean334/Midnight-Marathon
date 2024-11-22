using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneS : MonoBehaviour
{
    public void GoMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
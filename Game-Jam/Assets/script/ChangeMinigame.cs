using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMinigame : MonoBehaviour
{
    public GameObject Game1;
    public GameObject Game2;
    public GameObject Game3;

    private List<GameObject> games = new List<GameObject>();
    private GameObject currentGame;

    // Start is called before the first frame update
    void Start()
    {
        // Ajouter tous les jeux à la liste
        games.Add(Game1);
        games.Add(Game2);
        games.Add(Game3);

        // Initialiser avec un jeu au hasard
        ChooseRandomGame();
    }

    // Fonction pour choisir un jeu au hasard, en évitant de sélectionner celui précédemment joué
    void ChooseRandomGame()
    {
        // Choisir un jeu au hasard mais en excluant le jeu actuel
        List<GameObject> availableGames = new List<GameObject>(games);
        if (currentGame != null)
        {
            availableGames.Remove(currentGame);  // Exclure le jeu actuel
        }

        // Choisir un jeu aléatoire parmi ceux qui restent
        int randomIndex = Random.Range(0, availableGames.Count);
        GameObject nextGame = availableGames[randomIndex];

        // Activer ce jeu et désactiver les autres
        SetActiveGame(nextGame);
    }

    // Fonction pour activer le jeu sélectionné et désactiver les autres
    void SetActiveGame(GameObject game)
    {
        // Désactiver tous les jeux
        foreach (var g in games)
        {
            g.SetActive(false);
        }

        // Mettre à jour le jeu actuel
        currentGame = game;

        // Activer le jeu sélectionné
        currentGame.SetActive(true);
    }

    // Cette fonction peut être appelée lorsque le jeu actuel se termine (par exemple dans la gestion du score)
    public void OnGameOver()
    {
        // Choisir un autre jeu à jouer
        ChooseRandomGame();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Assure-toi d'importer UnityEngine.UI pour manipuler Image

public class MomScript : MonoBehaviour
{
    public Transform Spawn1;
    public Transform Spawn2;
    public Transform Spawn3;
    public Transform InitialPos;  // Position initiale de "mom"
    public HidePhone hidePhoneScript;  // Référence au script HidePhone pour accéder à isVisible et Room

    int RandomSpawn;
    float SpawnInterval; // Intervalle entre chaque spawn
    float StayDuration;  // Durée de séjour après le spawn
    bool Momtrigger;

    Coroutine mycoroutine;

    private void Start()
    {
        // Démarre le processus de spawn continu
        StartCoroutine(MomSpawnCoroutine());
    }

    IEnumerator MomSpawnCoroutine()
    {
        while (true) // La boucle infinie pour faire réapparaître "mom" en continu
        {
            // Attendre un délai aléatoire avant de faire apparaître "mom"
            SpawnInterval = Random.Range(2f, 5f); // Intervalle entre deux apparitions aléatoires
            yield return new WaitForSeconds(SpawnInterval); // Attente avant de spawner

            MomSpawn(); // Appeler la fonction de spawn

            // Durée de séjour aléatoire après le spawn
            StayDuration = Random.Range(3f, 6f); // Durée de séjour aléatoire
            yield return new WaitForSeconds(StayDuration); // Attendre le temps que "mom" reste à sa position

            // Retourner à la position initiale après le temps de séjour
            MoveToInitialPosition(); // Retour à la position initiale
        }
    }

    public void MomSpawn()
    {
        RandomSpawn = Random.Range(1, 4); // Nombre aléatoire entre 1 et 3 pour choisir le point de spawn
        if (RandomSpawn == 1)
        {
            transform.position = Spawn1.position; // Positionner "mom" sur Spawn1
            Momtrigger = true;
        }
        else if (RandomSpawn == 2)
        {
            transform.position = Spawn2.position; // Positionner "mom" sur Spawn2
            Momtrigger = true;
        }
        else if (RandomSpawn == 3)
        {
            transform.position = Spawn3.position; // Positionner "mom" sur Spawn3
            Momtrigger = true;
        }
        MomKiller();
    }

    // Fonction pour déplacer "mom" vers sa position initiale
    public void MoveToInitialPosition()
    {
        transform.position = InitialPos.position;  // Déplacer "mom" à la position initiale
    }

    // Fonction qui gère la couleur de la Room dans HidePhone en fonction de isVisible et Momtrigger
    public void MomKiller()
    {
        if (Momtrigger && hidePhoneScript != null)
        {
            if (hidePhoneScript.isvisble) // Vérifie si la Room est visible
            {
                // Change la couleur de la Room en rouge si isVisible est true
                Image roomImage = hidePhoneScript.Room.GetComponent<Image>();
                if (roomImage != null)
                {
                    Debug.Log("Changement de couleur en rouge");
                    roomImage.color = Color.red;  // Change la couleur de l'image en rouge
                }
                else
                {
                    Debug.LogWarning("Le composant Image n'a pas été trouvé sur Room !");
                }
            }
            else
            {
                // Si la Room n'est pas visible, on remet la couleur à la normale
                Image roomImage = hidePhoneScript.Room.GetComponent<Image>();
                if (roomImage != null)
                {
                    Debug.Log("Changement de couleur en blanc");
                    roomImage.color = Color.white;  // Retour à la couleur d'origine (ou autre couleur)
                }
            }
        }
    }
}

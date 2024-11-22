using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Assure-toi d'importer UnityEngine.UI pour manipuler Image
using UnityEngine.SceneManagement;
public class MomScript : MonoBehaviour
{
    public Transform Spawn1;
    public Transform Spawn2;
    public Transform Spawn3;
    public Transform InitialPos;  // Position initiale de "mom"
    public HidePhone hidePhoneScript;  // R�f�rence au script HidePhone pour acc�der � isVisible et Room

    int RandomSpawn;
    float SpawnInterval; // Intervalle entre chaque spawn
    float StayDuration;  // Dur�e de s�jour apr�s le spawn
    bool Momtrigger;

    Coroutine mycoroutine;

    private void Start()
    {
        // D�marre le processus de spawn continu
        StartCoroutine(MomSpawnCoroutine());
    }

    private void Update()
    {
        MomKiller();
    }

    IEnumerator MomSpawnCoroutine()
    {
        while (true) // La boucle infinie pour faire r�appara�tre "mom" en continu
        {
            // Attendre un d�lai al�atoire avant de faire appara�tre "mom"
            SpawnInterval = Random.Range(10, 20); // Intervalle entre deux apparitions al�atoires
            yield return new WaitForSeconds(SpawnInterval); // Attente avant de spawner

            MomSpawn(); // Appeler la fonction de spawn

            // Dur�e de s�jour al�atoire apr�s le spawn
            StayDuration = Random.Range(1f, 2f); // Dur�e de s�jour al�atoire
            yield return new WaitForSeconds(StayDuration); // Attendre le temps que "mom" reste � sa position

            // Retourner � la position initiale apr�s le temps de s�jour
            MoveToInitialPosition(); // Retour � la position initiale
        }
    }

    public void MomSpawn()
    {
        RandomSpawn = Random.Range(1, 3); // Nombre al�atoire entre 1 et 3 pour choisir le point de spawn
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
    }

    // Fonction pour d�placer "mom" vers sa position initiale
    public void MoveToInitialPosition()
    {
        transform.position = InitialPos.position;  // D�placer "mom" � la position initiale
        Momtrigger = false; // Assurez-vous que Momtrigger soit r�initialis� apr�s qu'elle soit retourn�e � sa position initiale
    }

    // Fonction qui g�re la couleur de la Room dans HidePhone en fonction de isVisible et Momtrigger
    public void MomKiller()
    {
        if (Momtrigger && hidePhoneScript != null)
        {
            Debug.Log("Momtrigger: " + Momtrigger);
            Debug.Log("isvisble: " + hidePhoneScript.isvisble);

            if (hidePhoneScript.isvisble == true && Momtrigger == true)
            {
                Debug.Log("Loading EndMenu scene");
                SceneManager.LoadScene("EndMenu");
            }
        }
    }
}

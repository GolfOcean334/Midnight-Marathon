using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePhone : MonoBehaviour
{
    public GameObject Room;
    public bool isvisble = true;

    void Start()
    {
        // Mettre � jour isvisble en fonction de l'�tat actuel de Room
        isvisble = Room.activeInHierarchy;
    }

    void Update()
    {
        // Vous pouvez ajouter d'autres logiques ici si n�cessaire
    }

    public void changeVisibilityOnClick()
    {
        if (Room.activeInHierarchy == true)
        {
            Room.SetActive(false);
            isvisble = false;
        }
        else
        {
            Room.SetActive(true);
            isvisble = true;
        }
    }
}

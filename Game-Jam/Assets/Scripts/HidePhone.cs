using UnityEngine;

public class HidePhone : MonoBehaviour
{
    public GameObject Room;
    public bool isvisble;


    void Start()
    {
    }

    void Update()
    {
        
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

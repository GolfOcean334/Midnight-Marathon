using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{

    [SerializeField] private SeparateGameManager.ElementType objectType;
    public SeparateGameManager.ElementType ObjectType
    {
        get => objectType;
        set => objectType = value;
    }
    public bool isOnCorrectGround;
    
    // Start is called before the first frame update
    void Start()
    {
        isOnCorrectGround = false;
        
        ObjectType = Random.Range(0, 2) == 0 ? SeparateGameManager.ElementType.Left : SeparateGameManager.ElementType.Right; // set the object type to a random value (left or right)
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null) // if the object collides with a ground
        {
            if (collision.gameObject.GetComponent<Ground>().GroundType == objectType) isOnCorrectGround = true; // if the object is on the correct ground
            GetComponent<Rigidbody2D>().simulated = false; // stop the object from moving
        }
    }
    
    // Set the color of the object based on its type
    private void SetColor()
    {
        if (objectType == SeparateGameManager.ElementType.Left) // if the object is of type Left
        {
            GetComponent<SpriteRenderer>().color = new Color(150, 0 , 0); // set the color to red
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 150 , 200); // set the color to blue
        }
    }
}

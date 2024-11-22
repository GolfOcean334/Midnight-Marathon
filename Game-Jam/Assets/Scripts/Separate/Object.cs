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
    public bool isInTheAir;
    
    // Start is called before the first frame update
    void Start()
    {
        isOnCorrectGround = false;
        isInTheAir = true;
        ObjectType = Random.Range(0, 2) == 0 ? SeparateGameManager.ElementType.Left : SeparateGameManager.ElementType.Right; // set the object type to a random value (left or right)
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>() == null) return; // if the object collides with a ground
        if (collision.gameObject.GetComponent<Ground>().GroundType == objectType) isOnCorrectGround = true; // if the object is on the correct ground
        isInTheAir = false;
        GetComponent<Rigidbody2D>().simulated = false; // stop the object from moving
    }
}

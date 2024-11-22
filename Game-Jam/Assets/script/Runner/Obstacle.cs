using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject trigger;
    public bool shouldBeDestroyed;
    
    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == trigger)
        {
            shouldBeDestroyed = true;
        }
    }
}

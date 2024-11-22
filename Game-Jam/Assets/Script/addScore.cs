using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    void Start()
    {

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncrementScore();
                Destroy(gameObject);
            }
        }
    }
}
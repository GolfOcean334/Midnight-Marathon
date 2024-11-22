using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePipe : MonoBehaviour
{
    private float speed = 5f;
    private float leftEdge = -6.40f;
    
    private void Start()
    {
    }

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}

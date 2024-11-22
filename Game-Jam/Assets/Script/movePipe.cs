using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePipe : MonoBehaviour
{
    public float speed = 5f;
    public float leftEdge = -5;

    private void Start()
    {
        // leftEdge = Camera.main.ScreenToViewportPoint(Vector3.zero).x - 1f;
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

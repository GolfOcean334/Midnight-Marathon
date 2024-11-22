using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Vector2 pos;
    private float speed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos.x += speed * Time.deltaTime; // incrémenter la position en X en fonction de la vitesse et du temps écoulé

        transform.position = new Vector2(transform.position.x + pos.x, transform.position.y);
    }
}

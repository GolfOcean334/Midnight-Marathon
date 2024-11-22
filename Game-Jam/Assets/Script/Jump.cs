using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody2D rb;
    private float jumpForce;
    private float maxRotation; 
    private float rotationSpeed; 
    private bool isFlapping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpForce = 5f;
        maxRotation = 30f;
        rotationSpeed = 3f; 
        isFlapping = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flap();
        }

        if (rb.velocity.y < 0)
        {
            float targetRotation = Mathf.LerpAngle(rb.rotation, -maxRotation, Time.deltaTime * rotationSpeed);
            rb.rotation = targetRotation;
        }
        else if (rb.velocity.y > 0)
        {
            float targetRotation = Mathf.LerpAngle(rb.rotation, maxRotation / 2f, Time.deltaTime * rotationSpeed);
            rb.rotation = targetRotation;
        }
    }

    private void Flap()
    {
        if (isFlapping) return; 

        isFlapping = true;
        rb.velocity = Vector2.zero; 
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 

        rb.rotation = 0f;

        Debug.Log("Flap");

        StartCoroutine(ResetFlap());
    }

    private IEnumerator ResetFlap()
    {
        yield return new WaitForSeconds(0.1f);
        isFlapping = false;
    }
}

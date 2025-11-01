using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float rotationSpeed = 180f;
    public float moveSpeed = 5f;
    public float maxSpeed = 5f;
    
    private Rigidbody2D rb;


    public LevelGen levelGen;


    void Awake()
        {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float rotationInput = Input.GetAxis("Horizontal");
        float moveInput = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.forward * -rotationInput * rotationSpeed * Time.deltaTime);

        Vector2 force = transform.up * moveInput * moveSpeed;
        rb.AddForce(force, ForceMode2D.Force);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Exit")
        {
            levelGen.NextRoom();
        }
        
    }
}

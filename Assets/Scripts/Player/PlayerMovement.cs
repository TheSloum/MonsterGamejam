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

    [HideInInspector] public bool canMove = true;
    public float movementThreshold = 0.1f;

    public Animator childAnimator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (childAnimator == null)
            Debug.LogWarning("Animator enfant non assigné !");
    }

    void Update()
    {
        if (!canMove) return;

        float rotationInput = Input.GetAxis("Horizontal");
        float moveInput = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.forward * -rotationInput * rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.up * moveInput * moveSpeed);

        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        // Animation basée uniquement sur l'input
        if (childAnimator != null)
        {
            bool isWalking = Mathf.Abs(moveInput) > 0.01f || Mathf.Abs(rotationInput) > 0.01f;
            childAnimator.SetBool("isWalking", isWalking);

            Debug.Log(isWalking ? "Walking" : "Idle");
        }
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Exit")
        {
            levelGen.NextRoom();
        }
    }

    public bool IsMoving()
    {
        float moveInput = Input.GetAxis("Vertical");
        float rotationInput = Input.GetAxis("Horizontal");
        return Mathf.Abs(moveInput) > 0.01f || Mathf.Abs(rotationInput) > 0.01f;
    }
}

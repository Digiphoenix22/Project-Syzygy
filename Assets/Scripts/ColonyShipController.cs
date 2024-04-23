using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyShipController : MonoBehaviour
{
   
    private Rigidbody2D rb;
    private Vector3 myPosition;
    private Quaternion myRotation;
    private Vector2 lastVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }
    void Update()
    {
        if (rb != null)
        {
            myPosition = transform.position;
            myRotation = transform.rotation.normalized;
            lastVelocity = rb.velocity;
        }
    }

    private void AdjustRotationAfterCollision(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Wall") && rb != null)
        {
            // Reflects the angle of the ship's velocity and simulates a bounce effect
            var speed = lastVelocity.magnitude;
            var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 1); // Ensure there's always some movement after collision

            // Corrects ship's rotation to match the new direction
            AdjustRotationAfterCollision(direction);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject != null)
            {
                if (rb != null)
                {
                    Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

                    // Calculate the momentum transfer (halved) from the player to the asteroid
                    Vector2 momentumTransfer = playerRb.velocity * 0.6f;
                    rb.velocity += momentumTransfer;

                    // Slow the player by half
                    playerRb.velocity *= 0.5f;
                }
            }
        }        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
    if (collision.gameObject.CompareTag("ColonyShip"))
        {
            if (rb.velocity.magnitude != 0)
            {
                rb.velocity *= 0;
                Vector3 colPosition = collision.gameObject.transform.position;
                Quaternion colRotation = collision.gameObject.transform.rotation.normalized;
                if (myPosition.x > colPosition.x)
                {
                    myPosition = colPosition;
                    myPosition.x += 2; // Adjust this value as needed for correct positioning
                }
                else
                {
                    myPosition = colPosition;
                    myPosition.x -= 2;
                }
                myRotation = colRotation;
                transform.rotation = myRotation;
                transform.position = myPosition;
                Destroy(rb); // Consider alternatives to destroying the Rigidbody to avoid potential issues
                transform.parent = collision.gameObject.transform;

                // Notify the GameManager that a ship has been aligned
                GameManager.instance.AddAlignedShip();
            }
        }
    }

}

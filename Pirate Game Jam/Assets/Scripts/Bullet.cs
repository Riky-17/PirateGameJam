using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 3f;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed; // Bullet moves towards the right when instantiated
    }
}

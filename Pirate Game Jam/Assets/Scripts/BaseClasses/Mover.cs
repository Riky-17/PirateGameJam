using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 6;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void AddForce(Vector2 dir, float speed, float accelRate)
    {
        Vector2 velocity = dir * speed;
        Vector2 velocityDiff = velocity - rb.linearVelocity;
        Vector2 force = velocityDiff * accelRate;
        force*= rb.mass;
        rb.AddForce(force);
    }

    public void FaceDirection(Vector2 dir)
    {
        if (dir.x < 0) // Left Direction
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (dir.x > 0) // Right Direction
            transform.rotation = Quaternion.identity;
    }
}

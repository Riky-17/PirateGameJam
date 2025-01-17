using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveInput;

    [SerializeField] float speed = 6;
    
    bool canMove = true;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        moveInput = Vector2.zero;

        //getting the move input
        if(canMove)
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                moveInput += Vector2.up;
            if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                moveInput += Vector2.down;
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                moveInput += Vector2.left;
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                moveInput += Vector2.right;

            moveInput = moveInput.normalized;
        }
    }

    void FixedUpdate()
    {
        //physics calc
        Vector2 velocityInput = moveInput * speed;
        Vector3 velocityDiff = velocityInput - rb.linearVelocity;
        float accelRate = 7f;
        Vector3 force = velocityDiff * accelRate;
        rb.AddForce(force);
    }
}

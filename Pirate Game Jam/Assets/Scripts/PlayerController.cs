using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer sr;
    Vector2 mousePos;

    Rigidbody2D rb;
    Vector2 moveInput;

    [SerializeField] float speed = 6;
    [SerializeField] float recoilForce = 15f;

    //how much should the recoil last for
    float recoilTime = .3f;
    float lastRecoil;

    bool canMove = true;

    void Awake()
    { 
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    

    void Update()
    {
        Movement();
        MousePosition();
        SpriteRotation();
        Shoot();
    }

    void FixedUpdate()
    {
        PhysicsCalc();
        Recoil();
    }

    void Movement()
    {
        moveInput = Vector2.zero;

        //getting the move input
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                moveInput += Vector2.up;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                moveInput += Vector2.down;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                moveInput += Vector2.left;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                moveInput += Vector2.right;

            moveInput = moveInput.normalized;
        }
    }

    void MousePosition()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Gets Vector2 mouse position
    }

    void PhysicsCalc()
    {
        //physics calc
        Vector2 velocityInput = moveInput * speed;
        Vector3 velocityDiff = velocityInput - rb.linearVelocity;
        float accelRate = 7f;
        Vector3 force = velocityDiff * accelRate;
        rb.AddForce(force);
    }

    void SpriteRotation()
    {
        // Rotates sprite based on mouse position
        Vector2 mouseVector = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        float rotZ = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //shoot

            //recoil
            lastRecoil = recoilTime;
        }
    }

    void Recoil()
    {
        if (lastRecoil > 0)
        {
            //physics calc
            Vector2 gunToMouse = new Vector2(transform.position.x - mousePos.x, transform.position.y - mousePos.y).normalized;
            Vector2 recoil = gunToMouse * recoilForce;
            Vector2 recoilDiff = recoil * lastRecoil / recoilTime;
            float accelRate = 10f;
            Vector2 force = recoilDiff * accelRate;

            rb.AddForce(force);

            lastRecoil -= Time.fixedDeltaTime;
        }
    }
}

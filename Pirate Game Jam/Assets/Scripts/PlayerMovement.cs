using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public Vector2 mousePos;
    Rigidbody2D rb;
    Vector2 moveInput;

    [SerializeField] float speed = 6;

    bool canMove = true;

    //weapon objects -> assigned in inspector
    [SerializeField] GameObject[] weapons;

    void Start()
    { 
        rb = GetComponent<Rigidbody2D>();

        //disabling game objects except for the first weapon 
        DisablingWeapons();
        weapons[0].SetActive(true);
    }
    

    void Update()
    {
        Movement();
        MousePosition();
        SpriteRotation();
        SwitchWeapon();
    }

    void FixedUpdate()
    {
        PhysicsCalc();       
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

    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DisablingWeapons();
            weapons[0].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DisablingWeapons();
            weapons[1].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DisablingWeapons();
            weapons[2].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DisablingWeapons();
            weapons[3].SetActive(true);
        }
    }

    void DisablingWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
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

   

    
}

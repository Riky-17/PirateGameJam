using UnityEngine;

public class Civilian : MonoBehaviour
{
    Rigidbody2D rb;
    

    [SerializeField] bool debugMode = true;

    [SerializeField] float speed = 4f;

    //how long should the civilian walk for
    [Range(.5f, 1), SerializeField] float walkTimeMin = .5f;
    [Range(.75f, 1.5f), SerializeField] float walkTimeMax = 1f;
    float walkTime;
    float walkTimer;

    //how long should the civilian wait for
    [SerializeField] float waitTime = 1f;
    float waitTimer;

    //how far can the civilian get from the originPoint
    [SerializeField] float maxDistance;
    [SerializeField] Vector2 originPoint;

    // the probability to pick left
    int leftDirChances = 50;

    Vector2 walkDir;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    void Start()
    {
        GameManager.Instance.Civilians.Add(this);
        GetNewDirection();
    }

    void OnDisable() => GameManager.Instance.Civilians.Remove(this);

    void Update()
    {
        if(walkTimer >= walkTime)
        {
            walkDir = Vector2.zero;
            waitTimer+= Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                //resetting timers
                waitTimer = 0;
                walkTimer = 0;

                CalculateDirectionChances();
                GetNewDirection();
            }
        }
    }

    void FixedUpdate()
    {
        if(walkTimer < walkTime)
            walkTimer += Time.fixedDeltaTime;

        if(transform.position.x <= originPoint.x - maxDistance / 2 || transform.position.x >= originPoint.x + maxDistance / 2)
        {
            rb.linearVelocityX = 0;
            walkTimer = walkTime;
        }
        
        Vector2 velocity = walkDir * speed;
        Vector2 velocityDiff = velocity - rb.linearVelocity;
        float accel = 3;
        Vector2 force = velocityDiff * accel;
        rb.AddForce(force);
    }

    void GetNewDirection()
    {
        walkTime = Random.Range(walkTimeMin, walkTimeMax);
        //101 because exclusive
        int chance = Random.Range(0, 101);
        walkDir = chance <= leftDirChances ? Vector2.left : Vector2.right;
    }

    void CalculateDirectionChances()
    {
        float currentX = transform.position.x;
        float originX = originPoint.x;
        float leftMostX = originX - maxDistance / 2;
        float rightMostX = originX + maxDistance / 2;

        leftDirChances =  Mathf.RoundToInt(100 * Mathf.InverseLerp(leftMostX, rightMostX, currentX)); 
        // Debug.Log(leftDirChances);
    }

    void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(originPoint, new(maxDistance, 1, 0));
            Gizmos.DrawSphere(originPoint, .25f);
        }
    }
}

using UnityEngine;

public class CivilianLady : Civilian
{
    Animator anim;

    [SerializeField] bool debugMode = true;

    [SerializeField] float speed = 3f;

    //how long should the civilian walk for
    [Range(.5f, 1), SerializeField] float walkTimeMin = .5f;
    [Range(.75f, 1.5f), SerializeField] float walkTimeMax = 1f;
    float walkTime;
    float walkTimer;

    //how far can the civilian get from the originPoint
    [SerializeField] float maxDistance;
    [SerializeField] Vector2 originPoint;

    // the probability to pick left
    int leftDirChances = 50;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CheckColorFlash();
        
        if(isThrowingItem)
        {
            if(throwingTimer >= 1)
            {
                transform.rotation = initialRot;
                ThrowItem(itemToThrow);
                throwingTimer = 0;
                isThrowingItem = false;
            }
            else
            {
                throwingTimer+= Time.deltaTime * rotatingSpeed;
                Rotate();
                return;
            }
        }

        if(walkTimer >= walkTime)
        {
            dir = Vector2.zero;
            waitTimer+= Time.deltaTime;
            anim.Play("Idle");

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

        if((transform.position.x <= originPoint.x - maxDistance / 2 && dir.x < 0) || (transform.position.x >= originPoint.x + maxDistance / 2 && dir.x > 0))
        {
            rb.linearVelocityX = 0;
            walkTimer = walkTime;
        }
        
        Vector2 velocity = dir * speed;
        Vector2 velocityDiff = velocity - rb.linearVelocity;
        float accel = 3;
        Vector2 force = velocityDiff * accel;
        rb.AddForce(force);
    }

    protected override void GetNewDirection()
    {
        walkTime = Random.Range(walkTimeMin, walkTimeMax);
        //101 because exclusive
        int chance = Random.Range(1, 101);

        dir = chance <= leftDirChances ? Vector2.left : Vector2.right;
        FaceDirection(dir);
        anim.Play("Walk");
    }

    void CalculateDirectionChances()
    {
        float currentX = transform.position.x;
        float originX = originPoint.x;
        float leftMostX = originX - maxDistance / 2;
        float rightMostX = originX + maxDistance / 2;

        leftDirChances =  Mathf.RoundToInt(100 * Mathf.InverseLerp(leftMostX, rightMostX, currentX));
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

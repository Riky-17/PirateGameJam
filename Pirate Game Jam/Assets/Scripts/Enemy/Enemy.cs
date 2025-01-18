using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] bool DebugMode = true;
    [SerializeField] float lookDistance = 5;
    [SerializeField] float maxDistance = 3;
    [SerializeField] float speed = 5;
    float maxDistanceOffset = .75f;
    
    Collider2D[] colliders;

    PlayerMovement playerMovement;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        if(playerMovement != null && (playerMovement.transform.position - transform.position).magnitude > lookDistance)
            playerMovement = null;
    }

    void FixedUpdate()
    {
        if(playerMovement == null)
            LookForPlayer();
        else
            Movement();
        
    }

    private void Movement()
    {
        // getting a copy vector of the player and making that vector y equals to the this enemy
        Vector3 playerFlat = playerMovement.transform.position;
        playerFlat.y = transform.position.y;
        Vector2 enemyToPlayerFlat = playerFlat - transform.position;

        // getting the non flat distance to calculate the actual distance
        float enemyToPlayer = (playerMovement.transform.position - transform.position).magnitude;
        
        Vector2 velocity = enemyToPlayerFlat.normalized * speed;
        Vector2 velocityDiff = velocity - rb.linearVelocity;
        float accelRate = 3/*enemyToPlayer > maxDistance + maxDistanceOffset || enemyToPlayer < maxDistance +maxDistanceOffset ? 6 : 10*/;
        Vector2 force = velocityDiff * accelRate;

        if(enemyToPlayer > maxDistance + maxDistanceOffset)
            rb.AddForce(force);
        else if(enemyToPlayer < maxDistance - maxDistanceOffset)
            rb.AddForce(-force);
    }

    private void LookForPlayer()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, lookDistance);

        foreach (Collider2D collider in colliders)
            if(collider.TryGetComponent(out playerMovement))
                break;
    }

    void OnDrawGizmos()
    {
        if (DebugMode)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, lookDistance);
    
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxDistance);
    
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, maxDistance + maxDistanceOffset);
            Gizmos.DrawWireSphere(transform.position, maxDistance - maxDistanceOffset);
        }
    }
}

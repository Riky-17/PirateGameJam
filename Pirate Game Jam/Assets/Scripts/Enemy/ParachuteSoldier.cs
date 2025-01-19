using System;
using UnityEngine;

public class ParachuteSoldier : Enemy
{
    [SerializeField] LayerMask agentMask;
    [SerializeField] LayerMask groundMask;

    bool isGrounded = false;

    void FixedUpdate()
    {
        if(player == null)
            LookForPlayer();

        if(!isGrounded)
        {
            if (Physics2D.Raycast(transform.position, -transform.up, .5f, groundMask))
            {
                gameObject.layer = (int)MathF.Log(agentMask.value, 2);
                isGrounded = true;
            }
        }
        else
            if(player != null)
                ChasePlayer();
    }

    protected override void ChasePlayer()
    {
        // getting a copy vector of the player and making that vector y equals to the this enemy
        Vector3 playerFlat = player.transform.position;
        playerFlat.y = transform.position.y;
        Vector2 enemyToPlayerFlat = playerFlat - transform.position;

        // getting the non flat distance to calculate the actual distance
        float enemyToPlayer = (player.transform.position - transform.position).magnitude;
        
        if(enemyToPlayer > maxDistance + maxDistanceOffset)
            AddForce(enemyToPlayerFlat.normalized, 3);
        else if(enemyToPlayer < maxDistance - maxDistanceOffset)
            AddForce(-enemyToPlayerFlat.normalized, 3);
    }
}

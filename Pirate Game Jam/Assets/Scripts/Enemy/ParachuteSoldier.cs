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
            if (Physics2D.Raycast(transform.position, -transform.up, 2f, groundMask))
            {
                isGrounded = true;
                gameObject.layer = (int)MathF.Log(agentMask.value, 2);
                rb.gravityScale = 1;
            }
        }
        else
            if(player != null)
                ChasePlayer();
            else
                Patrol();
    }
}

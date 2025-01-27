using UnityEngine;

public class BossParachuteEnemy : Enemy
{
    [SerializeField] LayerMask agentMask;
    [SerializeField] LayerMask groundMask;

    public GameObject parachute;

    bool isGrounded = false;
    
    void Start() => GameManager.Instance.BossEnemies.Add(this);
    void OnDisable() => GameManager.Instance.BossEnemies.Remove(this);

    void FixedUpdate()
    {
        if(player == null)
            LookForPlayer();

        if (!isGrounded)
        {
            if (Physics2D.Raycast(transform.position, -transform.up, 2f, groundMask))
            {
                isGrounded = true;
                gameObject.layer = (int)Mathf.Log(agentMask, 2);
                rb.gravityScale = 1;
                parachute.SetActive(false);
            }
        }
        else
        {
            if (player != null)
                ChasePlayer();
            else
                Patrol();
        }
    }

    public override void Die() => Destroy(gameObject);
}

using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] protected float damage = 10;
    [HideInInspector] protected LayerMask shooterLayer;
    protected Rigidbody2D rb;

    protected virtual void Awake() => rb = GetComponent<Rigidbody2D>();

    public GameObject bulletHitPrefab;

    IHealth target;

    void FixedUpdate()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, transform.right, .25f);

        if(raycastHit.collider != null && raycastHit.collider.TryGetComponent(out target) && raycastHit.collider.gameObject.layer != shooterLayer)
        {
            target.Damage(damage);
            Instantiate(bulletHitPrefab, transform.position + (transform.right * 0.3f), transform.rotation);
        }
        else
        {
            raycastHit = Physics2D.Raycast(transform.position, -transform.right, .25f);

            if(raycastHit.collider == null)
                return;

            if(raycastHit.collider.TryGetComponent(out target))
            {
                if(raycastHit.collider.gameObject.layer == shooterLayer)
                    return;
                
                target.Damage(damage);
                Instantiate(bulletHitPrefab, transform.position + (-transform.right * 0.3f), transform.rotation);
            }
            
        }

        Destroy(gameObject);
    }

    public void Init(float speed, LayerMask shooterLayer, float damage)
    {
        rb.linearVelocity = transform.right * speed;
        this.shooterLayer = shooterLayer;
        this.damage = damage;
    }
}

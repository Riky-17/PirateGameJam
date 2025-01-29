using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] protected float damage = 10;
    [HideInInspector] protected GameObject shooter;
    [SerializeField] protected LayerMask ignoreBulletMask;
    protected Rigidbody2D rb;

    protected virtual void Awake() => rb = GetComponent<Rigidbody2D>();

    public GameObject bulletHitPrefab;

    IHealth target;

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, .25f);

        if(hit.collider != null && hit.collider.TryGetComponent(out target) && hit.collider.gameObject != shooter && hit.collider.gameObject.layer != Mathf.Log(ignoreBulletMask, 2))
        {
            target.Damage(damage);
            Instantiate(bulletHitPrefab, transform.position + (transform.right * 0.3f), transform.rotation);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, -transform.right, .25f);

            if(hit.collider == null)
                return;

            if(hit.collider.gameObject.layer == Mathf.Log(ignoreBulletMask, 2))
                return;

            if(hit.collider.TryGetComponent(out target))
            {
                if(hit.collider.gameObject == shooter)
                    return;
                
                target.Damage(damage);
                Instantiate(bulletHitPrefab, transform.position + (-transform.right * 0.3f), transform.rotation);
            }

            if (hit.collider.TryGetComponent(out Bullet _))
                return;
            
        }

        Destroy(gameObject);
    }

    public void Init(float speed, GameObject shooter, float damage)
    {
        rb.linearVelocity = transform.right * speed;
        this.shooter = shooter;
        this.damage = damage;
    }
}

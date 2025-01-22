using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] protected float damage = 10;
    [HideInInspector] protected LayerMask shooterLayer;
    protected Rigidbody2D rb;

    protected virtual void Awake() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, transform.right, .5f);

        if(raycastHit.collider == null)
            return;

        if(raycastHit.collider.TryGetComponent(out IHealth target))
        {
            if(raycastHit.collider.gameObject.layer == shooterLayer)
                return;

            target.Damage(damage);
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

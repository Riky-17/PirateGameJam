using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float damage = 10;
    public LayerMask shooterLayer;
    Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Start() => rb.linearVelocity = transform.right * speed; // Bullet moves towards the right when instantiated

    void FixedUpdate()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, transform.right, .5f);

        if(raycastHit.collider == null)
            return;

        Debug.Log(shooterLayer.value);

        if(raycastHit.collider.TryGetComponent(out IHealth target))
        {
            if(raycastHit.collider.gameObject.layer == shooterLayer)
                return;

            target.Damage(damage);
        }

        Destroy(gameObject);
    }
}

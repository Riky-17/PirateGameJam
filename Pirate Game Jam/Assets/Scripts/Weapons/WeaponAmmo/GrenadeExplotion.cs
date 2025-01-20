using System.Collections;
using UnityEngine;

public class GrenadeExplotion : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float damage = 10f;
    public LayerMask shooterLayer;
    [SerializeField] float explosionDelay = 1.5f;
    Rigidbody2D rb;
    float explosionRadius = 15f;
    public GameObject explosionPrefab;
    SpriteRenderer sr;
    bool hasExploted = false;

    Collider2D[] colliders;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed; // Bullet moves towards the right when instantiated
        StartCoroutine(ExplosionDelay());
    }

    void Explosion()
    {
        GameObject tempExp = Instantiate(explosionPrefab, transform.position, transform.rotation);

        //making it big
        tempExp.transform.localScale *= explosionRadius;

        colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        if(colliders != null && colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                if(collider.TryGetComponent(out IHealth target))
                {
                    if(collider.gameObject.layer == shooterLayer.value)
                        continue;

                    target.Damage(damage);
                }
            }
        }

        //disabling sr 
        sr.enabled = false;

        //destroying the object
        Destroy(tempExp, 0.3f);                             
    }
    IEnumerator ExplosionDelay()
    {
        
        yield return new WaitForSeconds(explosionDelay);
        if (!hasExploted)
        {
            Explosion();
            hasExploted = true;
        }      
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasExploted)
        {
            Explosion();
            hasExploted = true;
        }
        
    }
}

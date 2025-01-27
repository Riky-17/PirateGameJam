using System;
using System.Collections;
using UnityEngine;

public class GrenadeExplotion : Bullet
{
    public static Action<Transform> onExplode;

    [SerializeField] float explosionDelay = 1.5f;

    float explosionRadius = 15f;
    public GameObject explosionPrefab;
    SpriteRenderer sr;
    bool hasExploded = false;

    Collider2D[] colliders;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // rb.linearVelocity = transform.right * speed; // Bullet moves towards the right when instantiated
        StartCoroutine(ExplosionDelay());
    }

    void OnDisable()
    {
        if(!hasExploded)
            Explosion();
    }

    void FixedUpdate()
    {
        if(hasExploded)
            return;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, .25f);
        if(hit.collider == null || hit.collider.gameObject == shooter)
            return;
        Explosion();
    }

    void Explosion()
    {
        GameObject tempExp = Instantiate(explosionPrefab, transform.position, transform.rotation);
        hasExploded = true;
        onExplode?.Invoke(transform);

        colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius * .23f);
        if(colliders != null && colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                if(collider.TryGetComponent(out IHealth target))
                {
                    if(collider.gameObject == shooter)
                        continue;

                    target.Damage(damage);
                }
            }
        }

        //disabling sr 
        sr.enabled = false;

        //destroying the object
        Destroy(tempExp, 0.3f);                             
        Destroy(gameObject, 0.3f);                             
    }

    IEnumerator ExplosionDelay()
    {
        
        yield return new WaitForSeconds(explosionDelay);
        if (!hasExploded)
            Explosion();
    }
}

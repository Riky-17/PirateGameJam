using System.Collections;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float explosionDelay = 2f;
    Rigidbody2D rb;
    float explosionRadius = 15f;
    CircleCollider2D cc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        rb.linearVelocity = transform.right * speed; // Bullet moves towards the right when instantiated
        StartCoroutine(ExplosionDelay(explosionDelay));
    }

    IEnumerator ExplosionDelay(float explosion)
    {
        yield return new WaitForSeconds(explosion);
        //animation

        //exploting it 
        cc.radius = explosionRadius;       
        
    }
}

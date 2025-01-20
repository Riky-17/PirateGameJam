using System.Collections;
using UnityEngine;

public class GrenadeExplotion : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float explosionDelay = 1.5f;
    Rigidbody2D rb;
    float explosionRadius = 15f;
    public GameObject explosionPrefab;
    SpriteRenderer sr;
    bool hasExploted = false;

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

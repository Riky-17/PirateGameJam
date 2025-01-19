using System.Collections;
using UnityEngine;

public class GrenadeExplotion : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float explosionDelay = 2f;
    Rigidbody2D rb;
    float explosionRadius = 15f;
    public GameObject explosionPrefab;
    SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed; // Bullet moves towards the right when instantiated
        StartCoroutine(ExplosionDelay(explosionDelay));      
    }

    IEnumerator ExplosionDelay(float explotion)
    {
        yield return new WaitForSeconds(explotion);

        GameObject tempExp = Instantiate(explosionPrefab, transform.position, transform.rotation);

        //making it big
        tempExp.transform.localScale *= explosionRadius;

        //disabling sr 
        sr.enabled = false;

        //destroying the object
        Destroy(tempExp, 0.3f);                             
    }
}

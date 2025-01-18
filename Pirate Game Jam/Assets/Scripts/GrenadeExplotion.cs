using System.Collections;
using UnityEngine;

public class GrenadeExplotion : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float explotionDelay = 2f;
    Rigidbody2D rb;
    float explotionRadius = 15f;
    CircleCollider2D cc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        rb.linearVelocity = transform.right * speed; // Bullet moves towards the right when instantiated
        StartCoroutine(ExplotionDelay(explotionDelay));
    }

    IEnumerator ExplotionDelay(float explotion)
    {
        yield return new WaitForSeconds(explotion);
        //animation
        Debug.Log("Explotion Occured");
        //exploting it 
        cc.radius = explotionRadius;       
        
    }
}

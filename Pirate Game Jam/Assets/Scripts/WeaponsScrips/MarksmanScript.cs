using UnityEngine;

public class MarksmanScript : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    PlayerMovement pm;
    //how much should the recoil last for
    float recoilTime = .3f;
    float lastRecoil;
    [SerializeField] float recoilForce = 15f;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //shoot

            //recoil
            lastRecoil = recoilTime;
        }
    }

    private void FixedUpdate()
    {
        Recoil();
    }
    
    void Recoil()
    {
        if (lastRecoil > 0)
        {
            //physics calc
            Vector2 gunToMouse = new Vector2(transform.position.x - pm.mousePos.x, transform.position.y - pm.mousePos.y).normalized;
            Vector2 recoil = gunToMouse * recoilForce;
            Vector2 recoilDiff = recoil * lastRecoil / recoilTime;
            float accelRate = 10f;
            Vector2 force = recoilDiff * accelRate;

            rb.AddForce(force);

            lastRecoil -= Time.fixedDeltaTime;
        }
    }
}

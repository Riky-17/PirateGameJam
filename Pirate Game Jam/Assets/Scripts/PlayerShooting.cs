using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    Camera mainCamera;
    Rigidbody2D rb;
    [SerializeField] float recoilForce = 15f;

    //how much should the recoil last for
    float recoilTime = .3f;
    float lastRecoil;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //shoot

            //recoil
            lastRecoil = recoilTime;
        }
    }

    void FixedUpdate()
    {
        if(lastRecoil > 0)
        {   
            //getting mouse position
            Vector3 mousePos = Input.mousePosition;
            mousePos = mainCamera.ScreenToWorldPoint(mousePos);
            mousePos.z = transform.position.z;

            //physics calc
            Vector2 gunToMouse = (transform.position - mousePos).normalized;
            Vector2 recoil = gunToMouse * recoilForce;
            Vector2 recoilDiff = recoil * lastRecoil / recoilTime;
            float accelRate = 10f;
            Vector2 force = recoilDiff * accelRate;

            rb.AddForce(force);

            lastRecoil -= Time.fixedDeltaTime;
        }
    }
}

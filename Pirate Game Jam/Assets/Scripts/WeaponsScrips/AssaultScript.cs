using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AssaultScript : WeaponSystem
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    PlayerMovement pm;
    Animator anim;

    //how much should the recoil last for
    float recoilTime = .3f;
    float lastRecoil;
    [SerializeField] float recoilForce = 15f;


    public Transform shootingPoint;
    public GameObject bulletPrefab;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponentInParent<PlayerMovement>();

        //assigning bullets and reload time 
        bulletsNum = 1000;
        initialBulletNum = bulletsNum;
        reloadTime = 3f;
    }
    private void OnEnable()
    {
        if (bulletsNum <= 0)
        {
            canShoot = false;
            StartCoroutine(ReloadingSpeed(reloadTime));
        }
    }

    void Update()
    {
        Shoot(shootingPoint, bulletPrefab);
    }

    public override void Shoot(Transform muzzle, GameObject bullet)
    {
        if (Input.GetMouseButton(0))
        {
            if (canShoot && bulletsNum > 0)
            {
                //shoot
                float maxFriction = 50;
                bool goingUp = false;
                float bulletsFriction = 50f;
                Quaternion bulletSprite = Quaternion.Euler(0f, 0f, bulletsFriction);
                GameObject tempBullet = Instantiate(bullet, muzzle.position, transform.rotation);
                if(bulletsFriction <= -maxFriction)
                {
                    goingUp = true;
                }
                else if(bulletsFriction >= maxFriction)
                {
                    goingUp = false;
                }
                //--------------
                if (goingUp)
                {
                    bulletsFriction -= 5f;
                }
                else
                {
                    bulletsFriction += 5f;
                }

                bulletsNum--;
                Destroy(tempBullet, 2f);
                //anim.Play("Shoot");
                //recoil
                lastRecoil = recoilTime;

                //check ammo
                if (bulletsNum == 0)
                {
                    canShoot = false;
                    StartCoroutine(ReloadingSpeed(reloadTime));
                }
            }                                
            
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

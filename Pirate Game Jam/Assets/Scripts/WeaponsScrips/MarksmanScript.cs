using UnityEngine;

public class MarksmanScript : WeaponSystem
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    PlayerMovement pm;
    //how much should the recoil last for
    float recoilTime = .3f;
    float lastRecoil;
    [SerializeField] float recoilForce = 15f;

    public Transform shootingPoint;
    public GameObject bulletPrefab;


    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pm = GetComponentInParent<PlayerMovement>();

        //assigning the bullets and time to reload 
        bulletsNum = 20;
        reloadTime = 5f;
        initialBulletNum = bulletsNum;
    }

    private void OnEnable()
    {
        if(bulletsNum <= 0)
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
        if (Input.GetMouseButtonDown(0))
        {

            if (canShoot && bulletsNum > 0)
            {
                //shoot
                GameObject tempBullet = Instantiate(bullet, muzzle.position, transform.rotation);
                tempBullet.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed * Time.deltaTime);
                bulletsNum--;
                Destroy(tempBullet, 2f);
                anim.Play("Shoot");
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
        else if (lastRecoil <= 0)
        {
            anim.Play("Idle"); // Plays Idle animation when recoil is finished
        }
    }
}

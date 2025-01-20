using UnityEngine;

public class GrenadeScript : WeaponSystem
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
        //Canvas
        weaponaryText = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WeaponDisplay>();

        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponentInParent<PlayerMovement>();

        bulletsNum = 5;
        initialBulletNum = bulletsNum;
        reloadTime = 5f;
    }

    private void OnEnable()
    {
        weaponaryText.updateAmmo(bulletsNum.ToString());
        weaponaryText.updateWeapon(this.gameObject.name);
        if (bulletsNum <= 0)
        {
            canShoot = false;
            StartCoroutine(ReloadingSpeed(reloadTime));
        }
    }

    void Update()
    {
        Shoot(shootingPoint, bulletPrefab);
        ManualReload(reloadTime);
    }

    public override void Shoot(Transform muzzle, GameObject bullet)
    {
        if (Input.GetMouseButtonDown(0) && PanelsManager.canReadInput)
        {
            if (canShoot && bulletsNum > 0)
            {
                //shoot

                GameObject tempBullet = Instantiate(bullet, muzzle.position, transform.rotation);
                bulletsNum--;
                weaponaryText.updateAmmo(bulletsNum.ToString());

                Destroy(tempBullet, 2f);
                anim.SetTrigger("isShooting");
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

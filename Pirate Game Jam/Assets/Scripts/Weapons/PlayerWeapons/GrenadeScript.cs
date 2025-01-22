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

    void Awake()
    {
        //Canvas
        weaponryText = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WeaponDisplay>();

        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponentInParent<PlayerMovement>();

        bulletsNum = 5;
        initialBulletNum = bulletsNum;
        reloadTime = 10f;

        weaponIndex = 3;
    }

    private void OnEnable()
    {
        //UI
        weaponryText.UpdateWeaponChosen(2);
        weaponryText.updateAmmo(bulletsNum.ToString());
        weaponryText.updateWeapon(this.gameObject.name);
        WeaponInfo();

        //reload
        if (bulletsNum <= 0)
        {
            canShoot = false;
            Reload(this);
        }
    }

    void Update()
    {
        Shoot(shootingPoint, bullet);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload(this);
        }
    }

    public override void Shoot(Transform muzzle, BulletSO bullet)
    {
        if (Input.GetMouseButtonDown(0) && PanelsManager.canReadInput)
        {
            if (canShoot && bulletsNum > 0)
            {
                //shoot
                CheckShooting(this);
                Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, transform.rotation);
                //initializing the bullet script
                tempBullet.Init(bullet.speed, pm.gameObject.layer, bullet.damage);

                bulletsNum--;
                weaponryText.updateAmmo(bulletsNum.ToString());

                Destroy(tempBullet, 2f);
                anim.SetTrigger("isShooting");
                //recoil
                lastRecoil = recoilTime;

                //check ammo
                if (bulletsNum == 0)
                {
                    canShoot = false;
                    Reload(this);
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

    public void ChangeSpriteColor(Color color) => sr.color = color;
    
    public void WeaponInfo()
    {
        weaponryText.PauseWeaponInfo("70", "1", recoilTime.ToString(), initialBulletNum.ToString());
    }
}

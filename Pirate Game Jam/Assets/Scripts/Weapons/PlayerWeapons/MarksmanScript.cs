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


    void Awake()
    {

        //UI
        weaponaryText = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WeaponDisplay>();
        weaponaryText.updateWeapon(this.gameObject.name);
        weaponaryText.UpdateWeaponChosen(0);
        weaponaryText.PauseWeaponInfo("10", "0.5", recoilTime.ToString(), initialBulletNum.ToString());


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
        //UI
        weaponaryText.UpdateWeaponChosen(0);
        weaponaryText.updateAmmo(bulletsNum.ToString());
        weaponaryText.updateWeapon(this.gameObject.name);
        weaponInfo();

        //RELOAD
        if (bulletsNum <= 0)
        {
       
            canShoot = false;
            StartCoroutine(ReloadingSpeed(reloadTime));
        }
    }

    void Update()
    {
        Shoot(shootingPoint, bullet);
        ManualReload(reloadTime);
    }
    
    public override void Shoot(Transform muzzle, BulletSO bullet)
    {
        if (Input.GetMouseButtonDown(0) && PanelsManager.canReadInput)
        {

            if (canShoot && bulletsNum > 0)
            {
                //shoot
                Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, transform.rotation);
                //initializing the bullet script
                tempBullet.Init(bullet.speed, pm.gameObject.layer, bullet.damage);

                bulletsNum--;
                weaponaryText.updateAmmo(bulletsNum.ToString());

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

    public void weaponInfo()
    {
        weaponaryText.PauseWeaponInfo("30", "0.1", recoilTime.ToString(), initialBulletNum.ToString());
    }
}

using Unity.Mathematics;
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
        weaponryText = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WeaponDisplay>();
        weaponryText.updateWeapon(this.gameObject.name);
        weaponryText.UpdateWeaponChosen(0);
        weaponryText.PauseWeaponInfo("10", "0.5", recoilTime.ToString(), initialBulletNum.ToString());


        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pm = GetComponentInParent<PlayerMovement>();

        //assigning the bullets and time to reload 
        bulletsNum = 20;
        reloadTime = 5f;
        initialBulletNum = bulletsNum;

        weaponIndex = 1;
    }

    private void OnEnable()
    {
        //UI
        weaponryText.UpdateWeaponChosen(0);
        weaponryText.updateAmmo(bulletsNum.ToString());
        weaponryText.updateWeapon(this.gameObject.name);
        weaponInfo();

        //RELOAD
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
                CheckShooting(this);
                //shoot
                Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, transform.rotation);
                //initializing the bullet script
                tempBullet.Init(bullet.speed, pm.gameObject.layer, bullet.damage);

                bulletsNum--;
                weaponryText.updateAmmo(bulletsNum.ToString());

                Destroy(tempBullet, 2f);
                anim.Play("Shoot");

                //recoil
                lastRecoil = recoilTime;
                
                //check ammo
                if (bulletsNum <= 0)
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
        else if (lastRecoil <= 0)
        {
            anim.Play("Idle"); // Plays Idle animation when recoil is finished
        }
    }

    public void weaponInfo()
    {
        weaponryText.PauseWeaponInfo("30", "0.1", recoilTime.ToString(), initialBulletNum.ToString());
    }
}

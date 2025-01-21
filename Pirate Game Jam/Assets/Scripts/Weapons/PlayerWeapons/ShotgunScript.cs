using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class ShotgunScript : WeaponSystem
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
        weaponaryText = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WeaponDisplay>();

        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponentInParent<PlayerMovement>();

        //assigning the bullets and time to reload 
        bulletsNum = 5;
        reloadTime = 3f;
        initialBulletNum = 5;
    }

    private void OnEnable()
    {
        //UI
        weaponaryText.UpdateWeaponChosen(3);
        weaponaryText.updateAmmo(bulletsNum.ToString());
        weaponInfo();
        weaponaryText.updateWeapon(this.gameObject.name);

        // reload 
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
                for (int bulletsFriction = 30; bulletsFriction >= -30; bulletsFriction-= 15)
                {
                    Quaternion tempRotation = Quaternion.Euler(0f, 0f, bulletsFriction);
                    Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, muzzle.rotation * tempRotation);
                    //initializing the bullet script
                    tempBullet.Init(bullet.speed, pm.gameObject.layer, bullet.damage);

                    Destroy(tempBullet.gameObject, 1f);
                }
                
                anim.SetTrigger("isShooting");
                bulletsNum--;
                weaponaryText.updateAmmo(bulletsNum.ToString());
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

    public void weaponInfo()
    {
        weaponaryText.PauseWeaponInfo("50", "0.5", recoilTime.ToString(), initialBulletNum.ToString());
    }
}

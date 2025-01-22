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
        weaponryText = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WeaponDisplay>();

        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponentInParent<PlayerMovement>();

        //assigning the bullets and time to reload 
        bulletsNum = 5;
        reloadTime = 3f;
        initialBulletNum = 5;

        weaponIndex = 4;
    }

    private void OnEnable()
    {
        //UI
        weaponryText.UpdateWeaponChosen(3);
        weaponryText.updateAmmo(bulletsNum.ToString());
        weaponInfo();
        weaponryText.updateWeapon(this.gameObject.name);

        // reload 
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
                weaponryText.updateAmmo(bulletsNum.ToString());
                //anim.Play("Shoot");

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

    public void weaponInfo()
    {
        weaponryText.PauseWeaponInfo("50", "0.5", recoilTime.ToString(), initialBulletNum.ToString());
    }
}

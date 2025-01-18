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
    public GameObject bulletPrefab;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponentInParent<PlayerMovement>();

        //assigning the bullets and time to reload 
        bulletsNum = 3;
        reloadTime = 3f;
        initialBulletNum = 10;
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ReloadingSpeed(reloadTime));
        }
    }
    public override void Shoot(Transform muzzle, GameObject bullet)
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (canShoot && bulletsNum > 0)
            {

                //shoot
                float bulletsFriction = 30f;
                GameObject[] tempBullets = new GameObject[5];
                List<GameObject> DestroyableBullets = new List<GameObject>();
                foreach (GameObject tempbullet in tempBullets)
                {
                    Quaternion tempRotation = Quaternion.Euler(0f, 0f, bulletsFriction);                  
                    bulletsFriction -= 15;    
                    DestroyableBullets.Add(Instantiate(bullet, muzzle.position, muzzle.rotation * tempRotation));
                    
                }
                foreach(GameObject tempbullet in DestroyableBullets)
                {
                    Destroy(tempbullet, 1f);
                }
                bulletsNum--;
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

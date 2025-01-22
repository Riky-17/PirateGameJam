using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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

    private bool isShooting = true;
    private int sprayAngle = 20;
    private float fireRafe = 0.1f;

    void Awake()
    {
        //Canvas
        weaponryText = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WeaponDisplay>();

        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponentInParent<PlayerMovement>();

        //assigning bullets and reload time 
        bulletsNum = 80;
        initialBulletNum = bulletsNum;
        reloadTime = 8f;

        weaponIndex = 2;
    }
    private void OnEnable()
    {
        //UI
        weaponryText.UpdateWeaponChosen(1);
        weaponryText.updateAmmo(bulletsNum.ToString());
        weaponryText.updateWeapon(this.gameObject.name);
        WeaponInfo();

        //reload
        if (bulletsNum <= 0)
        {
            canShoot = false;
            Reload(this);
        }
        isShooting = true;
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
        if (PanelsManager.canReadInput)
        {

            if (Input.GetMouseButton(0) && !Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (canShoot && bulletsNum > 0)
                {
                    //shoot
                   
                    if (isShooting)
                    {
                        CheckShooting(this);
                        int Innacuracy = Random.Range(-sprayAngle, sprayAngle); ;
                        Quaternion bulletSprite = Quaternion.Euler(0f, 0f, Innacuracy);
                        Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, transform.rotation * bulletSprite);
                        //initializing the bullet script
                        tempBullet.Init(bullet.speed, pm.gameObject.layer, bullet.damage);
                        
                        //fire the shooting event
                        onShotFired?.Invoke(transform);

                        bulletsNum--;
                        weaponryText.updateAmmo(bulletsNum.ToString());
                        StartCoroutine(fireRate());
                        Destroy(tempBullet.gameObject, 2f);
                        anim.SetTrigger("isShooting");
                        //recoil
                        lastRecoil = recoilTime;
                    }

                    //check ammo
                    if (bulletsNum == 0)
                    {
                        canShoot = false;
                        Reload(this);
                    }
                }

            }
        }
    }
    private IEnumerator fireRate()
    {
        isShooting = false;
        yield return new WaitForSeconds(fireRafe);
        isShooting = true;
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
        weaponryText.PauseWeaponInfo("10", fireRafe.ToString(), recoilTime.ToString(), initialBulletNum.ToString());
    }
}

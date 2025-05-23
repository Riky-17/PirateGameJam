using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSystem : MonoBehaviour
{
    //event fired when the weapon shoots
    public static Action<Transform> onShotFired;
    //components
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected PlayerMovement pm;
    protected Animator anim;

    //how much should the recoil last for
    protected float recoilTime = .3f;
    protected float lastRecoil;
    [SerializeField] protected float recoilForce = 15f;

    //weapon stats
    public float ReloadTime => reloadTime;
    [SerializeField] protected float reloadTime;
    public static float ReloadMultiplier = 1;
    public float InitialBulletNum => initialBulletNum;
    [SerializeField] protected int initialBulletNum = 3;
    protected int bulletsNum;
    [SerializeField] protected float fireRate;
    protected float fireRateTimer;
    public static float FireRateMultiplier = 1;

    [SerializeField] protected BulletSO bullet;
    // this is for level ups
    protected float damageMultiplier = 1;
    // this is for boost items
    public static float DamageBoostMultiplier = 1;
    [SerializeField] Transform shootingPoint;

    protected bool canShoot = true;
    protected bool isReloading = false;

    [SerializeField] protected byte weaponIndex;

    protected WeaponDisplay weaponryText;

    int amountOfCoroutines = 0;
    //dictionary to keep track of my coroutines 
    public Dictionary<WeaponSystem, Coroutine> myRunningCoroutines = new Dictionary<WeaponSystem, Coroutine>();
    //timer for reloading weapons when running out of ammo

    void Awake()
    {
        //Canvas
        weaponryText = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WeaponDisplay>();

        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pm = GetComponentInParent<PlayerMovement>();

        bulletsNum = initialBulletNum;
    }

    protected virtual void OnEnable()
    {
        //UI
        weaponryText.UpdateWeaponChosen(1);
        weaponryText.updateAmmo(bulletsNum.ToString());
        weaponryText.updateWeapon(gameObject.name);
        WeaponInfo();

        //reload
        if (bulletsNum <= 0)
        {
            canShoot = false;
            Reload(this);
        }
        
        fireRateTimer = fireRate;
    }

    void Update()
    {
        if(PanelsManager.canReadInput && Input.GetMouseButton(0) && canShoot && bulletsNum > 0 && CheckFireRate())
            Shoot(shootingPoint, bullet);
        if (Input.GetKeyDown(KeyCode.R))
            Reload(this);
    }

    private void FixedUpdate() => Recoil();

    public abstract void Shoot(Transform muzzle, BulletSO bullet); //shoot method called

    public void Reload(WeaponSystem weapon)
    {
        if (!weapon.isReloading)
        {
            Coroutine coroutine = CoroutineManager.Instance.StartingCoroutine(ReloadCoroutine());
            weapon.isReloading = true;
            myRunningCoroutines[weapon] = coroutine;
        }      
    }

    public void CheckShooting(WeaponSystem weapon)
    {
        weapon.isReloading = false;
        if (myRunningCoroutines.ContainsKey(weapon) && weapon.bulletsNum > 0)
        {
            if (myRunningCoroutines.TryGetValue(weapon, out Coroutine value))
            {
                CoroutineManager.Instance.StoppingCoroutine(value);
                myRunningCoroutines.Remove(weapon);
                weaponryText.disablingLoadingPanels(weaponIndex);
            }
        }
    }

    public bool CheckFireRate()
    {
        if(fireRateTimer >= fireRate * FireRateMultiplier)
            return true;

        fireRateTimer+= Time.deltaTime;
        return false;
    }

    public virtual void UpgradeStats()
    {
        reloadTime*= .90f;
        initialBulletNum+= 1;
        bulletsNum+= 1;
        damageMultiplier+= .1f;
        fireRate*= .90f;
    }
    
    public IEnumerator ReloadCoroutine()
    {
        amountOfCoroutines++;
        float remainTime = reloadTime * ReloadMultiplier;
        if (weaponryText != null)
            weaponryText.updateAmmo("Reloading");

        while (remainTime > 0)
        {
            isReloading = true;
            remainTime -= Time.deltaTime;
            if (weaponryText != null)
                weaponryText.loadingInfo(weaponIndex, Mathf.Ceil(remainTime).ToString());

            yield return null;
        }
        if (weaponryText != null)
            weaponryText.disablingLoadingPanels(weaponIndex);
        canShoot = true;
        bulletsNum = initialBulletNum;

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

    protected void InitBullet(Bullet bullet) => bullet.Init(this.bullet.speed, pm.gameObject, this.bullet.damage * damageMultiplier * DamageBoostMultiplier);

    public void MultiplyBullet(float multiplier)
    {
        initialBulletNum = (int)MathF.Floor(initialBulletNum * multiplier);
        bulletsNum = (int)MathF.Floor(bulletsNum * multiplier);
        if(gameObject.activeSelf)
            weaponryText.updateAmmo(bulletsNum.ToString());
    }

    public Color WeaponSpriteColor() => sr.color;
    public void ChangeSpriteColor(Color color) => sr.color = color;

    float TotalDamage => bullet.damage * damageMultiplier;
    public void WeaponInfo() => weaponryText.PauseWeaponInfo(TotalDamage.ToString("0.00"), fireRate.ToString("0.00"), reloadTime.ToString("0.00"), initialBulletNum.ToString("0.00"), gameObject.name);
}

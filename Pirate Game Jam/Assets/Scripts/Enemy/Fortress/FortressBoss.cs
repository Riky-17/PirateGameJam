using System.Collections.Generic;
using UnityEngine;

public class FortressBoss : Boss
{
    [SerializeField] Sprite attackSprite;
    [SerializeField] Sprite deathSprite;

    [SerializeField] List<Transform> windows;
    List<FortressWindow> weapons;
    [SerializeField] BulletSO grenade;
    [SerializeField] BulletSO shotgunBullet;
    [SerializeField] BulletSO assaultRifleBullet;
    [SerializeField] BulletSO marksmanBullet;

    public List<Enemy> SpawnedEnemies { get; private set;}

    protected override void InitBoss()
    {
        sr.GetComponent<SpriteRenderer>();
        sr.sprite = attackSprite;
        SpawnedEnemies = new();

        weapons = new()
        {
            new FortressGrenadeLauncher(this, windows[0], grenade),
            new FortressShotgun(this, windows[1], shotgunBullet),
            new FortressAssaultRifle(this, windows[2], assaultRifleBullet),
            new FortressMarksman(this, windows[3], marksmanBullet)
        };

        attacks = new() 
        {
            new FortressOverclock(this, player, null),
        };
    }

    protected override void PickAttack()
    {
        base.PickAttack();
        FortressWindow randomWindow = weapons[Random.Range(0, weapons.Count)];
        if(CurrentAttack is FortressOverclock overclock)
            overclock.SetWindow(randomWindow);
        shootingPoint = randomWindow.Transform;
    }

    protected override void Update()
    {
        TakeAim();
        base.Update();

        foreach (FortressWindow weapon in weapons)
        {
            if(!weapon.isSpecialAttacking)
                weapon.Shoot();
        }
    }

    //removing the base fixedUpdate
    void FixedUpdate() {}

    public override void TakeAim()
    {
        foreach (FortressWindow weapon in weapons)
        {
            if(!weapon.isSpecialAttacking)
                weapon.TakeAim(player);
        }
    }

    protected override void LoadNextScene()
    {
        
    }

    protected override void UpdateColor(Color color) => sr.color = color;
}

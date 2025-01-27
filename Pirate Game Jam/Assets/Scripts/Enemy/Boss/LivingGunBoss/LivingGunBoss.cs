using System.Collections.Generic;
using UnityEngine;

public class LivingGunBoss : Boss
{
    [SerializeField] List<LivingGunBossGun> weapons;
    LivingGunBossGun currentWeapon;

    

    protected override void InitBoss()
    {
        currentWeapon = weapons[0];
    }

    protected override void Update()
    {
        base.Update();
        TakeAim();

        currentWeapon.Shoot();
    }

    public override void TakeAim()
    {
        float x = player.transform.position.x - transform.position.x;
        if(x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.identity;

        base.TakeAim();
    }


    void SwitchWeapon()
    {
        int randomWeaponIndex = Random.Range(0, weapons.Count);
        if(weapons[randomWeaponIndex] == currentWeapon)
            return;
    } 

    protected override void LoadNextScene()
    {
        //load Credits Scene
    }

    protected override void UpdateColor(Color color)
    {
        sr.color = color;
    }
}

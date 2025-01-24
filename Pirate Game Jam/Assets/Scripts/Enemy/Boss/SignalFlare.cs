using UnityEngine;

public class SignalFlare : BossAttack
{
    public SignalFlare(Boss boss, PlayerMovement player, Transform shootingPoint, BulletSO bullet, Enemy enemy) : base(boss, player, shootingPoint, bullet) { enemyToSpawn = enemy; }

    const float CAMERA_MAX_WIDTH = 30 / 2;
    const float CAMERA_MAX_HEIGHT = 17 / 2;

    Enemy enemyToSpawn;
    int enemiesAmount = 2;

    // the delay after the grenade explodes
    float enemySpawnTime = 1f;
    float enemySpawnTimer;

    float grenadeShootTime = .3f;
    float grenadeShootTimer;

    bool grenadeShot;

    public override void InitAttack()
    {
        base.InitAttack();
        boss.Stop();
        enemySpawnTimer = 0;
        grenadeShootTimer = 0;
        grenadeShot = false;
    }

    public override void Attack()
    {
        if (!grenadeShot)
        {
            if(grenadeShootTimer < grenadeShootTime)
            {
                Quaternion cannonRotation = Quaternion.Euler(0, 0, 90);
                boss.RotateGun(cannonRotation);
                grenadeShootTimer+= Time.deltaTime;
                return;
            }
            else
            {
                grenadeShot = true;
                Bullet grenade = boss.InstantiateBullet(bullet.bulletPrefab, shootingPoint.position, shootingPoint.rotation);
                InitBullet(grenade);
                boss.DestroyBullet(grenade, .5f);
            }
        }

        boss.TakeAim();
        enemySpawnTimer+= Time.deltaTime;

        if(enemySpawnTimer >= enemySpawnTime)
        {
            for (int i = 0; i < enemiesAmount; i++)
            {
                float x = Random.Range(-CAMERA_MAX_WIDTH + 1, CAMERA_MAX_WIDTH - 1);
                Vector2 pos = new(x + boss.CenterPoint.x, CAMERA_MAX_HEIGHT);
                Enemy enemy = boss.InstantiateEnemy(enemyToSpawn, pos, Quaternion.identity);
                if(boss is TankBoss tankBoss)
                    tankBoss.SpawnedEnemies.Add(enemy);
                isAttackDone = true;
            }
        }
    }
}

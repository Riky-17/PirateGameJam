using UnityEngine;

public class FortressOverclock : BossAttack
{
    public FortressOverclock(Boss boss, PlayerMovement player, BulletSO bullet, BossParachuteEnemy enemy) : base(boss, player, bullet) { enemyToSpawn = enemy; }

    const float CAMERA_MAX_WIDTH = 30 / 2;
    const float CAMERA_MAX_HEIGHT = 17 / 2;

    bool SpawnedEnemy;

    Enemy enemyToSpawn;

    FortressWindow window;
    float overclock = .5f;

    float overclockTime = 5f;
    float overclockTimer;

    public override void InitAttack()
    {
        base.InitAttack();
        overclockTimer = 0;
    }

    public override void Attack()
    {
        if(!SpawnedEnemy)
        {
            SpawnedEnemy = true;
            float x = Random.Range(-CAMERA_MAX_WIDTH + 1, CAMERA_MAX_WIDTH - 1);
            Vector2 pos = new(x + boss.CenterPoint.x, CAMERA_MAX_HEIGHT);
            boss.InstantiateEnemy(enemyToSpawn, pos, Quaternion.identity);
        }


        if(overclockTimer >= overclockTime)
        {
            window.Overclock(1);
            isAttackDone = true;
        }
        else
        {
            window.Overclock(overclock);
            overclockTimer+= Time.deltaTime;
        }
    }

    public void SetWindow(FortressWindow window) => this.window = window;
}

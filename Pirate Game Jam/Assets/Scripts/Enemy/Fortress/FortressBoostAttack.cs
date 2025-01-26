using UnityEngine;

public class FortressOverclock : BossAttack
{
    public FortressOverclock(Boss boss, PlayerMovement player, BulletSO bullet) : base(boss, player, bullet) {}

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
        Debug.Log(window);
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

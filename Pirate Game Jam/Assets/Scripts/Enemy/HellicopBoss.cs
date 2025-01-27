using UnityEngine;

public class HellicopBoss : Boss
{
    SpriteRenderer srCannon;
    Transform trCannon;
    [Space]
    [SerializeField] BulletSO grenade;
    [SerializeField] BulletSO bullet;
    [SerializeField] Enemy enemyToSpawn;
    [SerializeField] Transform cannon;
    
    protected override void InitBoss()
    {
        sr = GetComponent<SpriteRenderer>();
        srCannon = GetComponentInChildren<SpriteRenderer>();
        trCannon = GetComponentInChildren<Transform>();
        attacks = new()
        {
           new MachineGun(this, player, bullet),
           new CarpetBoom(this, player, grenade)
        };
    }

    protected override void LoadNextScene() => GameManager.Instance.LoadScene(3);
    
    protected override void UpdateColor(Color color)
    {
        sr.color = color;
        srCannon.color = color;
    }

    public override void TakeAim()
    {
        Vector2 shootDir = (player.transform.position - transform.position).normalized;
        Vector3 upwards = Vector3.Cross(Vector3.forward, shootDir);
        Vector3 forward = Vector3.forward;

        if(shootDir.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            upwards = -upwards;
            forward = - forward;
        }
        else
            transform.rotation = Quaternion.identity;

        cannon.rotation = Quaternion.LookRotation(forward, upwards);
    }

    protected override void DeactivateSprite()
    {
        sr.enabled = false;
        srCannon.enabled = false;
    }
}

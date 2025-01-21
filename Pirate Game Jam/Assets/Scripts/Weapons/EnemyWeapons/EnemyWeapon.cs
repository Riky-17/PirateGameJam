using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    protected Animator anim;

    GameObject attachedEnemy;
    protected PlayerMovement player;
    protected bool isShooting;

    [SerializeField] protected BulletSO bulletSO;

    public Transform shootingPoint;

    public float shootingTimer;
    public float lastShot;

    public abstract void Shoot(PlayerMovement player, Vector3 dir, Quaternion aimRotation);
    public abstract void Idle();


    void Awake()
    {
        anim = GetComponent<Animator>();
        attachedEnemy = transform.parent.gameObject;
    }

    // void Update()
    // {
    //     if (isShooting)
    //     {
    //         SpriteRotation();
    //         //isShooting = true;
    //         //anim.Play("Shoot");
    //     }
    //     else
    //     {
    //         transform.rotation = attachedEnemy.transform.rotation;
    //         transform.localScale = new Vector2(1, 1);

    //         //isShooting = false;
    //         //anim.Play("Idle");
    //     }
    // }

    public void SpriteRotation(Quaternion aimRot)
    {
        // Rotates sprite based on player position
        // Vector2 mouseVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        // float rotZ = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;

        // if (rotZ >= -90 && rotZ <= 90)
        // {
        //     transform.rotation = Quaternion.Euler(0, 0, rotZ);
        //     transform.localScale = new Vector2(1, 1);
        // }
        // else
        // {
        //     transform.rotation = Quaternion.Euler(0, 0, rotZ);
        //     transform.localScale = new Vector2(-1, -1);
        // }
        // if(aimRot.eulerAngles.z < -90 || aimRot.eulerAngles.z > 90)
        //     transform.localScale = new(-1, -1);

        transform.rotation = aimRot;
    }

    public void Reset() => transform.rotation = attachedEnemy.transform.rotation;
}

using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    protected Animator anim;

    GameObject attachedEnemy;
    GameObject player;
    protected bool isShooting;

    [SerializeField] protected BulletSO bulletSO;

    public float shootingTimer = 1.5f;

    public abstract void Shoot(Vector3 dir, Quaternion aimRotation);


    void Awake()
    {
        anim = GetComponent<Animator>();
        attachedEnemy = this.transform.parent.gameObject;
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (isShooting)
        {
            SpriteRotation();
        }
        else
        {
            transform.rotation = attachedEnemy.transform.rotation;
            //transform.localScale = attachedEnemy.transform.localScale;
            transform.localScale = new Vector2(1, 1);
        }
    }

    public void SpriteRotation()
    {
        // Rotates sprite based on mouse position
        Vector2 mouseVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        float rotZ = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;

        if (rotZ >= -90 && rotZ <= 90)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            transform.localScale = new Vector2(-1, -1);
        }
    }
}

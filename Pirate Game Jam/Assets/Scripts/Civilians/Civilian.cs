using UnityEngine;

public class Civilian : MonoBehaviour, IHealth
{
    public float Health { get; set; }
    public float MaxHealth { get; set; } = 50;
    //how long should the red hit flash last for
    protected float hitTime = .1f;
    protected float hitTimer;

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    [SerializeField] protected float waitTime = 1;
    protected float waitTimer;

    protected Vector2 dir;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Health = MaxHealth;
    }

    void Start()
    {
        GameManager.Instance.Civilians.Add(this);
        GetNewDirection();
    }

    void OnDisable() => GameManager.Instance.Civilians.Remove(this);

    void Update()
    {
        if(hitTimer > 0)
            hitTimer-= Time.deltaTime;
        else
            sr.color = Color.white;

        if(waitTimer >= waitTime)
        {
            waitTimer = 0;
            GetNewDirection();
            return;
        }

        waitTimer+= Time.deltaTime;
    }

    protected virtual void GetNewDirection()
    {
        //101 because exclusive
        int chance = Random.Range(1, 101);

        dir = chance <= 50 ? Vector2.left : Vector2.right;
        FaceDirection(dir);
    }

    protected void FaceDirection(Vector2 dir)
    {
        if(dir.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (dir.x > 0)
            transform.rotation = Quaternion.identity;
    }

    public void Damage(float damageAmount)
    {
        Health-= damageAmount;
        hitTimer = hitTime;
        sr.color = Color.red;
        Debug.Log(gameObject.name + " Health: " + Health);
        if(Health <= 0)
            Die();
    }

    //TODO
    public void Die()
    {
        Destroy(gameObject);
    }
}

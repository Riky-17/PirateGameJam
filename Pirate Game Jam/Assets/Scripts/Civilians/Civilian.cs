using UnityEngine;

public class Civilian : MonoBehaviour, IHealth, IItemPicker
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

    ShortColorFlash shortColorFlash;

    //throwing item fields
    protected bool isThrowingItem = false;
    protected StatBoostItem itemToThrow;
    protected float rotatingSpeed = 2;
    protected float throwingTimer;
    protected Quaternion initialRot;
    Vector2 throwDir;
    int throwTurns = 1;

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
        CheckColorFlash();

        if(isThrowingItem)
        {
            if(throwingTimer >= 1)
            {
                transform.rotation = initialRot;
                ThrowItem(itemToThrow);
                throwingTimer = 0;
                isThrowingItem = false;
            }
            else
            {
                throwingTimer+= Time.deltaTime * rotatingSpeed;
                Rotate();
                return;
            }
        }

        if(waitTimer >= waitTime)
        {
            waitTimer = 0;
            GetNewDirection();
            return;
        }

        waitTimer+= Time.deltaTime;
    }

    protected void CheckColorFlash()
    {
        if(shortColorFlash.duration > 0)
        {
            sr.color = shortColorFlash.Color;
            shortColorFlash.duration-= Time.deltaTime;
            if(shortColorFlash.duration <= 0)
            {
                sr.color = Color.white;
                shortColorFlash = default;
            }
        }
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

    protected void Rotate()
    {
        float radAngle = throwTurns * (Mathf.PI * 2);
        float z = Mathf.Cos(radAngle * throwingTimer);
        float x = Mathf.Sin(radAngle * throwingTimer);

        Vector3 forward = new(x, 0, z);
        Quaternion rot = initialRot * Quaternion.LookRotation(forward, transform.up);
        transform.rotation = rot;
    }

    protected void ThrowItem(StatBoostItem item)
    {
        StatBoostItem itemToThrow = Instantiate(item, (Vector2)transform.position + throwDir * 2, Quaternion.identity);
        itemToThrow.ThrowItem(throwDir);
        Destroy(item.gameObject);
    }

    public void Heal(float healAmount)
    {
        Health+= healAmount;
        shortColorFlash = new(Color.green);
        if(Health > MaxHealth)
            Health = MaxHealth;
    }

    public void Damage(float damageAmount)
    {
        Health-= damageAmount;
        shortColorFlash = new(Color.red);
        if(Health <= 0)
            Die();
    }

    //TODO
    public void Die()
    {
        ObjectivesManager.Instance.KillCivilian();
        Destroy(gameObject);
    }

    public void PickItem(PickableItem item)
    {
        switch (item)
        {
            case StatBoostItem statBoostItem:
                if (!isThrowingItem)
                {
                    isThrowingItem = true;
                    float x = item.transform.position.x - transform.position.x;
                    if(x == 0)
                        x = 1;
    
                    throwDir = new(x, 0);
                    throwDir = throwDir.normalized;
                    throwDir += Vector2.up;
                    throwDir = throwDir.normalized;
                    itemToThrow = statBoostItem;
                    statBoostItem.DisableItem();
                    initialRot = transform.rotation;
                }
            break;
            default:
                item.Effect(this);
            break;
        }
    }
}

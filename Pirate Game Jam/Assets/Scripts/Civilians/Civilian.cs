using UnityEngine;

public class Civilian : MonoBehaviour, IHealth
{
    public float Health { get; set; }
    public float MaxHealth { get; set; } = 50;

    protected Rigidbody2D rb;

    [SerializeField] protected float waitTime = 1;
    protected float waitTimer;

    protected Vector2 dir;

    protected virtual void Awake() => rb = GetComponent<Rigidbody2D>();

    void Start()
    {
        GameManager.Instance.Civilians.Add(this);
        GetNewDirection();
    }

    void OnDisable() => GameManager.Instance.Civilians.Remove(this);

    void Update()
    {
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
        Debug.Log(gameObject.name + " Health: " + Health);
        if(Health <= 0)
            Die();
    }

    //TODO
    public void Die()
    {
        
    }
}

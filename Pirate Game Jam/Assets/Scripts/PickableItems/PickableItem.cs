using UnityEngine;

public abstract class PickableItem : MonoBehaviour
{
    
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D coll;

    float throwTime = .3f;
    float throwTimer;
    Vector2 throwDir;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        if(throwTimer > 0)
        {
            throwTimer-= Time.fixedDeltaTime;
            Vector2 throwForce = throwDir * 5;
            Vector2 throwDiff = throwForce * throwTimer / throwTime;
            float accelRate = 10;
            rb.AddForce(throwDiff * accelRate);
        }
    }

    public void ThrowItem(Vector2 dir)
    {
        EnableItem();
        throwDir = dir;
        throwTimer = throwTime;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent(out IItemPicker picker))
        {
            picker.PickItem(this);
            DisableItem();
            if (picker is PlayerMovement)
                ObjectivesManager.Instance.pickUpItem();
        }
    }

    public abstract void Effect(IItemPicker target);

    public void DisableItem()
    {
        sr.enabled = false;
        coll.enabled = false;
    }

    protected void EnableItem()
    {
        sr.enabled = true;
        coll.enabled = true;
    }
}

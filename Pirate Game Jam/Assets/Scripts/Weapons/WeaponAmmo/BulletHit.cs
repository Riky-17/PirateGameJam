using UnityEngine;

public class BulletHit : MonoBehaviour
{
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("TriggerDestroy");
    }
}

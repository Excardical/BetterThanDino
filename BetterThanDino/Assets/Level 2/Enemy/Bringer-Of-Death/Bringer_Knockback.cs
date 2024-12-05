using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer_Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private Bringer_Movement bringer_Movement; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bringer_Movement = GetComponent<Bringer_Movement>(); 
    }

    public void Knockback(Transform playerTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        bringer_Movement.ChangeState(BringerState.Knockback); 
        StartCoroutine(StunTimer(knockbackTime, stunTime));
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.velocity = direction * knockbackForce;
    }

    IEnumerator StunTimer(float knockbackTime, float stunTime)
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        bringer_Movement.ChangeState(BringerState.Idle);
    }
}
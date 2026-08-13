using System.Security.Cryptography;
using UnityEngine;
public class Enemy_Combat : MonoBehaviour
{
    public int damage = 5;
    public Transform attackPoint;
    public float weaponRange;
    public LayerMask playerLayer;
    public float KnockbackForce;
    public float stunTime;

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        if(hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
            hits[0].GetComponent<PlayerMovement>().Knockback(transform, KnockbackForce, stunTime);
        }
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    private int health{get; set;}
    
    void Awake()
    {
        health = GetMaxHealth();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    
    void TakeDamage(int damage)
    {
        health = Mathf.Max(0, health - damage);

        if (health == 0)
        {
            Die();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Laser"))
        {
            Laser laser = other.GetComponent<Laser>();
            if (laser.isFromPlayer)
            {
                TakeDamage(laser.GetDamage());
                Destroy(other.gameObject);
            }
        }
    }
    public abstract int GetMaxHealth();
    public abstract int GetCoins();
}

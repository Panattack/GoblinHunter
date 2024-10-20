using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, Hittable
{
    [SerializeField] private float health = 100;
    [SerializeField] private bool boss = false;

    public float Health { get => health; set => Health = health; }

    [SerializeField] public float damage = 10f;

    private void Start()
    {
        damage = 10f;
    }

    public void InitEnemy()
    {
        health = 200;
        damage = 3;
    }

    public void TakeDamage(float amount)
    {
        if (health > amount)
        {
            health -= amount;
            PlayerProfile.EnemyDamaged(this.gameObject.name,health);
        }
        else
        {
            if (boss)
            {
                EventManager.BossDefeated();
            }
            EventManager.EnemyDeath(this.gameObject);
            PlayerProfile.EnemyKilled(this.gameObject.name);
            PlayerProfile.IncrementKills();
        }
    }


}

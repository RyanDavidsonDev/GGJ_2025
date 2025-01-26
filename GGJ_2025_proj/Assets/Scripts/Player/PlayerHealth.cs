using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] public float health;
    public void Die()
    {
        //GameManager.Instance.CurrentGameState = GameManager.GameState.GameOver;
        GameManager.Instance.LoseGame();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }
}

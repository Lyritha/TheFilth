using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public void InatiateProjectile(float pLifetime)
    {
        StartCoroutine(ProjectileLifeTime(pLifetime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        List<string> ignoreTrigger = new() { "Player", "PlayerProjectile", "EnemyProjectile" };

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyTakeDamage enemyTakeDamage = collision.gameObject.GetComponent<EnemyTakeDamage>();
            enemyTakeDamage.TakeDamage();
            Destroy(gameObject);
        }
        else if (!ignoreTrigger.Contains(collision.gameObject.tag))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ProjectileLifeTime(float pLifetime)
    {
        yield return new WaitForSeconds(pLifetime);

        Destroy(gameObject);
    }
}

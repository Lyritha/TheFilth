using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float projectileLifeTime = 3f;

    private void Start()
    {
        StartCoroutine(DestroyDelay());
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(projectileLifeTime);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        List<string> ignoreTrigger = new() { "Enemy", "PlayerProjectile", "EnemyProjectile" };

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
            Destroy(gameObject);
        }
        else if (!ignoreTrigger.Contains(collision.gameObject.tag))
        {
            Destroy(gameObject);
        }
    }
}

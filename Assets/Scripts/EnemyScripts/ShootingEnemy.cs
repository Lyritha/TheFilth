using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float rotSpeed = 1;
    [SerializeField] float bulletSpeed = 10;
    [SerializeField] float minShootingTime = 2.0f;
    [SerializeField] float maxShootingTime = 6.0f;

    float shootingTime = 0;

    GameObject player;
    [SerializeField] GameObject bulletPosition = null;
    [SerializeField] GameObject audio_Shoot = null;


    private void Start()
    {
        shootingTime = Random.Range(minShootingTime, maxShootingTime);

        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(ShootingLoop());
    }

    private void Update()
    {
        RotateGunToPlayer();
    }

    IEnumerator ShootingLoop()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));

        while (true)
        {
            ShootBullet();

            yield return new WaitForSeconds(shootingTime);

            yield return null;
        }
    }

    void RotateGunToPlayer()
    {
        Vector3 dir = player.transform.position - transform.position;
        float angleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, angleZ - 90);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    void ShootBullet()
    {
        GameObject audio = Instantiate(audio_Shoot);
        DontDestroyOnLoad(audio);

        GameObject bulletClone = Instantiate(bullet, bulletPosition.transform.position, transform.rotation);
        bulletClone.GetComponent<Rigidbody2D>().AddRelativeForce(new(0,bulletSpeed), ForceMode2D.Impulse);
    }
}
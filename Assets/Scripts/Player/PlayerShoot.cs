using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    [Header("Main shooting")]
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float projectileLifetime = 2f;
    [SerializeField] float bulletDelay = 0.1f;

    [Header("Special Ability")]
    [SerializeField] float specialProjectileSpeed = 2f;
    [SerializeField] int MaxSpecialAmount = 3;
    [SerializeField] int SpawnSpecialAmount = 1;

    [Header("Utility")]
    [SerializeField] Transform shootLocation = null;
    [SerializeField] GameObject projectile = null;
    [SerializeField] GameObject specialProjectile = null;
    [SerializeField] TMP_Text specialText = null;
    [SerializeField] GameObject audio_Shoot = null;

    private bool isShooting = false;

    GameObject specialProjectileInstance = null;
    int currentSpecialAmount = 3;
    bool specialTriggered = false;

    Coroutine shootCoroutine = null;

    float spamAvoidenceTimer;

    private void Start()
    {
        currentSpecialAmount = SpawnSpecialAmount;
        specialText.text = $"Special: {currentSpecialAmount}";

        spamAvoidenceTimer = bulletDelay;
    }

    // Update is called once per frame
    void Update()
    {
        spamAvoidenceTimer += Time.deltaTime;

        //check if player is shooting
        isShooting = Input.GetMouseButton(0);
        NormalShooting();

        if (Input.GetMouseButtonDown(1)) SpecialShooting();
    }

    private void NormalShooting()
    {
        //if shooting and coroutine hasn't started yet start shooting, else stop coroutine
        if (isShooting && shootCoroutine == null && spamAvoidenceTimer >= bulletDelay)
        {
            shootCoroutine = StartCoroutine(ShootLoop());
        }
        else if (!isShooting && shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }

    private void SpecialShooting()
    {
        if (specialProjectileInstance == null && currentSpecialAmount != 0)
        {
            currentSpecialAmount--;
            specialText.text = $"Special: {currentSpecialAmount}";

            specialTriggered = false;

            GameObject audio = Instantiate(audio_Shoot);
            DontDestroyOnLoad(audio);

            //instance a projectile
            specialProjectileInstance = Instantiate(specialProjectile, shootLocation.position, transform.rotation);

            //only occurs once per projectile, so should be fine to run outside FixedUpdate without Time.DeltaTime
            specialProjectileInstance.GetComponent<Rigidbody2D>().AddRelativeForce(new(0, specialProjectileSpeed), ForceMode2D.Impulse);
        }
        else if (!specialTriggered)
        {
            specialProjectileInstance.GetComponent<PlayerProjectile_Special>().TriggerMagnet();
            specialTriggered = true;
        }
    }

    IEnumerator ShootLoop()
    {
        //while the player is shooting keep looping
        while (isShooting)
        {
            GameObject audio = Instantiate(audio_Shoot);
            DontDestroyOnLoad(audio);

            Shoot();
            spamAvoidenceTimer = 0;

            //wait for a certain time before starting while loop again
            yield return new WaitForSeconds(bulletDelay);

            yield return null;
        }

        yield return null;
    }

    void Shoot()
    {
        //instance a projectile
        GameObject projectileInstance = Instantiate(projectile, shootLocation.position, transform.rotation);
        projectileInstance.GetComponent<PlayerProjectile>().InatiateProjectile(projectileLifetime);

        //only occurs once per projectile, so should be fine to run outside FixedUpdate without Time.DeltaTime
        projectileInstance.GetComponent<Rigidbody2D>().AddRelativeForce(new(0,projectileSpeed),ForceMode2D.Impulse);
    }

    public void AddSpecial()
    {
        if (currentSpecialAmount < MaxSpecialAmount) currentSpecialAmount++;
        specialText.text = $"Special: {currentSpecialAmount}";
    }
}

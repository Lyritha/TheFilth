using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerProjectile_Special : MonoBehaviour
{
    [SerializeField] private GameObject magnet = null;
    private Rigidbody2D rb = null;

    [SerializeField] GameObject spriteEmpty = null;
    [SerializeField] GameObject spriteFull = null;

    [SerializeField] GameObject magnetParticles = null;
    [SerializeField] GameObject DeathParticles = null;

    [SerializeField] GameObject audio_Magnet = null;
    [SerializeField] GameObject audio_Explode = null;

    bool isMagnetOn = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TriggerMagnet()
    {
        StartCoroutine(Magnet());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isMagnetOn)
        {
            collision.gameObject.GetComponent<EnemyTakeDamage>().TakeDamage();
        }
    }

    //the "animation" of the projectile
    IEnumerator Magnet()
    {
        isMagnetOn = true;

        GameObject audio = Instantiate(audio_Magnet);
        DontDestroyOnLoad(audio);

        rb.drag = 2f;
        rb.mass = 100f;
        magnet.SetActive(true);

        spriteEmpty.SetActive(false);
        spriteFull.SetActive(true);

        GameObject instance = Instantiate(magnetParticles, rb.position, Quaternion.identity);
        instance.transform.parent = transform;

        yield return new WaitForSeconds(2f);

        magnet.GetComponent<PointEffector2D>().forceMagnitude = 150;

        GameObject audio1 = Instantiate(audio_Explode);
        DontDestroyOnLoad(audio1);

        yield return ShrinkObject();


        Instantiate(DeathParticles, rb.position, Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);

        yield return null;
    }

    private IEnumerator ShrinkObject()
    {
        float timer = 0;

        Vector3 initialScale = gameObject.transform.localScale;
        Vector3 targetScale = new(0, 0, 1);

        while (timer <= 0.1f)
        {
            float t = timer / 0.1f;
            gameObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            timer += Time.deltaTime;

            yield return null;
        }

        yield return null;
    }
}

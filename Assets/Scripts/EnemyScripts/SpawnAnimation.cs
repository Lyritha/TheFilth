using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimation : MonoBehaviour
{
    [SerializeField] float duration = 0.15f;
    [SerializeField] Vector2 offset = new(0, 0.1f);

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new(0, 0, 1);
        StartCoroutine(ScaleGameObjectUp());
    }

    IEnumerator ScaleGameObjectUp()
    {
        yield return new WaitForSeconds(Random.Range(offset.x, offset.y));

        float timer = 0;

        Vector3 initialScale = gameObject.transform.localScale;
        Vector3 targetScale = new(1,1,1);

        while (timer <= duration)
        {
            float t = timer / duration;
            gameObject.transform.localScale = EaseInOutLerp(initialScale, targetScale, t);
            timer += Time.deltaTime;

            yield return null;
        }

        gameObject.transform.localScale = targetScale;
        yield return null;
    }

    Vector3 EaseInOutLerp(Vector3 start, Vector3 end, float t)
    {
        t = t < 0.5f ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2; // Smooth interpolation
        return Vector3.Lerp(start, end, t);
    }
}

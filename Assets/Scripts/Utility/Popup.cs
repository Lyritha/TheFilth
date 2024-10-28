using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] GameObject popup = null;
    [SerializeField] Button[] buttons = null;

    [SerializeField] float enableSpeed = 0.1f;
    [SerializeField] float disableSpeed = 0.1f;

    public void EnablePopup(float speed = 0)
    {
        popup.transform.localScale = Vector3.zero;  // Start scale from zero
        popup.SetActive(true);

        speed = speed == 0 ? enableSpeed : speed;

        StartCoroutine(EnableAnim(speed));
    }

    public void DisablePopup(float speed = 0)
    {
        speed = speed == 0 ? disableSpeed : speed;
        StartCoroutine(DisableAnim(speed));
    }

    IEnumerator EnableAnim(float speed)
    {
        yield return StartCoroutine(Animation(speed, Vector3.one));

        if (buttons.Length != 0)
        {
            foreach (Button button in buttons)
            {
                button.interactable = true;
            }
        }

        yield return null;
    }

    IEnumerator DisableAnim(float speed)
    {
        if (buttons.Length != 0)
        {
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }

        yield return StartCoroutine(Animation(speed, Vector3.zero));
        popup.SetActive(false);
    }

    IEnumerator Animation(float duration, Vector3 targetScale)
    {
        Vector3 initialScale = popup.transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration); ; 
            yield return popup.transform.localScale = EaseInOutLerp(initialScale, targetScale, t);

            yield return null;
        }

        // Ensure final scale is exactly the targetScale
        popup.transform.localScale = targetScale;
    }

    Vector3 EaseInOutLerp(Vector3 start, Vector3 end, float t)
    {
        t = t < 0.5f ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2; // Smooth interpolation
        return Vector3.Lerp(start, end, t);
    }

    public float EnableSpeed { get { return enableSpeed; } }
    public float DisableSpeed { get { return disableSpeed; } }
}
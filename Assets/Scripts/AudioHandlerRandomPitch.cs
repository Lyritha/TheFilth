using System.Collections;
using UnityEngine;

public class AudioHandlerRandomPitch : MonoBehaviour
{
    [SerializeField] float minPitch = 0.9f;
    [SerializeField] float maxPitch = 1.1f;

    AudioSource audioSource = null;
    float audioLength = 0;


    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioLength = audioSource.clip.length;

        StartCoroutine(KillAfterPlay());
    }

    IEnumerator KillAfterPlay()
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();

        yield return new WaitForSecondsRealtime(audioLength);

        Destroy(gameObject);

        yield return null;
    }

    public float AudioLength { get { return audioLength; } }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFilthLook : MonoBehaviour
{
    [SerializeField] GameObject eye = null;
    [SerializeField] float maxDistance = 0.5f;
    [SerializeField] Vector2 TargetSwitchDelay = Vector2.zero;
    [SerializeField] float lookSpeed = 5f;

    Transform playerT = null;
    float minX, maxX, minY, maxY = 0;
    bool isFollowingPlayer = false;
    Vector2 target = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;

        Vector2 a = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        minX = a.x;
        minY = a.y;

        Vector2 b = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        maxX = b.x;
        maxY = b.y;

        target = transform.position;
        StartCoroutine(ChooseTarget());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = target - (Vector2)transform.position;
        float distance = dir.magnitude;

        dir.Normalize();
        dir *= maxDistance;

        if (isFollowingPlayer && distance > maxDistance)
        {
            target = playerT.position;
            eye.transform.position = Vector2.Lerp(eye.transform.position, (Vector2)transform.position + dir, Time.deltaTime * lookSpeed);
        }
        else
        {
            eye.transform.position = Vector2.Lerp(eye.transform.position, (Vector2)transform.position + dir, Time.deltaTime * lookSpeed);
        }

    }

    IEnumerator ChooseTarget()
    {
        yield return new WaitForSecondsRealtime(10f);

        while (true)
        {
            if (isFollowingPlayer) yield return new WaitForSecondsRealtime(Random.Range(TargetSwitchDelay.x  + 5, TargetSwitchDelay.y + 5));
            else yield return new WaitForSecondsRealtime(Random.Range(TargetSwitchDelay.x, TargetSwitchDelay.y));

            switch (Random.Range(0, 2))
            {
                case 0:
                    isFollowingPlayer = true;
                    break;
                case 1:
                    isFollowingPlayer = false;
                    target = ChooseRandomLookTarget();
                    break;
                default:
                    target = Vector2.zero;
                    break;
            }
        }
    }

    Vector2 ChooseRandomLookTarget()
    {
        Vector2 target = Vector2.zero;

        target.x = Random.Range(minX, maxX);
        target.y = Random.Range(minY, maxY);

        return target;
    }


}

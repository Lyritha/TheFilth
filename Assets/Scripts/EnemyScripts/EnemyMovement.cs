using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float maximumStepSize = 1f;
    [SerializeField] Vector2 randomUpdateRate = new(2,5);
    [SerializeField] float movementSpeed = 5f;
    
    float minX, maxX, minY, maxY = 0;
    Vector2 targetPos = Vector2.zero;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        targetPos = rb.position;

        Vector2 a= (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        minX = a.x;
        minY = a.y;

        Vector2 b = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        maxX = b.x;
        maxY = b.y;

        StartCoroutine(PathFind());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 forceDirection = (targetPos - rb.position).normalized;
        float distanceClamped = Mathf.Clamp01(Vector2.Distance(rb.position, targetPos));

        rb.AddForce(forceDirection * (movementSpeed * distanceClamped));
    }

    IEnumerator PathFind()
    {
        yield return new WaitForSeconds(Random.Range(randomUpdateRate.x, randomUpdateRate.y));

        while (true)
        {
            
            bool validPos = false;
            while (!validPos)
            {
                targetPos.x = Random.Range(minX, maxX);
                targetPos.y = Random.Range(minY, maxY);

                if (Vector2.Distance(rb.position, targetPos) < maximumStepSize) validPos = true;
            }

            yield return new WaitForSeconds(Random.Range(randomUpdateRate.x, randomUpdateRate.y));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPos, .5f);
        Gizmos.DrawWireSphere(transform.position, maximumStepSize);
    }
}

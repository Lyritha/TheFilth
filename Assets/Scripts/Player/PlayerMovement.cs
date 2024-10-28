using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float minMouseDistance = 0.1f;
    [SerializeField] private float maxMouseDistance = 1f;
    [SerializeField] private float mouseSpeed = 5f;
    [SerializeField] private float maxMouseSpeed = 2f;

    [SerializeField] GameObject mouseTarget = null;

    private Vector3 mousePosition = Vector3.zero;
    private Vector3 targetMousePosition = Vector3.zero;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    private void Update()
    {
        //get the mouse position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //make sure the position is on the same plane as this object
        mousePosition.z = transform.position.z;

        //set the mouse targets position
        mouseTarget.transform.position = mousePosition;
    }

    private void FixedUpdate()
    {
        //the objects current position
        Vector3 objectPosition = rb.position;
        
        //get direction of the mouse, non-normalized
        Vector3 direction = mousePosition - objectPosition;

        //get distance
        float distance = direction.magnitude;

        //move player if outside zone, otherwise stay static
        MovePlayer(objectPosition, direction, distance);

        //rotate player to aim at actual mouse position
        if (distance > minMouseDistance)
        rb.MoveRotation(Quaternion.FromToRotation(Vector3.up, direction.normalized));
    }

    private void MovePlayer(Vector3 pObjectPosition, Vector3 pDirection, float pDistance)
    {

        //get how far the object is outside the distance
        float DistanceOutsideZone = pDistance - maxMouseDistance;

        //if the object is outside the distance, update the target position
        if (pDistance > maxMouseDistance)
        {
            Vector3 clampedSpeed = pDirection.normalized * DistanceOutsideZone;
            clampedSpeed.x = Mathf.Clamp(clampedSpeed.x, -maxMouseSpeed, maxMouseSpeed);
            clampedSpeed.y = Mathf.Clamp(clampedSpeed.y, -maxMouseSpeed, maxMouseSpeed);

            //the target mouse position is the current position of the object with the vector added to make it bound to the maxMouseDistance
            targetMousePosition = pObjectPosition + clampedSpeed;
        }

        //transform the object
        rb.MovePosition(Vector3.Lerp(rb.position, targetMousePosition, mouseSpeed));
    }
}

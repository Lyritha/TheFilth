using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoundingBox : MonoBehaviour
{
    [SerializeField, Tooltip("used to offset collider for nav bar")] float bottomOffset = 0f;
    [SerializeField, Tooltip("used to offset collider for nav bar")] float topOffset = 0f;
    [SerializeField, Tooltip("used to offset collider for nav bar")] float leftOffset = 0f;
    [SerializeField, Tooltip("used to offset collider for nav bar")] float rightOffset = 0f;
    [SerializeField] private float edgeThickness = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBorders();
    }

    private void GenerateBorders()
    {
        //grab main camera
        Camera cam = Camera.main;

        //grab the corners of the screen, 1 aditional to close the bottom
        Vector2[] corners = new Vector2[5];

        Vector2 offsetLowerLeft = new(-edgeThickness + leftOffset, -edgeThickness + bottomOffset);
        Vector2 offsetUpperLeft = new(-edgeThickness + leftOffset, edgeThickness + topOffset);
        Vector2 offsetUpperRight = new(edgeThickness + rightOffset, edgeThickness + topOffset);
        Vector2 offsetLowerRight = new(edgeThickness + rightOffset, -edgeThickness + bottomOffset);

        corners[0] = (Vector2)cam.ViewportToWorldPoint(new Vector2(0, 0)) + offsetLowerLeft;
        corners[1] = (Vector2)cam.ViewportToWorldPoint(new Vector2(0, 1)) + offsetUpperLeft;
        corners[2] = (Vector2)cam.ViewportToWorldPoint(new Vector2(1, 1)) + offsetUpperRight;
        corners[3] = (Vector2)cam.ViewportToWorldPoint(new Vector2(1, 0)) + offsetLowerRight;

        corners[4] = corners[0];

        //create edge collider if doesn't exist, if exist then update values
        EdgeCollider2D edgeCollider = null;
        if (gameObject.GetComponent<EdgeCollider2D>() != null) edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
        else edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        edgeCollider.edgeRadius = edgeThickness;
        edgeCollider.points = corners;
    }
}

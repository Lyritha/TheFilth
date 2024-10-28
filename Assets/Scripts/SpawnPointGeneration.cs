using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnPointGeneration : MonoBehaviour
{
    [SerializeField] float bottomDeadZone = 0f;

    [SerializeField] private int bottomXDeadZone = 6;
    [SerializeField] private int bottomYDeadZone = 3;
    [SerializeField] private int topXDeadZone = 2;
    [SerializeField] private int topYDeadZone = 4;

    private Vector2[] validSpawnPoints = null;

    // Start is called before the first frame update
    void Awake()
    {
        UpdateValidSpawnPoints();
    }

    private void Update()
    {
        UpdateValidSpawnPoints();
    }

    private void UpdateValidSpawnPoints()
    {
        //get the world position of the bottom left of the screen
        Vector2 cameraPosBottomLeft = Camera.main.ViewportToWorldPoint(Vector2.zero);

        //get the world position of the top right of the screen
        Vector2 cameraPosTopRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector2 currentPosition = transform.position;

        List<Vector2> spawnPoints = new();

        //generate top left grid
        for (int offsetX = -topXDeadZone; offsetX > cameraPosBottomLeft.x + 0.25f; offsetX--)
        {
            for (int offsetY = topYDeadZone; offsetY < cameraPosTopRight.y - 0.25f; offsetY++)
            {
                spawnPoints.Add(currentPosition + new Vector2(offsetX, offsetY));
            }
        }

        //generate top right grid
        for (int offsetX = topXDeadZone; offsetX < cameraPosTopRight.x - 0.25f; offsetX++)
        {
            for (int offsetY = topYDeadZone; offsetY < cameraPosTopRight.y - 0.25f; offsetY++)
            {
                spawnPoints.Add(currentPosition + new Vector2(offsetX, offsetY));
            }
        }

        //generate bottom left grid
        for (int offsetX = -bottomXDeadZone; offsetX > cameraPosBottomLeft.x + 0.25f; offsetX--)
        {
            for (int offsetY = bottomYDeadZone; offsetY > cameraPosBottomLeft.y + 0.25f + bottomDeadZone; offsetY--)
            {
                spawnPoints.Add(currentPosition + new Vector2(offsetX, offsetY)); ;
            }
        }

        //generate bottom right grid
        for (int offsetX = bottomXDeadZone; offsetX < cameraPosTopRight.x - 0.25f; offsetX++)
        {
            for (int offsetY = bottomYDeadZone; offsetY > cameraPosBottomLeft.y + 0.25f + bottomDeadZone; offsetY--)
            {
                spawnPoints.Add(currentPosition + new Vector2(offsetX, offsetY));
            }
        }

        validSpawnPoints = spawnPoints.ToArray();
    }

    private void OnDrawGizmos()
    {
        if (validSpawnPoints != null)
        {
            foreach(Vector2 point in validSpawnPoints)
            {
                Gizmos.DrawWireCube(point, new(0.5f,0.5f));
            }
        }
    }

    public Vector2[] ValidSpawnPositions { get { return validSpawnPoints; } }
}

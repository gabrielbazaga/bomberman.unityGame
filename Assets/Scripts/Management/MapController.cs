using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 12;

    [SerializeField] private int innerWallSpacing = 2;

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject boxPrefab;

    [SerializeField] private List<Vector2> playerSpawnPositions;

    public List<Vector2> AllWallPositions { get; private set; }

    private const float TILE_OFFSET = 0.5f;

    private void Awake()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        AllWallPositions = new List<Vector2>();

        GenerateBoundaryWalls();
        GenerateInnerMatrixWalls();
        SpawnBoxesInValidLocations();
    }

    private void GenerateBoundaryWalls()
    {
        for (int x = 0; x <= width; x++)
        {
            SpawnAndRegisterWall(new Vector2(x - TILE_OFFSET, -TILE_OFFSET));
            SpawnAndRegisterWall(new Vector2(x - TILE_OFFSET, height - TILE_OFFSET));
        }

        for (int y = 1; y < height; y++)
        {
            SpawnAndRegisterWall(new Vector2(-TILE_OFFSET, y - TILE_OFFSET));
            SpawnAndRegisterWall(new Vector2(width - TILE_OFFSET, y - TILE_OFFSET));
        }
    }

    private void GenerateInnerMatrixWalls()
    {
        for (int y = innerWallSpacing; y < height; y += innerWallSpacing)
        {
            for (int x = innerWallSpacing; x < width; x += innerWallSpacing)
            {
                SpawnAndRegisterWall(new Vector2(x - TILE_OFFSET, y - TILE_OFFSET));
            }
        }
    }

    private void SpawnBoxesInValidLocations()
    {
        var invalidBoxPositions = new HashSet<Vector2>(AllWallPositions);
        foreach (var spawnPos in playerSpawnPositions)
        {
            invalidBoxPositions.Add(spawnPos);
        }

        for (float y = TILE_OFFSET; y < height; y++)
        {
            for (float x = TILE_OFFSET; x < width; x++)
            {
                var potentialPosition = new Vector2(x, y);
                
                if (!invalidBoxPositions.Contains(potentialPosition))
                {
                    Instantiate(boxPrefab, potentialPosition, Quaternion.identity, transform);
                }
            }
        }
    }

    private void SpawnAndRegisterWall(Vector2 position)
    {
        Instantiate(wallPrefab, position, Quaternion.identity, transform);
        AllWallPositions.Add(position);
    }
}
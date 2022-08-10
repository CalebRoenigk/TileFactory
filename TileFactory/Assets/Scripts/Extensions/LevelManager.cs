using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : Monobehavior
{
    [SerializeField] private Tilemap foreground;
    [SerializeField] private Tilemap background;
    [SerializeField] private int levelIndex;
    
    // Save a level map
    public void SaveMap()
    {
        // Create a new level data object
        LevelData newLevel = ScriptableObject.CreateInstance<LevelData>();

        // Set the general props
        newLevel.levelIndex = levelIndex;
        newLevel.name = $"Level " + levelIndex;
        
        // Create the grid that will store the level data
        // Compress the bounds of both tilemaps
        foreground.CompressBounds();
        background.CompressBounds();
        
        // Determine the level bounds using the sizes of both
        BoundsInt foregroundBounds = foreground.cellBounds;
        BoundsInt backgroundBounds = background.cellBounds;

        Vector3Int minBounds = new Vector3Int((int)Mathf.Min(foregroundBounds.xMin, backgroundBounds.xMin), (int)Mathf.Min(foregroundBounds.yMin, backgroundBounds.yMin), (int)Mathf.Min(foregroundBounds.zMin, backgroundBounds.zMin));
        Vector3Int maxBounds = new Vector3Int((int)Mathf.Min(foregroundBounds.xMax, backgroundBounds.xMax), (int)Mathf.Min(foregroundBounds.yMax, backgroundBounds.yMax), (int)Mathf.Min(foregroundBounds.zMax, backgroundBounds.zMax));
        
        // Create the grid bounds
        BoundsInt gridBounds = new BoundsInt();
        gridBounds.SetMinMax(minBounds, maxBounds);
        
        // Create the grid
        Grid levelGrid = public Grid(new Vector2Int(gridBounds.size.z, gridBounds.size.y));

        // Iterate over the foreground and background to get the level tiles and store them in the grid
        Vector3Int foregroundOffset = -foregroundBounds.min;
        foreach(Vector3Int cell in foregroundBounds.allPositionsWithin)
        {
            if (foreground.HasTile(cell))
            {
                Vector2Int gridPosition = foregroundOffset + cell;
                Entity tileEntity = new Entity();
            
                // TODO: Rename Terrain - SIdes as Terrain - Sides
            
                // Cast the entity as a type based on the tile name
                switch(foreground.GetTile(cell).name)
                {
                    case "Terrain - Sides":
                        tileEntity = new Terrain(0);
                        break;
                    case "Terrain - Top":
                        tileEntity = new Terrain(1);
                        break;
                    case "Terrain - Solo Top":
                        tileEntity = new Terrain(2);
                        break;
                    case "Water":
                        tileEntity = new Water();
                        break;
                    case "Support - Foreground":
                        tileEntity = new Support();
                        break;
                    default:
                        break;
                }
            
                levelGrid.PlaceEntity(tileEntity, (Vector2Int)gridPosition, 0);
            }
        }
            
        Vector3Int backgroundOffset = -backgroundBounds.min;
        foreach(Vector3Int cell in backgroundBounds.allPositionsWithin)
        {
            if (background.HasTile(cell))
            {
                Vector2Int gridPosition = backgroundOffset + cell;
                Entity tileEntity = new Entity();

                // Cast the entity as a type based on the tile name
                switch(background.GetTile(cell).name)
                {
                    case "Terrain - Sides":
                        tileEntity = new Terrain(0);
                        break;
                    case "Terrain - Top":
                        tileEntity = new Terrain(1);
                        break;
                    case "Terrain - Solo Top":
                        tileEntity = new Terrain(2);
                        break;
                    case "Water":
                        tileEntity = new Water();
                        break;
                    case "Support - Background":
                        tileEntity = new Support();
                        break;
                    default:
                        break;
                }
            
                levelGrid.PlaceEntity(tileEntity, (Vector2Int)gridPosition, 1);
            }
        }
            
        // Store the level grid
        newLevel.grid = levelGrid;
        
        // Save the level
        ScriptableObjectUtility.SaveLevelFile(newLevel);
}

    public void ClearMap()
    {
        
    }

    public void LoadMap()
    {
        
    }
}

#if UNITY_EDITOR

public static class ScriptableObjectUtility
{
    public static void SaveLevelFile(LevelData level)
    {
        // Create the asset in resources
        AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{level.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

#endif
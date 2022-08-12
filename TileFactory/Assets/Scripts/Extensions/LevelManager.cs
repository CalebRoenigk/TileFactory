using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using Factory;
using Factory.Objects;
using Factory.Data;
using Grid = Factory.Grid;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Tilemap foreground;
    [SerializeField] private Tilemap background;
    [SerializeField] private Tilemap entity;
    [SerializeField] private FactoryHandler factoryHandler;
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
        // foreground.CompressBounds();
        // background.CompressBounds();
        // entity.CompressBounds();
        
        // Determine the level bounds using the sizes of both
        BoundsInt foregroundBounds = foreground.cellBounds;
        BoundsInt backgroundBounds = background.cellBounds;
        BoundsInt entityBounds = entity.cellBounds;
    
        Vector3Int minBounds = new Vector3Int((int)Mathf.Min(foregroundBounds.xMin, backgroundBounds.xMin, entityBounds.xMin), (int)Mathf.Min(foregroundBounds.yMin, backgroundBounds.yMin, entityBounds.yMin), (int)Mathf.Min(foregroundBounds.zMin, backgroundBounds.zMin));
        Vector3Int maxBounds = new Vector3Int((int)Mathf.Max(foregroundBounds.xMax, backgroundBounds.xMax, entityBounds.xMax), (int)Mathf.Max(foregroundBounds.yMax, backgroundBounds.yMax, entityBounds.yMax), (int)Mathf.Max(foregroundBounds.zMax, backgroundBounds.zMax));

        // Create the grid bounds
        BoundsInt gridBounds = new BoundsInt();
        gridBounds.SetMinMax(minBounds, maxBounds);

        // Create the grid
        Grid levelGrid = new Grid(new Vector2Int(gridBounds.size.x, gridBounds.size.y));
        
        // Get the base offset of the tiles
        Vector3Int tileOffset = -gridBounds.min;

        // Iterate over the grid foreground entities
        // Entity[,] foregroundEntities = new Entity[gridBounds.size.x, gridBounds.size.y];
        List<Entity> foregroundEntities = new List<Entity>();
        for (int x = 0; x < gridBounds.size.x; x++)
        {
            for (int y = 0; y < gridBounds.size.y; y++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                Vector3Int tilePosition = (Vector3Int)gridPosition + tileOffset;

                if (foreground.HasTile(tilePosition))
                {
                    // Cast the entity as a type based on the tile name
                    switch(foreground.GetTile(tilePosition).name)
                    {
                        case "Terrain - Sides":
                            // foregroundEntities[x, y] = new Factory.Objects.Terrain(levelGrid, gridPosition, 0);
                            foregroundEntities.Add(new Factory.Objects.Terrain(levelGrid, gridPosition, 0));
                            break;
                        case "Terrain - Top":
                            // foregroundEntities[x, y] = new Factory.Objects.Terrain(levelGrid, gridPosition, 1);
                            foregroundEntities.Add(new Factory.Objects.Terrain(levelGrid, gridPosition, 1));
                            break;
                        case "Terrain - Solo Top":
                            // foregroundEntities[x, y] = new Factory.Objects.Terrain(levelGrid, gridPosition, 2);
                            foregroundEntities.Add(new Factory.Objects.Terrain(levelGrid, gridPosition, 2));
                            break;
                        case "Water":
                            // foregroundEntities[x, y] = new Water(levelGrid, gridPosition);
                            foregroundEntities.Add(new Water(levelGrid, gridPosition));
                            break;
                        case "Support - Foreground":
                            // foregroundEntities[x, y] = new Support(levelGrid, gridPosition);
                            foregroundEntities.Add(new Support(levelGrid, gridPosition));
                            break;
                        case "Goal":
                            // foregroundEntities[x, y] = new Goal(levelGrid, gridPosition);
                            foregroundEntities.Add(new Goal(levelGrid, gridPosition));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    // foregroundEntities[x, y] = new Entity(levelGrid, gridPosition);
                }
            }
        }

        newLevel.foreground = foregroundEntities;
        // levelGrid.foreground = foregroundEntities;
        
        
        // Debug.Log(levelGrid.foreground[0,2]);

        // foreach(Vector3Int cell in backgroundBounds.allPositionsWithin)
        // {
        //     if (background.HasTile(cell))
        //     {
        //         Vector2Int gridPosition = (Vector2Int)(tileOffset + cell);
        //         Entity tileEntity = new Entity();
        //
        //         // Cast the entity as a type based on the tile name
        //         switch(background.GetTile(cell).name)
        //         {
        //             case "Terrain - Sides":
        //                 tileEntity = new Factory.Objects.Terrain(0);
        //                 break;
        //             case "Terrain - Top":
        //                 tileEntity = new Factory.Objects.Terrain(1);
        //                 break;
        //             case "Terrain - Solo Top":
        //                 tileEntity = new Factory.Objects.Terrain(2);
        //                 break;
        //             case "Water":
        //                 tileEntity = new Water();
        //                 break;
        //             case "Support - Background":
        //                 tileEntity = new Support();
        //                 break;
        //             case "Goal":
        //                 tileEntity = new Goal();
        //                 break;
        //             default:
        //                 break;
        //         }
        //     
        //         levelGrid.PlaceEntity(tileEntity, (Vector2Int)gridPosition, 1);
        //     }
        // }
        
        // // Get all the Cargo
        // foreach (Vector3Int cell in entityBounds.allPositionsWithin)
        // {
        //     if (entity.HasTile(cell))
        //     {
        //         levelGrid.cargo.Add(new Box(levelGrid, new Vector2Int(cell.x + tileOffset.x, cell.y + tileOffset.y)));
        //     }
        // }

        // Store the level grid
        newLevel.grid = levelGrid;

        // Save the level
        ScriptableObjectUtility.SaveLevelFile(newLevel);
        
        // Debug.Log(newLevel.grid.foreground);
        // Debug.Log(newLevel.grid.foreground[0,2]);
    }

    // Clear all tilemaps
    public void ClearMap()
    {
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
    
        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.ClearAllTiles();
        }
    }
    
    public void LoadMap()
    {
        LevelData level = Resources.Load<LevelData>($"Levels/Level {levelIndex}");
        if (level == null)
        {
            Debug.LogError($"Level {levelIndex} does not exist.");
            return;
        }
    
        // ClearMap();

        factoryHandler.SetLevel(level.grid);
        factoryHandler.grid.SetForeground(level.foreground);

        // // TODO: Make tiles be placed. Need to determine how to do resource dict for tiles to match their entities
        // for (int x = 0; x < level.grid.bounds.size.x; x++)
        // {
        //     for (int y = 0; y < level.grid.bounds.size.y; y++)
        //     {
        //         Entity entityForeground = level.grid.foreground[x, y];
        //         Entity entityBackground = level.grid.background[x, y];
        //         Vector3Int position = new Vector3Int(x, y, 0);
        //         TileBase foregroundTile = new Tile();
        //         TileBase backgroundTile = new Tile();
        //         bool tileInForeground = true;
        //         bool tileInBackground = true;
        //
        //         switch (entityForeground.GetType().ToString())
        //         {
        //             case "Terrain":
        //                 // TODO: SUPPORT TERRAIN TYPES
        //                 break;
        //             case "Water":
        //                 break;
        //             case "Support":
        //                 break;
        //             case "Goal":
        //                 break;
        //             default:
        //                 tileInForeground = false;
        //                 break;
        //         }
        //         switch (entityBackground.GetType().ToString())
        //         {
        //             case "Terrain":
        //                 break;
        //             case "Water":
        //                 break;
        //             case "Support":
        //                 break;
        //             case "Goal":
        //                 break;
        //             default:
        //                 tileInBackground = false;
        //                 break;
        //         }
        //
        //         if (tileInForeground)
        //         {
        //             foreground.SetTile(position, foregroundTile);
        //         }
        //         if (tileInBackground)
        //         {
        //             background.SetTile(position, backgroundTile);
        //         }
        //     }
        // }
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
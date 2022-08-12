using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Factory.Objects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Factory
{
    [ExecuteInEditMode]
    public class FactoryHandler : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] public Grid grid;

        [Header("Tilemaps")]
        [SerializeField] private Tilemap foreground;
        [SerializeField] private Tilemap background;
        [SerializeField] private Tilemap entity;
        
        [Header("Prefab Data")]
        [SerializeField] private TileBase box;
        // TODO: Add some kind of lookup for different asset ruletiles for placement when building level

        [Header("Player")]
        [SerializeField] private Dictionary<Entity, int> inventory = new Dictionary<Entity, int>();
        [SerializeField] private int inventoryIndex = 0;

        private void Update()
        {
            LoadGrid();
        }

        private void FixedUpdate()
        {
            grid.Update();
            
            entity.ClearAllTiles();

            foreach (Entity cargo in grid.cargo)
            {
                entity.SetTile((Vector3Int)cargo.position, box);
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Draw the bounds
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(grid.bounds.center, grid.bounds.size);

            // Draw the entities in the foreground
            for (int x = 0; x < grid.bounds.size.x; x++)
            {
                for (int y = 0; y < grid.bounds.size.y; y++)
                {
                    Entity entity = grid.foreground[x, y];

                    switch (entity.GetType().ToString())
                    {
                        case "Factory.Objects.Terrain":
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireCube(new Vector3(entity.position.x, entity.position.y) + new Vector3(0.5f, 0.5f, 0f), Vector3.one);
                            break;
                        case "Factory.Objects.Water":
                            Gizmos.color = Color.blue;
                            Gizmos.DrawWireCube(new Vector3(entity.position.x, entity.position.y) + new Vector3(0.5f, 0.5f, 0f), Vector3.one);
                            break;
                        case "Factory.Objects.Support":
                            break;
                        case "Factory.Objects.Goal":
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawWireCube(new Vector3(entity.position.x, entity.position.y) + new Vector3(0.5f, 0.5f, 0f), Vector3.one);
                            break;
                        case "Factory.Objects.Conveyor":
                            Gizmos.color = Color.red;
                            Gizmos.DrawWireCube(new Vector3(entity.position.x, entity.position.y) + new Vector3(0.5f, 0.5f, 0f), Vector3.one);
                            break;
                        default:
                            break;
                    }
                }
            }

            foreach (Entity cargo in grid.cargo)
            {
                switch (cargo.GetType().ToString())
                {
                    case "Factory.Objects.Box":
                        Gizmos.color = new Color(0.731f, 0.637f, 0.516f);
                        Gizmos.DrawWireCube(new Vector3(cargo.position.x, cargo.position.y) + new Vector3(0.5f, 0.5f, 0f), Vector3.one);
                        break;
                    default:
                        break;
                }
            }
        }

        // private void Update()
        // {
        //     // Placement of tile
        //     if (inventory.ElementAt(inventoryIndex).Value > 0)
        //     {
        //         // Can place the selected inventory item
        //         if (Input.GetMouseButtonUp(0))
        //         {
        //             // User clicked left, attempt placement
        //             Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //             Vector2Int gridPosition = new Vector2Int((int)Mathf.Floor(worldPosition.x), (int)Mathf.Floor(worldPosition.y));
        //             AttemptTilePlacement(inventory.Keys.ToArray()[inventoryIndex], gridPosition);
        //         }
        //     }
        //     
        //     // Removal of tile
        //     if (Input.GetMouseButtonUp(1))
        //     {
        //         // User clicked right, attempt removal
        //         Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //         Vector2Int gridPosition = new Vector2Int((int)Mathf.Floor(worldPosition.x), (int)Mathf.Floor(worldPosition.y));
        //         AttemptTileRemoval(gridPosition);
        //     }
        // }
        
        // Attempt to place a tile from the inventory
        private void AttemptTilePlacement(Entity entity, Vector2Int position)
        {
            // Set the entity to removeable
            entity.isRemoveable = true;
            
            bool tilePlaced = grid.PlaceEntity(entity, position);
            if (tilePlaced)
            {
                // The tile was placed, remove 1 count from the item in the inventory
                inventory[entity]--;
            }
        }
        
        // Attempt to remove a tile from the grid and place it in inventory
        private void AttemptTileRemoval(Vector2Int position)
        {
            if (grid.EntityRemoveable(position))
            {
                // Get the entity from the grid
                Entity removeableEntity = grid.GetTileAtPosition(position);
                
                // Create a new instance of the object type for storage in the inventory
                System.Object inventoryEntity = Activator.CreateInstance(removeableEntity.GetType());
                
                // Remove the entity from the grid
                grid.RemoveEntity(removeableEntity);

                // Add the entity to the inventory
                AddEnityToInventory((Entity)inventoryEntity);
            }
        }
        
        // Adds an entity to the inventory
        private void AddEnityToInventory(Entity entity, int count = 1)
        {
            if (inventory.ContainsKey(entity))
            {
                inventory[entity] += count;
            }
            else
            {
                inventory.Add(entity, count);
            }
        }
        
        // Loads the level in
        public void SetLevel(Grid grid)
        {
            this.grid = grid;
        }
        
        // Loads the grid of the current level
        private void LoadGrid()
        {
            // Create the grid that will store the level data
            // Compress the bounds of both tilemaps
            foreground.CompressBounds();
            background.CompressBounds();
            entity.CompressBounds();
            
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
            grid = new Grid(new Vector2Int(gridBounds.size.x, gridBounds.size.y));
            
            // Get the base offset of the tiles
            Vector3Int tileOffset = gridBounds.min;

            // Iterate over the grid foreground entities
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
                                grid.foreground[x, y] = new Factory.Objects.Terrain(grid, gridPosition, 0);
                                break;
                            case "Terrain - Top":
                                grid.foreground[x, y] = new Factory.Objects.Terrain(grid, gridPosition, 1);
                                break;
                            case "Terrain - Solo Top":
                                grid.foreground[x, y] = new Factory.Objects.Terrain(grid, gridPosition, 2);
                                break;
                            case "Water":
                                grid.foreground[x, y] = new Water(grid, gridPosition);
                                break;
                            case "Support - Foreground":
                                grid.foreground[x, y] = new Support(grid, gridPosition);
                                break;
                            case "Goal":
                                grid.foreground[x, y] = new Goal(grid, gridPosition);
                                break;
                            case "Conveyor - Right":
                            case "Conveyor - Left":
                            case "Conveyor - Splitter":
                                grid.foreground[x, y] = new Conveyor(grid, gridPosition, Vector2Int.zero);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        grid.foreground[x, y] = new Entity(grid, gridPosition);
                    }
                }
            }
            
            // Get all the Cargo
            foreach (Vector3Int cell in entityBounds.allPositionsWithin)
            {
                if (entity.HasTile(cell))
                {
                    grid.cargo.Add(new Box(grid, new Vector2Int(cell.x + tileOffset.x, cell.y + tileOffset.y)));
                }
            }
        }
    }
}

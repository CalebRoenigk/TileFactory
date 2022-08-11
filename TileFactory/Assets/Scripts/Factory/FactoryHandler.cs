using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Factory.Objects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Factory
{
    public class FactoryHandler : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private Grid grid;

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

        private void Start()
        {
            grid = new Grid(new Vector2Int(12, 300));
            grid.AddEntity(new Vector2Int(8, 25));
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

        private void Update()
        {
            // Placement of tile
            if (inventory.ElementAt(inventoryIndex).Value > 0)
            {
                // Can place the selected inventory item
                if (Input.GetMouseButtonUp(0))
                {
                    // User clicked left, attempt placement
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2Int gridPosition = new Vector2Int((int)Mathf.Floor(worldPosition.x), (int)Mathf.Floor(worldPosition.y));
                    AttemptTilePlacement(inventory.Keys.ToArray()[inventoryIndex], gridPosition);
                }
            }
            
            // Removal of tile
            if (Input.GetMouseButtonUp(1))
            {
                // User clicked right, attempt removal
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPosition = new Vector2Int((int)Mathf.Floor(worldPosition.x), (int)Mathf.Floor(worldPosition.y));
                AttemptTileRemoval(gridPosition);
            }
        }
        
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
    }
}

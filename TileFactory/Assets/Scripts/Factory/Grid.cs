using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factory.Objects;

namespace Factory
{
    [System.Serializable]
    public class Grid
    {
        public BoundsInt bounds;

        // TODO: Maybe see about making the grid of level tiles into a single tile map with z position as well and using z position to place multiple tiles within 1 tile map at different depths as opposed to two or more layers
        public Entity[,] foreground;
        public Entity[,] background;

        public List<Entity> cargo = new List<Entity>();

        public List<Entity> onGoalEntities = new List<Entity>();

        public Grid()
        {
            
        }

        public Grid(Vector2Int size)
        {
            this.bounds = new BoundsInt();
            Vector3Int boundsMax = (Vector3Int)size;
            boundsMax.z = 1;
            this.bounds.SetMinMax((Vector3Int)Vector2Int.zero, boundsMax);

            this.foreground = new Entity[size.x, size.y];
            this.background = new Entity[size.x, size.y];

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    this.foreground[x, y] = new Entity(this, new Vector2Int(x, y));
                    this.background[x, y] = new Entity(this, new Vector2Int(x, y));
                }
            }
        }
        
        // Sets the foreground entities from a list
        public void SetForeground(List<Entity> foregroundEntities)
        {
            foreach (Entity entity in foregroundEntities)
            {
                if (CheckPosition(entity.position))
                {
                    PlaceEntity(entity, entity.position, 0);
                }
            }
        }

        // Returns tiles from the foreground
        public Entity GetTileAtPosition(Vector2Int position)
        {
            // Check if the position is out of bounds and if so return a solid tile with no movement
            if (!CheckPosition(position))
            {
                return new Entity(this, position, true, false);
            }
            
            return foreground[position.x, position.y];
        }
        
        // Checks if a position is open
        public bool CheckPosition(Vector2Int position)
        {
            // Out of bounds tiles
            if (!bounds.Contains(new Vector3Int(position.x, position.y, 0)))
            {
                return false;
            }
            
            // Solid Tiles
            if (!foreground[position.x, position.y].isSolid)
            {
                return true;
            }

            // Cargo Tiles
            if (cargo.FindIndex(c => c.position == position) != -1)
            {
                Entity cargoObject = cargo.Find(c => c.position == position);
                if (cargoObject.isSolid && cargoObject.willMove)
                {
                    return true;
                }
            }

            return false;
        }
        
        // Updates all the objects in the grid
        public void Update()
        {
            // Check preupdate if objects can move
            foreach (Entity cargoObject in cargo)
            {
                cargoObject.PreUpdate();
            }

            // Iterate and go bottom to top and right to left to update
            for (int y = bounds.size.y - 1; y > -1; y--)
            {
                for (int x = bounds.size.x - 1; x > -1; x--)
                {
                    this.foreground[x, y].Update();
                    this.background[x, y].Update();
                }
            }
            
            // Update each piece of cargo
            foreach (Entity cargoObject in cargo)
            {
                cargoObject.Update();
            }
            
            // Check each cargo and see if it is on a goal
            foreach (Entity cargoObject in cargo)
            {
                cargoObject.PostUpdate();
                
                // Update the status of the cargo in relation to the goal
                if (cargoObject.isOnGoal && !onGoalEntities.Contains(cargoObject))
                {
                    onGoalEntities.Add(cargoObject);
                }

                if (!cargoObject.isOnGoal && onGoalEntities.Contains(cargoObject))
                {
                    onGoalEntities.Remove(cargoObject);
                }
            }
        }
        
        // Adds a entity to the grid
        public void AddEntity(Vector2Int position)
        {
            if (CheckPosition(position))
            {
                cargo.Add(new Box(this, position));
            }
        }
        
        // Returns true if the position is a goal object OR if the position is an object on the goal
        public bool IsOnGoal(Vector2Int position)
        {
            Entity entity = GetTileAtPosition(position);
            
            return  entity.isOnGoal || entity.GetType() == typeof(Goal);
        }
        
        // Removes an entity from the grid, handles them differently depending on what type of entity they are
        public void RemoveEntity(Entity entity)
        {
            if (bounds.Contains(new Vector3Int(entity.position.x, entity.position.y, 0)))
            {
                Vector2Int position = entity.position;
                Entity replacementEntity = new Entity(this, position);
                
                // Tile based removal
                if (foreground[entity.position.x, entity.position.y].GetType() == entity.GetType())
                {
                    foreground[position.x, position.y] = replacementEntity;
                    return;
                }
                if (background[entity.position.x, entity.position.y].GetType() == entity.GetType())
                {
                    background[position.x, position.y] = replacementEntity;
                    return;
                }
            }

            if (cargo.Contains(entity))
            {
                // Cargo based removal
                // Check if the on goal entities contains this entity
                if (onGoalEntities.Contains(entity))
                {
                    onGoalEntities.Remove(entity);
                }
                
                // Remove the cargo from the cargo list
                cargo.Remove(entity);
                return;
            }
        }
        
        // Places a tile at a position if the position is empty
        public bool PlaceEntity(Entity entity, Vector2Int position, int layer = 0)
        {
            // Debug.Log("Position: " + position.ToString() + " placing " + entity.GetType().ToString());
            if (CheckPosition(position))
            {
                // Debug.Log("Can place!");
                // The position is empty, store the entity at the position
                entity.position = position;
                switch (layer)
                {
                    case 0:
                    default:
                        foreground[position.x, position.y] = entity;
                        return true;
                    case 1:
                        background[position.x, position.y] = entity;
                        return true;
                }
            }

            return false;
        }
        
        // Tests if the entity is removeable
        public bool EntityRemoveable(Vector2Int position)
        {
            Entity selectedTile = GetTileAtPosition(position);

            return selectedTile.isRemoveable;
        }
    }
}
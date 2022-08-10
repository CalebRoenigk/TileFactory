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
        public Vector2Int size;
        
        public Entity[,] foreground;
        public Entity[,] background;

        public List<Entity> cargo = new List<Entity>();

        public List<Entity> onGoalEntities = new List<Entity>();

        public Grid()
        {
            
        }

        public Grid(Vector2Int size)
        {
            this.size = size;
            
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

        // Returns tiles from the foreground
        public Entity GetTileAtPosition(Vector2Int position)
        {
            return foreground[position.x, position.y];
        }
        
        // Checks if a position is open
        public bool CheckPosition(Vector2Int position)
        {
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
            for (int y = size.y - 1; y > -1; y--)
            {
                for (int x = size.x - 1; x > -1; x--)
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
            return foreground.FindIndex(e => e.position == position && (e.isOnGoal || e.GetType().Equals(typeof(Goal))));
        }
    }
}
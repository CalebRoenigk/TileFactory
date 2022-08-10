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
            
            // TODO: REIMPLEMENT UPDATE SO ITS DONE IN 3 STAGES
            // STAGE 3 IS CHECKING GOALS
        }
        
        // Adds a entity to the grid
        public void AddEntity(Vector2Int position)
        {
            if (CheckPosition(position))
            {
                cargo.Add(new Box(this, position));
            }
        }
    }
}
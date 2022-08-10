using System;
using Factory;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory.Objects
{
    [System.Serializable]
    public class Entity
    {
        public Grid grid;

        public Vector2Int position;

        public Vector2Int movementDirection;
        
        public bool hasGravity;
        public bool isSolid;
        
        public bool willMove;
        public Vector2Int moveTo;

        public Entity()
        {
            this.isSolid = false;
            this.hasGravity = false;
        }

        public Entity(Grid grid)
        {
            this.grid = grid;
            
            this.isSolid = false;
            this.hasGravity = false;
        }
        
        public Entity(Grid grid, Vector2Int position)
        {
            this.grid = grid;
            this.position = position;
            
            this.isSolid = false;
            this.hasGravity = false;
        }
        
        public Entity(Grid grid, Vector2Int position, bool isSolid, bool hasGravity)
        {
            this.grid = grid;
            this.position = position;
            
            this.isSolid = isSolid;
            this.hasGravity = hasGravity;
        }
        
        // The checking update
        public void PreUpdate()
        {
            // Check gravity first
            if (hasGravity)
            {
                if (grid.CheckPosition(position + Vector2Int.down))
                {
                    // Can move down
                    willMove = true;
                    moveTo = position + Vector2Int.down;
                    return;
                }
                else
                {
                    // Check if the object can move horizontally
                    Entity objectBelow = grid.GetTileAtPosition(position + Vector2Int.down);
                    if (objectBelow != null && objectBelow.movementDirection != Vector2Int.zero)
                    {
                        willMove = true;
                        moveTo = position + objectBelow.movementDirection;
                        return;
                    }
                }
            }

            willMove = false;
            moveTo = position;
        }
        
        public void Update()
        {
            if (willMove)
            {
                position = moveTo;
            }
        }
    }
}
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory.Objects
{
    public class Box : Entity
    {
        public int fallDistance;
        public int maxFallDistance;
        
        public Box()
        {
            this.hasGravity = true;
            this.isSolid = true;
        }
        
        public Box(Grid grid, Vector2Int position)
        {
            this.grid = grid;

            this.position = position;
            
            this.hasGravity = true;
            this.isSolid = true;

            maxFallDistance = 3;
        }
        
        // The checking update
        public void PreUpdate()
        {
            base.PreUpdate();
        }
        
        // The main update
        public void Update()
        {
            base.Update();

            if (moveTo.y <= -1)
            {
                // Moving down, count fall distance
                fallDistance += moveTo.y;
            }
            
            else
            {
                // Moving not down
                // Check if the object has fallen farther than the max fall distance
                if (fallDistance >= maxFallDistance)
                {
                    // Object should break
                    base.DestroyEntity();
                }
                else
                {
                    // Reset the fall distance
                    fallDistance = 0;
                }
            }
        }
    }
}
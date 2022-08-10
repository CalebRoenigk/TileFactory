using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory.Objects
{
    public class Box : Entity
    {
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
        }
        
        // The checking update
        public void PreUpdate()
        {
            Debug.Log("Hello!");
            base.PreUpdate();
        }
    }
}
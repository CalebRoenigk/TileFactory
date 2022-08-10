using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory.Objects
{
    public class Goal : Entity
    {
        public Goal()
        {
            this.hasGravity = false;
            this.isSolid = true;
        }
        
        public Goal(Grid grid, Vector2Int position)
        {
            this.grid = grid;

            this.position = position;
            
            this.hasGravity = false;
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
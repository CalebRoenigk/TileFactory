using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory.Objects
{
    public class Water : Entity
    {
        public Water()
        {
            this.isSolid = false;
            this.hasGravity = false;
        }
        
        public Water(Grid grid, Vector2Int position): base(grid, position)
        {
            this.isSolid = false;
            this.hasGravity = false;
        }
    }
}
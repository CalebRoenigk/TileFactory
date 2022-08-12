using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory.Objects
{
    public class Conveyor : Entity
    {
        public Conveyor()
        {
            this.isSolid = true;
            this.hasGravity = false;
        }
        
        public Conveyor(Vector2Int movementDirection)
        {
            this.isSolid = true;
            this.hasGravity = false;
            this.movementDirection = movementDirection;
        }
        
        public Conveyor(Grid grid, Vector2Int position, Vector2Int movementDirection): base(grid, position)
        {
            this.isSolid = true;
            this.hasGravity = false;
            this.movementDirection = movementDirection;
        }
    }
}
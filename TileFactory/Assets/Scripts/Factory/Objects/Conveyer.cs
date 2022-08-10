using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory.Objects
{
    public class Conveyer : Entity
    {
        public Conveyer()
        {
            this.isSolid = true;
            this.hasGravity = false;
        }
        
        public Conveyer(Vector2Int movementDirection)
        {
            this.isSolid = true;
            this.hasGravity = false;
            this.movementDirection = movementDirection;
        }
    }
}
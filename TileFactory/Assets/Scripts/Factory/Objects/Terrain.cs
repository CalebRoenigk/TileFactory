using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory.Objects
{
    public class Terrain : Entity
    {
        public int terrainType; // The terrain variant
        
        public Terrain()
        {
            this.terrainType = 0;
            this.isSolid = true;
            this.hasGravity = false;
        }
        
        public Terrain(int terrainType)
        {
            this.terrainType = terrainType;
            this.isSolid = true;
            this.hasGravity = false;
        }
    }
}
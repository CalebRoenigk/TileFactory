using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Factory;
using Factory.Objects;

namespace Factory.Data
{
    [System.Serializable]
    public struct EntityResource
    {
        public sting name;
        public Entity entity;
        public TileBase tileBase;
    }
}

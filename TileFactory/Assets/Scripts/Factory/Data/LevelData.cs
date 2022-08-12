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
    [CreateAssetMenu(menuName = "Factory/Data/Level Data")]
    public class LevelData : ScriptableObject
    {
        public int levelIndex;
        public Grid grid;

        public List<Entity> foreground = new List<Entity>();
    }
}
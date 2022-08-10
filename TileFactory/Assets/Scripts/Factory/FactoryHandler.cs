using System;
using System.Collections;
using System.Collections.Generic;
using Factory.Objects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Factory
{
    public class FactoryHandler : MonoBehaviour
    {
        [SerializeField] private Grid grid;

        [SerializeField] private Tilemap foreground;
        [SerializeField] private Tilemap background;
        [SerializeField] private Tilemap entity;

        [SerializeField] private TileBase box;

        private void Start()
        {
            grid = new Grid(new Vector2Int(12, 300));
            grid.AddEntity(new Vector2Int(8, 25));
        }

        private void FixedUpdate()
        {
            grid.Update();
            
            entity.ClearAllTiles();

            foreach (Entity cargo in grid.cargo)
            {
                entity.SetTile((Vector3Int)cargo.position, box);
            }
        }
    }
}

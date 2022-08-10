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
    [CreateAssetMenu(menuName = "Factory/Data/Entity Resources")]
    public class EntityResources : ScriptableObject
    {
        public List<EntityResource> entityResources = new List<EntityResource>();
        
        // Returns a tilebase of an entity given the entity
        public TileBase GetEntityTileBase(Entity entity)
        {
            int entityIndex = entityResources.FindIndex(e => typeof(e.entity) == typeof(entity));
            if (entityIndex != -1)
            {
                return entityResources[entityIndex].tileBase;
            }

            return new TileBase();
        }
        
        // Return an entity from a given tilebase
        public Entity GetEntity(TileBase tile)
        {
            int entityIndex = entityResources.FindIndex(e => typeof(e.tileBase) == typeof(tile));
            if (entityIndex != -1)
            {
                return entityResources[entityIndex].entity;
            }

            return new Entity();
        }
    }
}
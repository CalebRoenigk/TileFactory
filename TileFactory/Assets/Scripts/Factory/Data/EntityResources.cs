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
            int entityIndex = entityResources.FindIndex(e => e.entityType == entity.GetType());
            if (entityIndex != -1)
            {
                return entityResources[entityIndex].tileBase;
            }

            return null;
        }
        
        // Return an entity from a given tilebase
        public Type GetEntity(TileBase tile)
        {
            int entityIndex = entityResources.FindIndex(e => e.tileBase.GetType() == tile.GetType());
            if (entityIndex != -1)
            {
                return entityResources[entityIndex].entityType;
            }

            return new Entity().GetType();
        }
    }
}
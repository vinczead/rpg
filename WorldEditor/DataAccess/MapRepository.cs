using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldEditor.Utility;

namespace WorldEditor.DataAccess
{
    public class MapRepository
    {
        readonly List<Map> maps = new List<Map>();
        public List<Map> GetMaps()
        {
            return new List<Map>(maps);
        }

        public Map GetMapById(string id)
        {
            var map = maps.FirstOrDefault(map => map.Id == id);

            if (map == null)
                throw new ArgumentException($"No map was found with id '{id}'.");

            return map;
        }
        public void AddMap(Map map)
        {
            if (map == null)
                throw new ArgumentNullException("map");

            maps.Add(map);
            MapAdded?.Invoke(this, new EntityEventArgs<Map>(map));
        }
        public void AddNewMap()
        {
            var i = 1;
            while (maps.FirstOrDefault(map => map.Id == $"Map{i}") != null)
                i++;

            AddMap(new Map() { Id = $"Map{i}" });
        }

        public void RemoveMap(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var map = maps.FirstOrDefault(map => map.Id == id);
            if (map == null)
                throw new ArgumentException($"No map was found with id '{id}'.");

            maps.Remove(map);
            MapRemoved(this, new EntityEventArgs<Map>(map));
        }

        public void RemoveAt(int index)
        {
            if(index < 0 || index >= maps.Count)
                throw new ArgumentException($"Invalid index: {index}");

            maps.RemoveAt(index);

        }

        public event EventHandler<EntityEventArgs<Map>> MapAdded;
        public event EventHandler<EntityEventArgs<Map>> MapRemoved;
    }
}

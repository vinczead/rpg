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
        public List<Map> Maps { get; set; } = new List<Map>();

        public List<Map> GetMaps()
        {
            return new List<Map>(Maps);
        }

        public Map GetMapById(string id)
        {
            var map = Maps.FirstOrDefault(map => map.Id == id);

            if (map == null)
                throw new ArgumentException($"No map was found with id '{id}'.");

            return map;
        }
        public void AddMap(Map map)
        {
            if (map == null)
                throw new ArgumentNullException("map");

            Maps.Add(map);
            MapAdded?.Invoke(this, new EntityEventArgs<Map>(map));
        }
        public void AddNewMap()
        {
            var i = 1;
            while (Maps.FirstOrDefault(map => map.Id == $"Map{i}") != null)
                i++;

            AddMap(new Map() { Id = $"Map{i}" });
        }

        public void RemoveMap(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var map = Maps.FirstOrDefault(map => map.Id == id);
            if (map == null)
                throw new ArgumentException($"No map was found with id '{id}'.");

            Maps.Remove(map);
            MapRemoved(this, new EntityEventArgs<Map>(map));
        }

        public void RemoveAt(int index)
        {
            if(index < 0 || index >= Maps.Count)
                throw new ArgumentException($"Invalid index: {index}");

            Maps.RemoveAt(index);

        }

        public event EventHandler<EntityEventArgs<Map>> MapAdded;
        public event EventHandler<EntityEventArgs<Map>> MapRemoved;
    }
}

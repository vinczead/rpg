using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WorldEditor.Utility;

namespace WorldEditor.DataAccess
{
    public class TileRepository
    {
        readonly List<Tile> tiles = new List<Tile>();
        public List<Tile> GetTiles()
        {
            return new List<Tile>(tiles);
        }

        public Tile GetTileById(string id)
        {
            var tile = tiles.FirstOrDefault(tile => tile.Id == id);

            if (tile == null)
                throw new ArgumentException($"No tile was found with id '{id}'.");

            return tile;
        }
        public void AddTile(Tile tile)
        {
            if (tile == null)
                throw new ArgumentNullException("tile");

            tiles.Add(tile);
            TileAdded?.Invoke(this, new EntityEventArgs<Tile>(tile));
        }
        public void AddNewTile()
        {
            var i = 1;
            while (tiles.FirstOrDefault(tile => tile.Id == $"Tile{i}") != null)
                i++;

            AddTile(new Tile() { Id = $"Tile{i}" });
        }

        public void RemoveTile(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var tile = tiles.FirstOrDefault(tile => tile.Id == id);
            if (tile == null)
                throw new ArgumentException($"No tile was found with id '{id}'.");

            tiles.Remove(tile);
            TileRemoved(this, new EntityEventArgs<Tile>(tile));
        }

        public void RemoveAt(int index)
        {
            if(index < 0 || index >= tiles.Count)
                throw new ArgumentException($"Invalid index: {index}");

            tiles.RemoveAt(index);

        }


        public event EventHandler<EntityEventArgs<Tile>> TileAdded;
        public event EventHandler<EntityEventArgs<Tile>> TileRemoved;
    }
}

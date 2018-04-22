using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Flags]
public enum GridLayer
{
    Walls,
    Entity,
    Obstacle,
    Floor
}

[RequireComponent(typeof(Grid))]
public class GridManager : MonoSingleton<GridManager>
{
    private Tilemap[] _tilemaps;

    public float GridSize {
        get
        {
            if (Math.Abs(_grid.cellSize.x - _grid.cellSize.y) > 0.01f)
                throw new Exception("Grid doesn't consist of squares.");
            return _grid.cellSize.x;
        }
    }

    private Grid _grid;

    private void Awake()
    {
        DefineSingleton(this);

        _grid = GetComponent<Grid>();
        _tilemaps = GetComponentsInChildren<Tilemap>();
    }

    public TileBase[] GetTilesOn(Vector3Int position)
    {
        var tiles = new List<TileBase>();
        foreach (var tilemap in _tilemaps)
        {
            var tile = tilemap.GetTile(position);
            if (tile != null)
                tiles.Add(tile);
        }

        return tiles.ToArray();
    }

    public TileBase GetTilesOn(Vector3Int position, GridLayer layer)
    {
        var tiles = new List<TileBase>();
        return _tilemaps.First(tilemap => tilemap.name == layer.ToString()).GetTile(position);
    }

    public bool IsFree(Vector3Int position, GridLayer layer = GridLayer.Walls)
    {
        var tile = GetTilesOn(new Vector3Int(position.x, position.y, 0), layer);
        return tile == null;
    }

    public bool IsFree(Vector3Int position)
    {
        var tile = GetTilesOn(new Vector3Int(position.x, position.y, 0));
        return tile.Length == 0;
    }
}

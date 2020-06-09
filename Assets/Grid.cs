using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Grid
{
    private Cell[] _cells;
    private int _height;
    private int _width;
    private Player _player;

    public Cell this[int x, int y]
    {
        get
        {
            if (x >= _width || y >= _height)
                throw new InvalidOperationException("There is no cell with such x: " + 
                                                    x + " and y: " + y + " in grid.");

            return _cells[_height * x + y];
        }

        set
        {
            _cells[_height * x + y] = value;
            _cells[_height * x + y].LocalPosition = new Vector2(x, y);
        }
    }

    public Grid(int width, int height, Player player)
    {
        _cells = new Cell[width * height];
        _height = height;
        _width = width;
        _player = player;
    }
}

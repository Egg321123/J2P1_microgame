using System;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    // constants
    private const int MAX_ATTEMPTS = 100;   // the amount of times a new path is allowed to be generated
    private const byte DOWN = 0b00;         // binary notation of the DOWN direction
    private const byte RIGHT = 0b01;        // binary notation of the RIGHT direction
    private const byte UP = 0b10;           // binary notation of the UP direction
    private const byte LEFT = 0b11;         // binary notation of the LEFT direction

    private readonly TileData[,] tiles;     // contains the tiles in the world
    private readonly List<Vector2Int> path; // contains the positions of the tiles
    private readonly int scale;             // sets the width and height of the level

    /// <summary>
    /// creates a new level
    /// </summary>
    public Level(int scale)
    {
        this.scale = scale;
        tiles = new TileData[scale, scale];
        path = new List<Vector2Int>();
    }

    // attempt to generate a new path, return TRUE if successful, FALSE if unsuccessful
    private bool Generate(System.Random rand)
    {
        // fill the level with empty tiles
        for (int x = 0; x < scale; x++)
            for (int y = 0; y < scale; y++)
                tiles[x, y] = new TileData(TileType.EMPTY, new Vector2Int(x, y));

        // clear the path list
        path.Clear();

        int directions = rand.Next(int.MaxValue);   // store the directions as a binary-encoded random string
        int posX = 0;                               // the current tile X position
        int posY = 0;                               // the current tile Y position
        int bitOffset = 0;                          // the offset in bits that is being read from directions
        int attempts = 0;                           // the amount of times the code has attempted to select a tile

        // run while posX and posY haven't reached their destination or scale^2 (surface area) hasn't been reached
        while ((posX != (scale - 1) || posY != (scale - 1)) && attempts < (scale * scale))
        {
            // if the current type is empty, create a new path node
            if (tiles[posX, posY].type == TileType.EMPTY)
            {
                tiles[posX, posY].type = TileType.PATH; // set the tile's type to PATH
                path.Add(new Vector2Int(posX, posY));   // add the path node
            }

            // get the current movement data
            byte data = (byte)((directions >> bitOffset) & 0b11); // exclude the bits that are about the direction
            int x = posX;
            int y = posY;

            // increase the bit offset by two, because we read two bits at a time
            bitOffset += 2;

            // if the new bit offset is outside of the range of the size of an int, generate new random data
            if (bitOffset > (sizeof(int) * 8) - 1) // sizeof gets the size in bytes, 1B = 8 bit, remove one bit because signed (can be negative)
            {
                directions = rand.Next(int.MaxValue);
                bitOffset = 0;
            }

            // decide the direction to travel
            if (data == DOWN) y++;  // DOWN is first, because DOWN is 0b00, and otherwise cause an error
            else if (data == RIGHT) x++;
            else if (data == UP) y--;
            else if (data == LEFT) x--;

            // if the new location is valid to place a tile at, store the new HEAD tile position
            if (IsValidPlacement(x, y))
            {
                posX = x;
                posY = y;
            }

            // increase the attempt
            attempts++;
        }

        // return the success of the operation
        if (attempts >= (scale * scale))
            return false;

        return true;
    }

    /// <summary>
    /// generates the level until <see cref="MAX_ATTEMPTS"/> has been reached.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public void GenerateLevel(int level)
    {
        System.Random rand = new(level);    // create a new random using the current level as the seed
        int attempts = 0;                   // the amount of times that a path has been attempted to generate
        bool success = false;               // whether the generation was successful

        // generate a new level until successful or MAX_ATTEMPTS has been reached
        while (success == false && attempts < MAX_ATTEMPTS)
        {
            success = Generate(rand);
            attempts++;
        }

        // throw an exception if MAX_ATTEMPTS has been reached
        if (attempts >= MAX_ATTEMPTS)
            throw new IndexOutOfRangeException($"could not find a valid path for level '{level}'");

        Debug.Log($"Generated level {level} in {attempts} attempts.");
    }

    /// <returns>
    /// <see langword="true"/> if the location is within the bounds of the level, and on an empty tile, otherwise <see langword="false"/> is returned
    /// </returns>
    public bool IsValidPlacement(int x, int y)
    {
        // check if the position is outside the level
        if (x < 0 || x >= scale || y < 0 || y >= scale)
            return false;

        // check if the position is not an empty tile
        if (tiles[x, y].type != TileType.EMPTY)
            return false;

        return true;
    }

    // gets the tile at the position
    public TileData GetTile(int x, int y) => tiles[x, y];
    public TileData GetTile(Vector2Int pos) => tiles[pos.x, pos.y];
}

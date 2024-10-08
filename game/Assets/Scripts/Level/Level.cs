using System;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    // constants
    private const int MAX_ATTEMPTS = 1000;  // the amount of times a new path is allowed to be regenerated
    private const int MAX_SKIPS = 10;       // the amount of times a tile is allowed to be skipped in generation (tiles are skipped if the picked direction can't be placed)
    private const byte DOWN = 0b00;         // binary notation of the DOWN direction
    private const byte RIGHT = 0b01;        // binary notation of the RIGHT direction
    private const byte UP = 0b10;           // binary notation of the UP direction
    private const byte LEFT = 0b11;         // binary notation of the LEFT direction

    public readonly int width;              // sets the width of the level
    public readonly int height;             // sets the height of the level
    private readonly TileData[,] tiles;     // contains the tiles in the world
    private readonly List<Vector2Int> path; // contains the positions of the tiles

    /// <summary>
    /// creates a new level instance
    /// </summary>
    public Level(int width, int height)
    {
        this.width = width;
        this.height = height;
        tiles = new TileData[width, height];
        path = new List<Vector2Int>();
    }

    private void AddPathNode(int x, int y)
    {
        tiles[x, y].type = TileType.PATH; // set the tile's type to PATH
        path.Add(new Vector2Int(x, y));   // add the path node
    }

    // attempt to generate a new path, return TRUE if successful, FALSE if unsuccessful
    private bool GeneratePath(System.Random rand)
    {
        // fill the level with empty tiles
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                tiles[x, y] = new TileData(TileType.EMPTY, new Vector2Int(x, y));

        // clear the path list
        path.Clear();

        int directions = rand.Next(int.MaxValue);   // store the directions as a binary-encoded random string
        int posX = 0;                               // the current tile X position
        int posY = 0;                               // the current tile Y position
        int bitOffset = 0;                          // the offset in bits that is being read from directions
        int skipCount = 0;                          // the amount of times that a a tile is skipped

        // run while posX and posY haven't reached their destination
        // or if MAX_SKIPS count has been reached
        while ((posX != (width - 1) || posY < (height - 1)) && skipCount < MAX_SKIPS)
        {
            // if the current type is empty, create a new path at this location
            if (tiles[posX, posY].type == TileType.EMPTY)
            {
                AddPathNode(posX, posY);    // add the node
                skipCount = 0;              // reset the skip count
            }

            // get the current movement data
            byte data = (byte)((directions >> bitOffset) & 0b11); // isolate 2 bits which will contain the direction information
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
            if (data == DOWN) y++;
            else if (data == RIGHT) x++;
            else if (data == UP) y--;
            else if (data == LEFT) x--;

            // if the new location is invalid to place a tile at
            if (IsValidPlacement(x, y) == false)
            {
                // increase the skip count
                skipCount++;
                continue; // skip to the next loop
            }

            // set this as the HEAD / newest tile position
            posX = x;
            posY = y;
        }

        AddPathNode(posX, posY);    // add the final node

        // return the success of the operation
        if (skipCount >= MAX_SKIPS)
            return false;

        path.Reverse(); // reverse the path, uwu

        return true;
    }

    /// <summary>
    /// attempts to generate a valid level with a random path until <see cref="MAX_ATTEMPTS"/> has been reached.
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
            success = GeneratePath(rand);
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
    public bool IsValidPlacement(int x, int y, bool ignoreLevelBounds = false)
    {
        // check if the position is outside the level
        if (x < 0 || x >= width || y < 0 || y >= height)
            return ignoreLevelBounds;

        // check if the position is not an empty tile
        if (tiles[x, y].type != TileType.EMPTY)
            return false;

        return true;
    }

    // gets the tile at the position
    public TileData GetTile(int x, int y) => tiles[x, y];
    public TileData GetTile(Vector2Int pos) => tiles[pos.x, pos.y];

    // gets the path
    public IReadOnlyList<Vector2Int> GetPath() => path;
}

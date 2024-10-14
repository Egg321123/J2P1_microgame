using System;
using UnityEngine;

[Serializable]
public struct TileMeshData
{
    public Mesh end;            // has 1 adjointed neighbour
    public Mesh straight;       // has 2 opposing neighbours
    public Mesh corner;         // has 2 adjointed neighbours
    public Mesh single;         // has 3 adjointed neighbours
    public Mesh full;           // has no neighbours
    public Mesh empty;          // has neighbours on each side
}

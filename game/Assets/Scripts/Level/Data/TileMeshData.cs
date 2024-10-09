using System;
using UnityEngine;

[Serializable]
public struct TilePrefabData
{
    public GameObject straight;     // has 2 opposing neighbours
    public GameObject corner;       // has 2 adjointed neighbours
    public GameObject single;       // has 3 adjointed neighbours
    public GameObject full;         // has no neighbours
    public GameObject empty;        // has neighbours on each side
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ShockwaveProjectile : MonoBehaviour
{
    [SerializeField] ParticleSystem system;

    public void Initialize(float range = 1)
    {
        float radius = range * 2;

        ParticleSystem.MainModule sizeModule = system.main;

        sizeModule.startSize = radius;
    }
}
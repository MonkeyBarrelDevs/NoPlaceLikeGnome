using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] CollectibleType collectibleType;

    public CollectibleType GetCollectibleType() 
    {
        return collectibleType;
    }

    public enum CollectibleType
    {
        None,
        Water,
        Nutrients,
        SpeedSeed,
        BackpackSeed,
        NutrientSeed,
        WaterSeed
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int water = 0;
    public int nutrients = 0;
    public int seeds = 0;
    public Boolean hasBackpack = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getWater()
    {
        return water;
    }

    public int getNutrients()
    {
        return nutrients;
    }

    public int getSeeds()
    {
        return seeds;
    }

    public void setWater(int setter)
    {
        water = setter;
    }

    public void setNutrients(int setter)
    {
        nutrients = setter;
    }

    public void setSeeds(int setter)
    {
        seeds = setter;
    }
}

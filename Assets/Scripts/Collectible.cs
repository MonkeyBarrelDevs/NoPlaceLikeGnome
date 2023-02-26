using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    enum CollectibleType
    {
        Water,
        Nutrients,
        Seeds
    }

    [SerializeField] CollectibleType collectibleType;
    GameController gameController;
    bool triggered;
    static bool holdingFull, backpackFull;

     void FindReferences()
    {
        //gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

     // Start is called before the first frame update
    void Start()
    {
        holdingFull = false;
        backpackFull = false;
        triggered = false;
        FindReferences();
    }

    void ResolvePickup()
    {
        switch (collectibleType) 
        {
            case CollectibleType.Water:
                if(gameController.getWater() == 0)
                    holdingFull = true;
                else if(gameController.getWater() == 1)
                    backpackFull = true;
                //audioManager.Play("PickUpWater");
                break;
            case CollectibleType.Nutrients:
                if(gameController.getNutrients() == 0)
                    holdingFull = true;
                else if(gameController.getNutrients() == 1)
                    backpackFull = true;
                //audioManager.Play("PickUpNutrients");
                break;
            case CollectibleType.Seeds:
                if(gameController.getSeeds() == 0)
                    holdingFull = true;
                else if(gameController.getSeeds() == 1)
                    backpackFull = true;
                //audioManager.Play("PickUpSeeds")
                break;
        }
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            triggered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            triggered = false;
        }
    }

    private void Update()
    {
        if(triggered)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                switch (collectibleType)
                {
                    case CollectibleType.Water:
                        if((gameController.getWater() == 0 || (gameController.hasBackpack && gameController.getWater() == 1)) && ((gameController.getNutrients() + gameController.getSeeds() + gameController.getWater()) < 3))
                        {
                            if(!holdingFull || (!backpackFull && gameController.hasBackpack))
                            {
                                ResolvePickup();
                                gameController.setWater(gameController.getWater() + 1);
                            }
                        }
                        break;
                    case CollectibleType.Nutrients:
                        if((gameController.getNutrients() == 0 || (gameController.hasBackpack && gameController.getNutrients() == 1)) && ((gameController.getNutrients() + gameController.getSeeds() + gameController.getWater()) < 3))
                        {
                            if(!holdingFull || (!backpackFull && gameController.hasBackpack))
                            {
                                ResolvePickup();
                                gameController.setNutrients(gameController.getNutrients() + 1);
                            }
                        }
                        break;
                    case CollectibleType.Seeds:
                        if((gameController.getSeeds() == 0 || (gameController.hasBackpack && gameController.getSeeds() == 1)) && ((gameController.getNutrients() + gameController.getSeeds() + gameController.getWater()) < 3))
                        {
                            if(!holdingFull || (!backpackFull && gameController.hasBackpack))
                            {
                                ResolvePickup();
                                gameController.setSeeds(gameController.getSeeds() + 1);
                            }
                        }
                        break;
                }
            }
        }
    }
}

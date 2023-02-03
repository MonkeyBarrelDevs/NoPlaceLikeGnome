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
    Boolean triggered = false;

     void FindReferences()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

     // Start is called before the first frame update
    void Start()
    {
        FindReferences();
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    void ResolvePickup()
    {
        switch (collectibleType) 
        {
            case CollectibleType.Water:
                //audioManager.Play("PickUpWater");
                break;
            case CollectibleType.Nutrients:
                //audioManager.Play("PickUpNutrients");
                break;
            case CollectibleType.Seeds:
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
                        if(gameController.getWater() == 0 || (gameController.hasBackpack && gameController.getWater() == 1))
                        {
                            ResolvePickup();
                            gameController.setWater(gameController.getWater() + 1);
                        }
                        break;
                    case CollectibleType.Nutrients:
                        if(gameController.getNutrients() == 0 || (gameController.hasBackpack && gameController.getNutrients() == 1))
                        {
                            ResolvePickup();
                            gameController.setNutrients(gameController.getNutrients() + 1);
                        }
                        break;
                    case CollectibleType.Seeds:
                        if(gameController.getSeeds() == 0 || (gameController.hasBackpack && gameController.getSeeds() == 1))
                        {
                            ResolvePickup();
                            gameController.setSeeds(gameController.getSeeds() + 1);
                        }
                        break;
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleDropoff : MonoBehaviour
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

    void ResolveDropoff()
    {
        switch (collectibleType) 
        {
            case CollectibleType.Water:
                //audioManager.Play("DropoffWater");
                break;
            case CollectibleType.Nutrients:
                //audioManager.Play("DropoffNutrients");
                break;
            case CollectibleType.Seeds:
                //audioManager.Play("DropoffSeeds")
                break;
        }
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
                        if(gameController.getWater() > 0)
                        {
                            ResolveDropoff();
                            gameController.setWater(gameController.getWater() - 1);
                        }
                        break;
                    case CollectibleType.Nutrients:
                        if(gameController.getNutrients() > 0)
                        {
                            ResolveDropoff();
                            gameController.setNutrients(gameController.getNutrients() - 1);
                        }
                        break;
                    case CollectibleType.Seeds:
                        if(gameController.getSeeds() > 0)
                        {
                            ResolveDropoff();
                            gameController.setSeeds(gameController.getSeeds() - 1);
                        }
                        break;
                }
            }
        }
    }
}
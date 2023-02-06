using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Collectible.CollectibleType HoldSlot = Collectible.CollectibleType.None;
    [SerializeField] Collectible.CollectibleType BackpackSlot = Collectible.CollectibleType.None;

    [SerializeField] Transform pickupCenter;
    [SerializeField] float pickupRange = 1;

    [SerializeField] GameObject WaterObject;
    [SerializeField] GameObject NutrientObject;
    [SerializeField] GameObject SpeedSeedObject;
    [SerializeField] GameObject BackpackSeedObject;
    [SerializeField] GameObject NutrientsSeedObject;
    [SerializeField] GameObject WaterSeedObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Holding Keybind
        {
            // Interact with Plot / House Logic
            //if (InteractCheck()) { }

            // Drop Held Item Logic
            if (DropCheck()) { }
            
            // Pickup Collectible Logic
            else if (PickupCheck()) { }
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // Backpack Keybind
        {
            // Check for backpack upgrade
            Collectible.CollectibleType swapHolder = HoldSlot;
            HoldSlot = BackpackSlot;
            BackpackSlot = swapHolder;
        }
    }

    //bool InteractCheck() 
    //{
    //    Collider[] collidedObjects = Physics.OverlapSphere(pickupCenter.position, pickupRange);

    //    GameObject interactableObject = null;

    //    foreach (Collider collider in collidedObjects)
    //    {
    //        if (collider.gameObject.GetComponent<Interactable>())
    //        {
    //            interactableObject = collider.gameObject;
    //            break;
    //        }
    //    }

    //    return (interactableObject && interactableObject.GetComponent<Interactable>().Interact(ref this.gameObject));
    //}

    bool DropCheck() 
    {
        switch (HoldSlot)
        {
            case Collectible.CollectibleType.None:
                return false;
            case Collectible.CollectibleType.Water:
                Instantiate(WaterObject, pickupCenter.transform.position, Quaternion.identity);
                break;
            case Collectible.CollectibleType.Nutrients:
                Instantiate(NutrientObject, pickupCenter.transform.position, Quaternion.identity);
                break;
            case Collectible.CollectibleType.SpeedSeed:
                Instantiate(SpeedSeedObject, pickupCenter.transform.position, Quaternion.identity);
                break;
            case Collectible.CollectibleType.BackpackSeed:
                Instantiate(BackpackSeedObject, pickupCenter.transform.position, Quaternion.identity);
                break;
            case Collectible.CollectibleType.NutrientSeed:
                Instantiate(NutrientsSeedObject, pickupCenter.transform.position, Quaternion.identity);
                break;
            case Collectible.CollectibleType.WaterSeed:
                Instantiate(WaterSeedObject, pickupCenter.transform.position, Quaternion.identity);
                break;
        }

        HoldSlot = Collectible.CollectibleType.None;
        return true;
    }

    bool PickupCheck() 
    {
        Collider[] collidedObjects = Physics.OverlapSphere(pickupCenter.position, pickupRange);

        GameObject collectibleObject = null;

        foreach (Collider collider in collidedObjects)
        {
            if (collider.gameObject.GetComponent<Collectible>())
            {
                collectibleObject = collider.gameObject;
                break;
            }
        }

        if (collectibleObject && StoreIn(collectibleObject.GetComponent<Collectible>().GetCollectibleType(), ref HoldSlot))
        {
            Destroy(collectibleObject);
            return true;
        }
        return false;
    }

    bool StoreIn(Collectible.CollectibleType resource, ref Collectible.CollectibleType container)
    {
        if (container == Collectible.CollectibleType.None)
        {
            container = resource;
            return true;
        }
        return false;
    }
}

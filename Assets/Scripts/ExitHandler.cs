using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHandler : MonoBehaviour
{

    public int exitDirectionID;
    private WorldGenerator worldGenerator; 

    // Start is called before the first frame update
    void Start()
    {
        worldGenerator = FindObjectOfType<WorldGenerator>();
        if (worldGenerator == null) {
            Debug.Log("Failed to find the world generator object.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other) {
        //Debug.Log("Generate room");
        if (other.tag == "Player") {
            //worldGenerator.GenerateRoom(exitDirectionID);
            //worldGenerator.RenderRooms(exitDirectionID);
        }
    }

}

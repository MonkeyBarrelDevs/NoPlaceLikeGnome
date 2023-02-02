using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    private WorldState worldState;

    public int northRenderDistance = 1;
    public int southRenderDistance = 1;
    public int eastRenderDistance = 1;
    public int westRenderDistance = 1;

    public GameObject room;

    public Vector2 distanceOffset;

    public Dictionary<(int, int), GameObject> renderedRooms;

    public PlayerController player;

    public int currentRoomX;
    public int currentRoomZ;

    public int worldSeed;
    // Start is called before the first frame update
    void Start()
    {
        currentRoomX = 0;
        currentRoomZ = 0;
        player = FindObjectOfType<PlayerController>();
        renderedRooms = new();
        worldState = new(worldSeed);
        //renderedRooms.Add((0, 0), GameObject.Find("Room"));
        //RenderRooms();
    }

    // Update is called once per frame
    void Update()
    {
        currentRoomX = (int)((Math.Abs(player.x) + 5) / 10) * Math.Sign(player.x);
        currentRoomZ = (int)((Math.Abs(player.z) + 5) / 10) * Math.Sign(player.z);
        //Debug.Log("Current room: " + currentRoomX + ", " + currentRoomZ + ".");
        RenderRooms();
        //Debug.Log(worldState.GetCurrentRoomX() + ", y: " + worldState.GetCurrentRoomY());
    }

    public void GenerateRoom(int directionID) {
        Debug.Log("Trying to generate a room");
        int newRoomOffsetX = GetNewRoomOffsetXFromID(directionID);
        int newRoomOffsetY = GetNewRoomOffsetYFromID(directionID);
        worldState.SetCurrentRoom(newRoomOffsetX, newRoomOffsetY);

        if (worldState.RoomExists(newRoomOffsetX, newRoomOffsetY)) {
            return;
        }

        WorldState.Room newRoom = new(newRoomOffsetX, newRoomOffsetY, worldState.worldSeedXOffset, worldState.worldSeedZOffset);
        worldState.AddRoom(newRoom);
        Instantiate(room, new Vector3(newRoomOffsetX * distanceOffset.x, 0, newRoomOffsetY * distanceOffset.y), new Quaternion(0, 180, 0, 0));
    }

    public void RenderRooms() {
        //Debug.Log("Trying to generate a room");
        for (int z = -southRenderDistance; z < northRenderDistance + 1; z++) {
            for (int x = -westRenderDistance; x < eastRenderDistance + 1; x++) {
                if (renderedRooms.ContainsKey((currentRoomX + x, currentRoomZ + z))) {
                    continue;
                }

                if (!worldState.RoomExists(currentRoomX + x, currentRoomZ + z)) {
                    WorldState.Room newRoom = new(currentRoomX + x, currentRoomZ + z, worldState.worldSeedXOffset, worldState.worldSeedZOffset);
                    worldState.AddRoom(newRoom);
                }

                GameObject renderedRoom = Instantiate(room, new Vector3((currentRoomX + x) * distanceOffset.x, 0, (currentRoomZ + z) * distanceOffset.y), new Quaternion(0, 180, 0, 0));
                renderedRooms.TryAdd((currentRoomX + x, currentRoomZ + z), renderedRoom);

            }
        }

        foreach (KeyValuePair<(int, int), GameObject> renderedRoom in renderedRooms) {
            if (renderedRoom.Key.Item1 > currentRoomX && Math.Abs(renderedRoom.Key.Item1 - currentRoomX) > eastRenderDistance) {
                Destroy(renderedRoom.Value);
                renderedRooms.Remove(renderedRoom.Key);
            } else if (renderedRoom.Key.Item1 < currentRoomX && Math.Abs(renderedRoom.Key.Item1 - currentRoomX) > westRenderDistance) {
                Destroy(renderedRoom.Value);
                renderedRooms.Remove(renderedRoom.Key);
            } else if (renderedRoom.Key.Item2 < currentRoomZ && Math.Abs(renderedRoom.Key.Item2 - currentRoomZ) > southRenderDistance) {
                Destroy(renderedRoom.Value);
                renderedRooms.Remove(renderedRoom.Key);
            } else if (renderedRoom.Key.Item2 > currentRoomZ && Math.Abs(renderedRoom.Key.Item2 - currentRoomZ) > northRenderDistance) {
                Destroy(renderedRoom.Value);
                renderedRooms.Remove(renderedRoom.Key);
            }

            /*if (Math.Abs(renderedRoom.Key.Item1 - currentRoomX) >= southRenderDistance + northRenderDistance || Math.Abs(renderedRoom.Key.Item2 - currentRoomZ) >= eastRenderDistance + westRenderDistance) {
                Destroy(renderedRoom.Value);
                renderedRooms.Remove(renderedRoom.Key);
            }*/
        }


        /*for (int z = -1; z < 4; z++) {
            for (int x = -2; x < 3; x++) {
                if (renderedRooms.ContainsKey((currentRoomX + x, currentRoomZ + z))) {
                    continue;
                }

                if (!worldState.RoomExists(currentRoomX + x, currentRoomZ + z)) { 
                    WorldState.Room newRoom = new(currentRoomX + x, currentRoomZ + z);
                    worldState.AddRoom(newRoom);
                }

                GameObject renderedRoom = Instantiate(room, new Vector3((currentRoomX + x) * distanceOffset.x, 0, (currentRoomZ + z) * distanceOffset.y), new Quaternion(0, 180, 0, 0));
                renderedRooms.TryAdd((currentRoomX + x, currentRoomZ + z), renderedRoom);

            }
        }

        foreach (KeyValuePair<(int, int), GameObject> renderedRoom in renderedRooms) {
            if (Math.Abs(renderedRoom.Key.Item1 - currentRoomX) > 2 || Math.Abs(renderedRoom.Key.Item2 - currentRoomZ) > 3) {
                Destroy(renderedRoom.Value);
                renderedRooms.Remove(renderedRoom.Key);
            }
        }*/
    }

    public void UpdateCurrentRoom(int directionID) {
        int newRoomOffsetX = GetNewRoomOffsetXFromID(directionID);
        int newRoomOffsetY = GetNewRoomOffsetYFromID(directionID);
        worldState.SetCurrentRoom(newRoomOffsetX, newRoomOffsetY);
    }

    private int GetNewRoomOffsetXFromID(int directionID) {
        int currentRoomOffsetX = worldState.GetCurrentRoomX();
        if (directionID == 2) {
            Debug.Log(currentRoomOffsetX+1);
            return ++currentRoomOffsetX;
        }
        if (directionID == 3) {
            Debug.Log(currentRoomOffsetX-1);
            return --currentRoomOffsetX;
        }
        return currentRoomOffsetX;
    }

    private int GetNewRoomOffsetYFromID(int directionID) {
        int currentRoomOffsetY = worldState.GetCurrentRoomY();
        if (directionID == 0) {
            return ++currentRoomOffsetY;
        }
        if (directionID == 1) {
            return --currentRoomOffsetY;
        }
        return currentRoomOffsetY;
    }
}

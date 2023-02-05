using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using System.Runtime.CompilerServices; 
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour {

    enum CollectibleType {
        Water,
        Nutrients,
        Seeds
    }

    public GameObject water;
    public GameObject nutrients;

    public TileRenderer tileRenderer;

    private WorldState worldState;

    // Room render distances; renders 1 room out from the current room in each cardinal direction by default.
    public int northRenderDistance = 1;
    public int southRenderDistance = 1;
    public int eastRenderDistance = 1;
    public int westRenderDistance = 1;

    public GameObject room;
    public GameObject blue;

    public Vector2 distanceOffset;

    public Dictionary<(int, int), WorldState.Room> renderedRooms;
    // TODO: Naive approach. Make object pool here.
    public Dictionary<WorldState.Room, List<GameObject>> renderedRoomProps;

    public PlayerController player;

    public int currentRoomX;
    public int currentRoomZ;

    public int worldSeed;
    public int minimumRoomResources;
    public int maximumRoomResources;
    // Start is called before the first frame update
    void Start()
    {
        currentRoomX = 0;
        currentRoomZ = 0;
        player = FindObjectOfType<PlayerController>();
        renderedRooms = new();
        renderedRoomProps = new();
        worldState = new(worldSeed);
        //renderedRooms.Add((0, 0), GameObject.Find("Room"));
        //RenderRooms();
    }

    // Update is called once per frame
    void Update()
    {
        currentRoomX = (int)(player.x / 20) - (player.x < 0 ? 1 : 0);
        currentRoomZ = (int)(player.z / 20) - (player.z < 0 ? 1 : 0);
        //if (player.x < 0) currentRoomX--;
        //if (player.z < 0) currentRoomZ--;
        //Debug.Log("Current room: " + currentRoomX + ", " + currentRoomZ + ".");
        RenderRooms();
        //Debug.Log(worldState.GetCurrentRoomX() + ", y: " + worldState.GetCurrentRoomY());
    }

    public void RenderRooms() {
        //Debug.Log("Trying to generate a room");
        for (int z = -southRenderDistance; z < northRenderDistance + 1; z++) {
            for (int x = -westRenderDistance; x < eastRenderDistance + 1; x++) {
                if (renderedRooms.ContainsKey((currentRoomX + x, currentRoomZ + z))) {
                    continue;
                }
                
                //GameObject renderedRoom = Instantiate(room, new Vector3((currentRoomX + x) * distanceOffset.x, 0, (currentRoomZ + z) * distanceOffset.y), new Quaternion(0, 180, 0, 0));
                

                if (!worldState.RoomExists(currentRoomX + x, currentRoomZ + z)) {
                    WorldState.Room newRoom = new(currentRoomX + x, currentRoomZ + z, worldState.worldSeedXOffset, worldState.worldSeedZOffset);
                    worldState.AddRoom(newRoom);
                }
                WorldState.Room currentRoom = worldState.GetRoom(currentRoomX + x, currentRoomZ + z);
                renderedRooms.TryAdd((currentRoomX + x, currentRoomZ + z), currentRoom); // Write extension method TryGetRoom
                tileRenderer.FillRoomGround(currentRoomX + x, currentRoomZ + z, currentRoom.grassNoise);

                renderedRoomProps.TryAdd(currentRoom, new List<GameObject>());
                foreach ((CollectibleType, int, int) resource in currentRoom.roomProps) {
                    switch (resource.Item1) {
                        case CollectibleType.Nutrients:
                            // Instantiates the new resource and adds it to the list of rendered room props for the current room.
                            renderedRoomProps[currentRoom].Add(Instantiate(nutrients, new Vector3(resource.Item2, 0, resource.Item3), new Quaternion(0, 180, 0, 0)));
                            break;
                        case CollectibleType.Water:
                            // Instantiates the new resource and adds it to the list of rendered room props for the current room.
                            renderedRoomProps[currentRoom].Add(Instantiate(water, new Vector3(resource.Item2, 0, resource.Item3), new Quaternion(0, 180, 0, 0)));
                            break;
                    }
                }
            }
        }

        foreach (KeyValuePair<(int, int), WorldState.Room> renderedRoom in renderedRooms) {
            if (renderedRoom.Key.Item1 > currentRoomX && Math.Abs(renderedRoom.Key.Item1 - currentRoomX) > eastRenderDistance) {
                //Destroy(renderedRoom.Value);
                ClearRoomResources(renderedRoom.Value);
                tileRenderer.ClearRoomTiles(renderedRoom.Key.Item1, renderedRoom.Key.Item2);
                renderedRooms.Remove(renderedRoom.Key);
            } else if (renderedRoom.Key.Item1 < currentRoomX && Math.Abs(renderedRoom.Key.Item1 - currentRoomX) > westRenderDistance) {
                //Destroy(renderedRoom.Value);
                ClearRoomResources(renderedRoom.Value);
                tileRenderer.ClearRoomTiles(renderedRoom.Key.Item1, renderedRoom.Key.Item2);
                renderedRooms.Remove(renderedRoom.Key);
            } else if (renderedRoom.Key.Item2 < currentRoomZ && Math.Abs(renderedRoom.Key.Item2 - currentRoomZ) > southRenderDistance) {
                //Destroy(renderedRoom.Value);
                ClearRoomResources(renderedRoom.Value);
                tileRenderer.ClearRoomTiles(renderedRoom.Key.Item1, renderedRoom.Key.Item2);
                renderedRooms.Remove(renderedRoom.Key);
            } else if (renderedRoom.Key.Item2 > currentRoomZ && Math.Abs(renderedRoom.Key.Item2 - currentRoomZ) > northRenderDistance) {
                //Destroy(renderedRoom.Value);
                ClearRoomResources(renderedRoom.Value);
                tileRenderer.ClearRoomTiles(renderedRoom.Key.Item1, renderedRoom.Key.Item2);
                renderedRooms.Remove(renderedRoom.Key);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ClearRoomResources(WorldState.Room room) {
        foreach (GameObject roomProp in renderedRoomProps[room]) {
            Destroy(roomProp);
        }
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

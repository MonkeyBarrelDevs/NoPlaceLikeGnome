using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState
{

    public class Room {
        public int roomOffsetX;
        public int roomOffsetY; // TODO: refactor to z
        public float[,] grassNoise;

        public Room(int offsetX, int offsetY, int worldSeedXOffset, int worldSeedZOffset) {
            grassNoise = new float[20, 20];
            roomOffsetX = offsetX;
            roomOffsetY = offsetY;
            GenerateGrassNoise(worldSeedXOffset, worldSeedZOffset);
        }

        private void GenerateGrassNoise(int worldSeedXOffset, int worldSeedZOffset) {
            float roomStartX = roomOffsetX * 20 + worldSeedXOffset;
            float roomStartZ = roomOffsetY * 20 + worldSeedZOffset;

            for (int x = 0; x < 20; x++) {
                for (int z = 0; z < 20; z++) {
                    float grassNoiseValue = Mathf.PerlinNoise((float)(roomStartX + x)/200000, (float)(roomStartZ + z)/200000);
                    //Debug.Log(grassNoiseValue);
                    grassNoise[x, z] = grassNoiseValue;
                }
            }

            /*Debug.Log("For room x" + roomOffsetX + ", z" + roomOffsetY);
            for (int x = 0; x < 20; x++) {
                for (int z = 0; z < 20; z++) {
                    Debug.Log(grassNoise[x, z]);
                }
                Debug.Log("\n");
            }*/
        }
    }

    public static Dictionary<(int, int), Room> rooms;
    private int currentRoomX;
    private int currentRoomY;
    private int worldSeed;
    public int worldSeedXOffset;
    public int worldSeedZOffset;

    public WorldState(int seed) {
        worldSeed = seed;
        System.Random rand = new(seed);
        worldSeedXOffset = rand.Next(0, 100000);
        worldSeedZOffset = rand.Next(0, 100000);
        rooms = new();
        currentRoomX = 0;
        currentRoomY = 0;
    }

    public void AddRoom(Room newRoom) {
        rooms.Add((newRoom.roomOffsetX, newRoom.roomOffsetY), newRoom);
    }

    public Room GetRoom(int roomX, int roomZ) { 
        if (rooms.TryGetValue((roomX, roomZ), out Room room)) {
            return room;
        }
        return null;
    } 

    public bool RoomExists(int roomOffsetX, int roomOffsetY) {
        return rooms.ContainsKey((roomOffsetX, roomOffsetY));
    }

    public void SetCurrentRoom(int currentRoomX, int currentRoomY) {
        this.currentRoomX = currentRoomX;
        this.currentRoomY = currentRoomY;
    }

    public int GetCurrentRoomX() {
        return currentRoomX;
    }

    public int GetCurrentRoomY() {
        return currentRoomY;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldState
{

    public class Room {
        public enum CollectibleType {
            Water,
            Nutrients,
            Seeds
        }

        public int roomOffsetX;
        public int roomOffsetY; // TODO: refactor to z
        public float[,] grassNoise;
        public Tilemap tilemap;
        public HashSet<(CollectibleType, int, int)> roomProps;
        public WorldGenerator worldGenerator;

        public Room(int offsetX, int offsetY, int worldSeedXOffset, int worldSeedZOffset) {
            worldGenerator = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>();
            roomProps = new();
            grassNoise = new float[20, 20];
            roomOffsetX = offsetX;
            roomOffsetY = offsetY;
            GenerateGrassNoise(worldSeedXOffset, worldSeedZOffset);
            GenerateRoomResources();
        }

        private void GenerateGrassNoise(int worldSeedXOffset, int worldSeedZOffset) {
            //Tile dirt = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>().dirt;
            float roomStartX = roomOffsetX * 20 + worldSeedXOffset;
            float roomStartZ = roomOffsetY * 20 + worldSeedZOffset;

            for (int x = 0; x < 20; x++) {
                for (int z = 0; z < 20; z++) {
                    float grassNoiseValue = Mathf.PerlinNoise((float)(roomStartX + x) / 2, (float)(roomStartZ + z) / 2);
                    grassNoise[x, z] = grassNoiseValue;
                }
            }
        }

        private void GenerateRoomResources() { 
            int waterToGenerate = (int)Random.Range(worldGenerator.minimumRoomResources, (float)worldGenerator.maximumRoomResources);
            for (int i = 0; i < waterToGenerate; i++) {
                roomProps.Add((CollectibleType.Water, (roomOffsetX * 20) + (int)Random.Range(1f, 20f), (roomOffsetY * 20) + (int)Random.Range(1f, 20f)));
            }

            int nutrientsToGenerate = (int)Random.Range(worldGenerator.minimumRoomResources, (float)(worldGenerator.maximumRoomResources - waterToGenerate));
            for (int i = 0; i < nutrientsToGenerate; i++) {
                roomProps.Add((CollectibleType.Nutrients, (roomOffsetX * 20) + (int)Random.Range(1f, 20f), (roomOffsetY * 20) + (int)Random.Range(1f, 20f)));
            }
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

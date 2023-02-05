using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRenderer : MonoBehaviour
{

    public Tilemap tilemap;
    public TileBase dirtTile;
    public TileBase grassTile;
    public TileBase grassDetailTile;

    [Range(0f, 1f)]
    public float grassiness;

    [Range(0f, 1f)]
    public float grassDetail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearRoomTiles(int roomX, int roomZ) {
        int minBoundX = (roomX * 20);
        int minBoundZ = (roomZ * 20);
        int maxBoundX = (roomX * 20) + 20;
        int maxBoundZ = (roomZ * 20) + 20;

        for (int x = minBoundX; x < maxBoundX; x++) {
            for (int z = minBoundZ; z < maxBoundZ; z++) {
                tilemap.SetTile(new Vector3Int(x, z), null);
            } 
        }
    }

    public void FillRoomGround(int roomX, int roomZ, float[,] grassNoise) {
        int minBoundX = (roomX * 20);
        int minBoundZ = (roomZ * 20);
        //int maxBoundX = (roomX * 20) + 20;
        //int maxBoundZ = (roomZ * 20) + 20;

        for (int x = 0; x < 20; x++) {
            for (int z = 0; z < 20; z++) {
                if (grassNoise[x, z] > grassiness) {
                    tilemap.SetTile(new Vector3Int(minBoundX + x, minBoundZ + z), dirtTile);
                } else {
                    tilemap.SetTile(new Vector3Int(minBoundX + x, minBoundZ + z), grassTile);
                    //if (grassNoise[x, z] < grassDetail) {
                    //    tilemap.SetTile(new Vector3Int(minBoundX + x, minBoundZ + z, 1), grassDetailTile);
                   // }
                }
            }
        }
    }
}

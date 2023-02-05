using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRenderer : MonoBehaviour
{

    public Tilemap tilemap;
    public TileBase dirtTile;
    public TileBase grassTile;

    [Range(0f, 1f)]
    public float grassiness;

    // Start is called before the first frame update
    void Start()
    {
        /*for (int x = -10; x < 9; x++) {
            for (int z = -10; z < 9; z++) {
                tilemap.SetTile(new Vector3Int(x, z, 0), dirtTile);
            }
        }*/

        //BoundsInt bounds = new(new Vector3Int(-10, -10, 0), new Vector3Int(20, 20, 0));
        //TileBase[] tiles = new TileBase[400];
        //tilemap.SetTilesBlock(bounds, tiles);
        /*BoundsInt bounds = new(-10, -10, 0, 20, 20, 0);
        TileBase[] tileArray = new TileBase[400];
        for (int i = 0; i < tileArray.Length; i++) {
            tileArray[i] = i % 2 == 0 ? dirtTile : grassTile;
        }
        tilemap.SetTilesBlock(bounds, tileArray);*/
        //tilemap.BoxFill(new Vector3Int(0, 0, 0), grassTile, -10, -10, 40, 40);
        Debug.Log(tilemap.cellBounds);
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
        int maxBoundX = (roomX * 20) + 20;
        int maxBoundZ = (roomZ * 20) + 20;

        for (int x = 0; x < 20; x++) {
            for (int z = 0; z < 20; z++) {
                if (grassNoise[x, z] > grassiness) {
                    tilemap.SetTile(new Vector3Int(minBoundX + x, minBoundZ + z), dirtTile);
                } else {
                    tilemap.SetTile(new Vector3Int(minBoundX + x, minBoundZ + z), grassTile);
                }
            }
        }
    }
}

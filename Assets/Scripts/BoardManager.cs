using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public int width, height;
    public Tile[] tiles;
    public Tile exitTile;
    private Tilemap board;

    void Start()
    {
        GenerateTileMap();
        PlaceExitTile();
    }

    void Update()
    {
        
    }

    public void GenerateTileMap() {
        board = GetComponentInChildren<Tilemap>();
        for (int i = -4; i <= 3; i++) { //x
            for (int j = -4; j <= 3; j++) { //y
                int randomIndex = Random.Range(0, tiles.Length);
                Vector3Int coordinate = new Vector3Int(i, j, 0);
                board.SetTile(coordinate, tiles[randomIndex]);
            }
        }
    }

    public void PlaceExitTile() {
        board.SetTile(new Vector3Int(3, 3, 0), exitTile);
    }
}

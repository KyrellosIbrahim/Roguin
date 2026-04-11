using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public int width, height;
    public Tile[] tiles;
    public Tile exitTile;
    public Tile startTile;
    public Tile wallTile;
    private Tilemap board;

    void Start()
    {
        GenerateTileMap();
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
        PaintBorder();
        PlaceExitTile();
        placeStartTile();
    }

    public void PlaceExitTile() {
        board.SetTile(new Vector3Int(3, 3, 0), exitTile);
    }

    private void placeStartTile() {
        board.SetTile(new Vector3Int(-4, -4, 0), startTile);
    }

    private void PaintBorder()
    {
        for (int i = -5; i <= 4; i++)
        {
            board.SetTile(new Vector3Int(i, -5, 0), wallTile);
            board.SetTile(new Vector3Int(i,  4, 0), wallTile);
            board.SetTile(new Vector3Int(-5, i, 0), wallTile);
            board.SetTile(new Vector3Int( 4, i, 0), wallTile);
        }
    }
}

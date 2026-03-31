using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public int width, height;
    public Tile[] tiles;
    private Tilemap board;

    void Start()
    {
        board = GetComponentInChildren<Tilemap>();
        for (int i = -4; i <= 3; i++) { //x
            for (int j = -4; j <= 3; j++) { //y
                int randomIndex = Random.Range(0, tiles.Length);
                Vector3Int coordinate = new Vector3Int(i, j, 0);
                board.SetTile(coordinate, tiles[randomIndex]);
            }
        }
    }

    void Update()
    {
        
    }

    public void SetTile() {

    }
}

using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase rockTile;
    public TurnManager turnManager;

    public AudioClip[] moveSounds;
    private AudioSource audioSource;

    private Vector3Int gridPos;

    void Start()
    {
        gridPos = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.GetCellCenterWorld(gridPos);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!turnManager.IsPlayerTurn())
            return;

        Vector3Int moveDir = Vector3Int.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            moveDir = new Vector3Int(0, 1, 0);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            moveDir = new Vector3Int(0, -1, 0);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            moveDir = new Vector3Int(-1, 0, 0);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            moveDir = new Vector3Int(1, 0, 0);

        if (moveDir != Vector3Int.zero)
        {
            TryMove(moveDir);
        }
    }

    void TryMove(Vector3Int dir)
    {
        Vector3Int newPos = gridPos + dir;

        if (IsWalkable(newPos))
        {
            gridPos = newPos;
            transform.position = tilemap.GetCellCenterWorld(gridPos);

            // 🔊 Play random footstep
            if (moveSounds.Length > 0 && audioSource != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                int index = Random.Range(0, moveSounds.Length);
                audioSource.PlayOneShot(moveSounds[index]);
            }

            turnManager.EndPlayerTurn();
        }
    }

    bool IsWalkable(Vector3Int pos)
    {
        TileBase tile = tilemap.GetTile(pos);
        return tile != rockTile;
    }
}
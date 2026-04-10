using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Pengie : MonoBehaviour
{
    public float moveTime = 0.2f;
    private bool isMoving = false;

    public Tilemap tilemap;
    public TileBase rockTile;

    // UI / score
    public MoveCounter moveCounter;

    // enemies
    public EnemyMovement polarBear;
    public EnemyMovement seal;

    private Vector3Int gridPos;

    // world bounds (exact requirement)
    private Vector2 minWorld = new Vector2(-3.5f, -3.5f);
    private Vector2 maxWorld = new Vector2(3.5f, 3.5f);

    void Start()
    {
        // FORCE START POSITION
        transform.position = new Vector3(-3.5f, -3.5f, -1);

        gridPos = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.GetCellCenterWorld(gridPos);

        GetComponent<SpriteRenderer>().sortingOrder = 5;
    }

    void Update()
    {
        if (isMoving || Keyboard.current == null)
            return;

        Vector3Int dir = Vector3Int.zero;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            dir = Vector3Int.up;
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            dir = Vector3Int.down;
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            dir = Vector3Int.left;
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            dir = Vector3Int.right;

        if (dir != Vector3Int.zero)
            StartCoroutine(Move(dir));
    }

    System.Collections.IEnumerator Move(Vector3Int dir)
    {
        isMoving = true;

        Vector3Int newPos = gridPos + dir;
        Vector3 worldPos = tilemap.GetCellCenterWorld(newPos);

        // WORLD BOUNDARY CHECK
        if (worldPos.x < minWorld.x || worldPos.x > maxWorld.x ||
            worldPos.y < minWorld.y || worldPos.y > maxWorld.y)
        {
            isMoving = false;
            yield break;
        }

        // TILE CHECK (rock)
        TileBase targetTile = tilemap.GetTile(newPos);
        if (targetTile == rockTile)
        {
            isMoving = false;
            yield break;
        }

        Vector3 start = transform.position;
        Vector3 end = worldPos;

        float t = 0f;

        while (t < moveTime)
        {
            transform.position = Vector3.Lerp(start, end, t / moveTime);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        gridPos = newPos;

        //ADD MOVE COUNT HERE
        if (moveCounter != null)
            moveCounter.AddMove();

        isMoving = false;

        // trigger enemies AFTER penguin moves
        if (polarBear != null)
            polarBear.MoveTowardPlayer();

        if (seal != null)
            seal.MoveTowardPlayer();
    }
}
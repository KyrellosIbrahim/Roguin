using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform player;
    public TileBase rockTile;

    private Vector3Int gridPos;

    public float moveTime = 0.2f;
    private bool isMoving = false;

    void Start()
    {
        gridPos = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.GetCellCenterWorld(gridPos);
    }

    public void MoveTowardPlayer()
    {
        if (!isMoving)
            StartCoroutine(Move());
    }

    System.Collections.IEnumerator Move()
    {
        isMoving = true;

        Vector3Int playerCell = tilemap.WorldToCell(player.position);
        Vector3Int diff = playerCell - gridPos;

        Vector3Int dir;

        // choose main direction toward player
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            dir = new Vector3Int((int)Mathf.Sign(diff.x), 0, 0);
        else
            dir = new Vector3Int(0, (int)Mathf.Sign(diff.y), 0);

        // try main move
        Vector3Int tryPos = gridPos + dir;

        // if blocked, try alternatives
        if (!IsWalkable(tryPos))
        {
            Vector3Int alt1 = new Vector3Int(dir.y, dir.x, 0);
            Vector3Int alt2 = new Vector3Int(-dir.y, -dir.x, 0);

            if (IsWalkable(gridPos + alt1))
                tryPos = gridPos + alt1;
            else if (IsWalkable(gridPos + alt2))
                tryPos = gridPos + alt2;
            else
            {
                isMoving = false;
                yield break;
            }
        }

        Vector3 start = transform.position;
        Vector3 end = tilemap.GetCellCenterWorld(tryPos);

        float t = 0f;

        while (t < moveTime)
        {
            transform.position = Vector3.Lerp(start, end, t / moveTime);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        gridPos = tryPos;

        isMoving = false;
    }

    bool IsWalkable(Vector3Int pos)
    {
        TileBase tile = tilemap.GetTile(pos);
        return tile != rockTile;
    }
}
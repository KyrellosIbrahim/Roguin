using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform player;

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

        Vector3Int dir = Vector3Int.zero;

        Vector3Int diff = playerCell - gridPos;

        // simple chase logic (grid-based)
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            dir = new Vector3Int((int)Mathf.Sign(diff.x), 0, 0);
        else
            dir = new Vector3Int(0, (int)Mathf.Sign(diff.y), 0);

        Vector3Int newPos = gridPos + dir;

        Vector3 start = transform.position;
        Vector3 end = tilemap.GetCellCenterWorld(newPos);

        float t = 0f;

        while (t < moveTime)
        {
            transform.position = Vector3.Lerp(start, end, t / moveTime);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        gridPos = newPos;

        isMoving = false;
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform player;
    public TileBase rockTile;

    public float moveTime = 0.2f;
    public int detectionRange = 3;

    public AudioClip[] moveSounds;
    public AudioClip catchSound;

    private Vector3Int gridPos;
    private bool isMoving = false;
    private AudioSource audioSource;

    void Start()
    {
        gridPos = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.GetCellCenterWorld(gridPos);
        audioSource = GetComponent<AudioSource>();
    }

    public void MoveTowardPlayer()
    {
        if (isMoving)
            return;

        Vector3Int playerCell = tilemap.WorldToCell(player.position);
        int distance = Mathf.Abs(playerCell.x - gridPos.x) + Mathf.Abs(playerCell.y - gridPos.y);

        if (distance <= detectionRange)
        {
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        isMoving = true;

        Vector3Int playerCell = tilemap.WorldToCell(player.position);
        Vector3Int diff = playerCell - gridPos;

        Vector3Int dir;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            dir = new Vector3Int((int)Mathf.Sign(diff.x), 0, 0);
        else
            dir = new Vector3Int(0, (int)Mathf.Sign(diff.y), 0);

        Vector3Int tryPos = gridPos + dir;

        // 💀 If enemy reaches player
        if (tryPos == playerCell)
        {
            if (catchSound != null && audioSource != null)
                audioSource.PlayOneShot(catchSound);

            GameManager.Instance.GameOver();
            isMoving = false;
            yield break;
        }

        // obstacle handling
        if (!IsWalkable(tryPos))
        {
            Vector3Int alt1 = new Vector3Int(dir.y, dir.x, 0);
            Vector3Int alt2 = new Vector3Int(-dir.y, -dir.x, 0);

            if (gridPos + alt1 == playerCell || gridPos + alt2 == playerCell)
            {
                if (catchSound != null && audioSource != null)
                    audioSource.PlayOneShot(catchSound);

                GameManager.Instance.GameOver();
                isMoving = false;
                yield break;
            }

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

        //  Play random enemy step sound
        if (moveSounds.Length > 0 && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            int index = Random.Range(0, moveSounds.Length);
            audioSource.PlayOneShot(moveSounds[index]);
        }

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
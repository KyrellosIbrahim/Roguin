using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Pengie : MonoBehaviour
{
    public float moveTime = 0.2f;
    private bool isMoving = false;

    public Tilemap tilemap;
    public TileBase rockTile;
    public TileBase stairTile;

    // UI / score
    public MoveCounter moveCounter;
    public int maxHealth;
    public int currentHealth;
    public GameObject[] hearts;
    public GameObject heartPrefab;
    public GameObject noHeartPrefab;

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

        maxHealth = 3;
        currentHealth = maxHealth;
        hearts = new GameObject[currentHealth];
        for (int heart = 0; heart < currentHealth; heart++)
        {
            Vector3 heartPosition = new Vector3(-6 + (.5f * heart), 3.5f, 5);
            GameObject heartObject = Instantiate(heartPrefab, heartPosition, Quaternion.identity);
            hearts[heart] = heartObject;
        }
    }

    void Update()
    {
        int heartSlot = 0;
        for (int index = heartSlot; index < currentHealth; index++)
        {
            Destroy(hearts[index]);
            hearts[index] = Instantiate(heartPrefab, new Vector3(-6 + (.5f * heartSlot), 3.5f, 5), Quaternion.identity);
            heartSlot++;
        }
        for (int index = heartSlot; index < maxHealth; index++)
        {
            Destroy(hearts[index]);
            hearts[index] = Instantiate(noHeartPrefab, new Vector3(-6 + (.5f * heartSlot), 3.5f, 5), Quaternion.identity);
        }

        //game over
        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
            isMoving = false;
        }

        if (isMoving || Keyboard.current == null)
            return;

        //player movement
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

        //Stairs reached
        if (targetTile == stairTile)
        {
            GameManager.Instance.LoadNextLevel();
            isMoving = false;
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
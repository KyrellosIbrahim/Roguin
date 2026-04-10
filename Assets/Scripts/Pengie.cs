using UnityEngine;
using UnityEngine.InputSystem;

public class Pengie : MonoBehaviour
{
    public float moveTime = 0.2f;
    public float stepSize = 1f;

    private bool isMoving = false;

    private Vector2 minBounds = new Vector2(-3.5f, -3.5f);
    private Vector2 maxBounds = new Vector2(3.5f, 3.5f);

    void Start()
    {
        transform.position = new Vector3(-3.5f, -3.5f, -1);
        GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    void Update()
    {
        if (isMoving || Keyboard.current == null)
            return;

        Vector3 direction = Vector3.zero;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            direction = Vector3.up;
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            direction = Vector3.down;
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            direction = Vector3.left;
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            direction = Vector3.right;

        if (direction != Vector3.zero)
            StartCoroutine(Move(direction));
    }

    System.Collections.IEnumerator Move(Vector3 direction)
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 end = start + (direction * stepSize);

        if (end.x < minBounds.x || end.x > maxBounds.x ||
            end.y < minBounds.y || end.y > maxBounds.y)
        {
            isMoving = false;
            yield break;
        }

        float t = 0f;

        while (t < moveTime)
        {
            transform.position = Vector3.Lerp(start, end, t / moveTime);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        isMoving = false;
    }
}
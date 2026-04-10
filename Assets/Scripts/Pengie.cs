using UnityEngine;
using UnityEngine.InputSystem;

public class Pengie : MonoBehaviour
{
    public float moveTime = 0.2f;
    private bool isMoving = false;
    void Start()
    {
    transform.position = new Vector3(
        Mathf.Round(transform.position.x),
        Mathf.Round(transform.position.y),
        -1);
    }

    void Update()
    {
        if (isMoving) return;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            StartCoroutine(Move(Vector3.up));
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            StartCoroutine(Move(Vector3.down));
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            StartCoroutine(Move(Vector3.left));
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            StartCoroutine(Move(Vector3.right));
    }

    System.Collections.IEnumerator Move(Vector3 direction)
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 end = start + direction;

        float elapsed = 0;

        while (elapsed < moveTime)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / moveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        isMoving = false;
    }
}
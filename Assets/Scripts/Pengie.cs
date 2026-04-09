using UnityEngine;

public class Pengie : MonoBehaviour
{
    public float moveTime = 0.2f;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            StartCoroutine(Move(Vector3.up));
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            StartCoroutine(Move(Vector3.down));
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            StartCoroutine(Move(Vector3.left));
        else if (Input.GetKeyDown(KeyCode.RightArrow))
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
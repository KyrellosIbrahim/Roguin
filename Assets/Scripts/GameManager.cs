using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;
    public BoardManager boardManager;
    public string gameOverSceneName = "GameOver";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");

        if (player != null)
            player.SetActive(false);

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameOverSceneName);
    }

    public void NextLevel()
    {
        // Regenerate the board
        if (boardManager != null)
        {
            boardManager.GenerateTileMap();
            boardManager.PlaceExitTile();
        }

        // Reset player position and health
        if (player != null)
        {
            Pengie pengie = player.GetComponent<Pengie>();
            if (pengie != null)
                pengie.ResetPlayer();
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Victory");
    }
}
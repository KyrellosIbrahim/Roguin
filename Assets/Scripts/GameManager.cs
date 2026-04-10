using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;
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
}
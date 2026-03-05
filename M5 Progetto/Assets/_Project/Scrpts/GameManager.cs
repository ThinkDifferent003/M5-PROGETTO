using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance;
    [SerializeField] private GameObject _gameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (_gameOver != null)
        {
            _gameOver.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (_gameOver != null)
        {
            _gameOver.SetActive(true);
        }
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

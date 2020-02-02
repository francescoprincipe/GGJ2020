using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNewGame : MonoBehaviour {
    public void StartANewGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBackToMainMenu : MonoBehaviour {
    public void GoBackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
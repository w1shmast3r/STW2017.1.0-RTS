using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    /// <summary>
    /// Script for controlling game logic, like levels, menus, etc.
    /// </summary>
	
    void Start() {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
    }

    public void StartStandaloneGame() {
        Debug.Log("Starting Game");
        SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.LoadSceneAsync("Level_01", LoadSceneMode.Additive);
    }

    public void ShowMultiplayerMenu() {
        Debug.Log("Showing multiplayer");
        SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.LoadSceneAsync("MultiplayerMenu", LoadSceneMode.Additive);
    }
}

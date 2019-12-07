using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {

	private const string MAIN_MENU_SCENE = "Menu";
    public static bool gameIsPaused = false;

	public GameObject resumeButton;
	public GameObject restartButton;
	public GameObject exitButton;
    public GameObject inGameMenu;

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
            gameIsPaused = !gameIsPaused;
            if (IsMenuEnabled()) {
				EnableMenu(true);
			} else {
				EnableMenu(false);
			}
		}
	}

	private bool IsMenuEnabled() {
		bool ret = gameIsPaused;
		return ret;
	}

	private void EnableMenu(bool enabled) {
        if (enabled)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        inGameMenu.SetActive(enabled);
        if (MainMenuPlayerPreferences.InLevelCreationMode)
        {
            restartButton.SetActive(false);
        }
	}

	public void OnResumeButtonClicked() {
        gameIsPaused = false;
        EnableMenu(false);
	}

	public void OnRestartButtonClicked() {
		Scene currScene = SceneManager.GetActiveScene();
        gameIsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(currScene.name);
	}

	public void OnExitButtonClicked() {
        Time.timeScale = 1;
        gameIsPaused = false;
        SceneManager.LoadScene(MAIN_MENU_SCENE);
	}
}

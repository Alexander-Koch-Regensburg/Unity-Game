using UnityEngine;
using UnityEngine.SceneManagement;
using SFB;

public class MainMenuButtonHandler : MonoBehaviour
{
    private const string mainScene = "Scenes/MainScene";
    private const string levelEditor = "Scenes/LevelEditor";

    public void NewGameBtn() {
        MainMenuPlayerPreferences.LoadFromJson = false;
        MainMenuPlayerPreferences.InLevelCreationMode = false;
        SceneManager.LoadScene(mainScene);
    }

    public void LoadGameBtn() {
		string[] chosenPaths = StandaloneFileBrowser.OpenFilePanel("Select json file to load.", "", "json", false);
		if (chosenPaths == null || chosenPaths.Length == 0) {
			return;
		}
		string path = chosenPaths[0];
        if(path.Length != 0) {
            MainMenuPlayerPreferences.LoadFromJson = true;
            MainMenuPlayerPreferences.PathToLoad = path;
            MainMenuPlayerPreferences.InLevelCreationMode = false;
            SceneManager.LoadScene(mainScene);
        }
        else {
            Debug.LogError("You have to choose a valid directory. Could not load current game.");
        }
    }

    public void LevelCreationBtn()
    {
        MainMenuPlayerPreferences.LoadFromJson = false;
        MainMenuPlayerPreferences.InLevelCreationMode = true;
        SceneManager.LoadScene(levelEditor);
    }

    public void ExitGameBtn() {
        Application.Quit();
    }
}

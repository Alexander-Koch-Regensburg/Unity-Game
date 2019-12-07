using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameScreen : MonoBehaviour {

	public static EndGameScreen instance;

	public GameObject screenBlocker;
	public TMP_Text endGameText;
	public GameObject restartButton;
	public GameObject mainMenuButton;

	private const string MAIN_MENU_SCENE = "Menu";

	private const string GAME_WON_MESSAGE = "You Won!";
	private const string GAME_LOST_MESSAGE = "You Died!";

	private bool gameEnded;

	private void Awake() {
		instance = this;
	}

	private void Start() {
		ShowMenu(false);
	}

	private void Update() {
		if (PersonController.instance.Enemies.Count == 0) {
			SetGameWon();
		}
	}

	/// <summary>
	/// Notifies the screen the game is lost and displays the corresponding UI elements
	/// </summary>
	public void SetGameLost() {
		if (gameEnded == false) {
			Debug.Log("Game lost triggered");
			endGameText.text = GAME_LOST_MESSAGE;
			ShowMenu(true);
		}
		gameEnded = true;
	}

	/// <summary>
	/// Notifies the screen the game is won and displays the corresponding UI elements
	/// </summary>
	public void SetGameWon() {
		if (gameEnded == false) {
			Debug.Log("Game won triggered");
			endGameText.text = GAME_WON_MESSAGE;
			ShowMenu(true);
		}
		gameEnded = true;
	}

	private void ShowMenu(bool showMenu) {
		screenBlocker.SetActive(showMenu);
		endGameText.gameObject.SetActive(showMenu);
		restartButton.SetActive(showMenu);
		mainMenuButton.SetActive(showMenu);
	}

	public void OnRestartButtonClicked() {
		Scene currScene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(currScene.name);
	}

	public void OnMainMenuButtonClick() {
		SceneManager.LoadScene(MAIN_MENU_SCENE);
	}
}

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class ControllerTest {
	private const string MAIN_SCENE_NAME = "MainScene";

	[SetUp]
	public void Setup() {
		SceneManager.LoadScene(MAIN_SCENE_NAME);
	}

	[UnityTest]
	public IEnumerator LevelController_IsPresent() {
		yield return null;
		Assert.NotNull(LevelController.instance);
	}

	[UnityTest]
	public IEnumerator PersonController_IsPresent() {
		yield return null;
		Assert.NotNull(PersonController.instance);
	}

	[UnityTest]
	public IEnumerator TilemapController_IsPresent() {
		yield return null;
		Assert.NotNull(TilemapController.instance);
	}
}

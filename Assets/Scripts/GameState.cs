using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

	void OnEnable() {
		EventManager.StartListening("GameOver", GameOver);
	}

	void OnDisable() {
		EventManager.StopListening("GameOver", GameOver);
	}

	private Transition _transition = null;

	void Awake() {
		_transition = GetComponent<Transition>();
	}

	public void OnMenuSelect(int scene) {
		StartCoroutine("Transition", scene);
		_transition.In();
	}


	public void GameOver() {
		StartCoroutine("Transition", -2);
		_transition.Out();
	}


	IEnumerator Transition(int scene) {
		yield return new WaitForSeconds(1.0f);
		switch (scene) {
		case -1: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); break;
		case 0: SceneManager.LoadScene(0); break;
		case 1: SceneManager.LoadScene(1); break;
		default: break;
		}
	}
}

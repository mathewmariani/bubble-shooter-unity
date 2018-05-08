using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	
	private Transition _transition = null;

	void Awake() {
		_transition = GetComponent<Transition>();
	}

	public void OnMenuSelect(int scene) {
		StartCoroutine("Transition", scene);
		_transition.Out();
	}

	IEnumerator Transition(int scene) {
		yield return new WaitForSeconds(1.0f);
		switch (scene) {
		case 0: SceneManager.LoadScene(0); break;
		case 2: SceneManager.LoadScene(2); break;
		case 3: SceneManager.LoadScene(3); break;
		default:break;
		}
	}
}

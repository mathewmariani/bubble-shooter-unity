using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour {

	void OnEnable() {
		EventManager.StartListening("GameOver", GameOver);
	}

	void OnDisable() {
		EventManager.StopListening("GameOver", GameOver);
	}

	public void GameOver() {
		for(int i = 0; i < transform.childCount; i++) {
			var child = transform.GetChild(i).gameObject;
			if(child != null) {
				child.SetActive(true);
			}
		}
	}
}

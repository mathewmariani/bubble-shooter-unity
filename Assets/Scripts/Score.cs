using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	// variables
	private int value = 0;

	// components
	private Text _text;

	// listeners
	private UnityAction scoreListener;

	void Awake() {
		// components
		_text = GetComponent<Text>();

		// listeners
		scoreListener = new UnityAction (UpdateScore);
	}

	void Start() {
		_text.text = ("SCORE: " + value);
	}


	void OnEnable() {
		EventManager.StartListening("ScoreUpdated", UpdateScore);
	}

	void OnDisable() {
		EventManager.StopListening("ScoreUpdated", UpdateScore);
	}

	void UpdateScore() {
		value = value + 100;
		_text.text = ("SCORE: " + value);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shifts : MonoBehaviour {

	private Vector3 shiftOffset = new Vector3(0.0f, -0.24f, 0.0f);

	void OnEnable() {
		EventManager.StartListening("Shift", Shift);
	}

	void OnDisable() {
		EventManager.StopListening("Shift", Shift);
	}

	public void Shift() {
		transform.position += shiftOffset;
	}
}

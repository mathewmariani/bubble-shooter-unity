using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play : MonoBehaviour {

    private ParticleSystem[] _particleSystems = null;

	void Awake() {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
	}
	
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.E)) {
            foreach(var p in _particleSystems) {
                p.Play();
            }
        }
	}
}

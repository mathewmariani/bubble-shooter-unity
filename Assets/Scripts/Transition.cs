using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {

	// variables
	public float transitionTime = 1.0f; 
	public bool transitionOutOnStart = false;
	public bool transitionInOnStart = false;

	// materials
	public Material TransitionMaterial;

	void OnRenderImage(RenderTexture src, RenderTexture dst) {
		if (TransitionMaterial != null) {
			Graphics.Blit(src, dst, TransitionMaterial);
		}
	}

	void Start() {
		if (transitionInOnStart) {
			In();
		}
		if (transitionOutOnStart) {
			Out();
		}
	}

	IEnumerator _TransitionOut(float delay) {
		bool fadingOut = (delay < 0.0f);
		float fadingOutSpeed = (1.0f / delay); 
		float cutoffValue = 0.0f;
	
		while ((cutoffValue >= 0.0f && fadingOut)||(cutoffValue <= 1.0f && !fadingOut)) {
			cutoffValue += Time.deltaTime * fadingOutSpeed; 
			TransitionMaterial.SetFloat("_Cutoff", cutoffValue);
			yield return null; 
		}
	}

	IEnumerator _TransitionIn(float delay) {
		bool fadingOut = (delay < 0.0f);
		float fadingOutSpeed = (1.0f / delay); 
		float cutoffValue = TransitionMaterial.GetFloat("_Cutoff");

		while ((cutoffValue >= 0.0f && !fadingOut)||(cutoffValue <= 1.0f && fadingOut)) {
			cutoffValue -= Time.deltaTime * fadingOutSpeed; 
			TransitionMaterial.SetFloat("_Cutoff", cutoffValue);
			yield return null; 
		}
	}

	public void In() {
		StopAllCoroutines(); 
		StartCoroutine("_TransitionIn", transitionTime); 
	}

	public void Out() {
		StopAllCoroutines(); 
		StartCoroutine("_TransitionOut", transitionTime); 		
	}
}

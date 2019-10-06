using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringObject : MonoBehaviour {
	public Text scoreText;

	public int value;

	public bool hitable = true;
	public bool onehit = false;

	private int score;

	public void OnTriggerEnter(Collider other) {
		if (hitable && other.tag == "Axe" && other.GetComponent<Rigidbody>().isKinematic) {
			score += value;

			scoreText.text = "Score: " + score;
			if (onehit) {
				hitable = false;
			}
		}

	}

	// Update is called once per frame
	void Update() {

	}
}

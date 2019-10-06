using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WarpBall : MonoBehaviour {
	public GameObject warpable;
	public bool? growing;

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Axe") {
			warpable = other.gameObject;
		}
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}

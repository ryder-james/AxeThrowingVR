using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {
	private Vector3 startPos;
	private Quaternion startRot;

	// Start is called before the first frame update
	void Start() {
		Transform t = gameObject.transform;
		startPos = new Vector3(t.position.x, t.position.y, t.position.z);
		startRot = new Quaternion(t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w);
	}

	// Update is called once per frame
	void Update() {
		if (gameObject.transform.position.y < -5) {
			gameObject.transform.rotation = startRot;
			gameObject.transform.position = startPos;
		}
	}
}

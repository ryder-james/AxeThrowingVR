using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStick : MonoBehaviour {
	public float stickVelocity;

	private Rigidbody body;

	private GameObject stuckObject;
	// Start is called before the first frame update
	void Start() {
		body = GetComponent<Rigidbody>();
	}

	public void OnTriggerEnter(Collider other) {
		if (body.velocity.magnitude > stickVelocity && other.tag == "Stickable") {
			Physics.IgnoreCollision(GetComponent<BoxCollider>(), other);
			body.isKinematic = true;
			stuckObject = other.gameObject;
		}
	}

	public void Unstick() {
		if (stuckObject) {
			Physics.IgnoreCollision(GetComponent<BoxCollider>(), stuckObject.GetComponent<Collider>(), false);
			body.isKinematic = false;
			stuckObject = null;
		}
	}

	// Update is called once per frame
	void Update() {
	}
}

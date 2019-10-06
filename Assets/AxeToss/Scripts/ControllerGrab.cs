using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrab : MonoBehaviour {
	public SteamVR_Input_Sources handType;
	public SteamVR_Behaviour_Pose controllerPose;
	public SteamVR_Action_Boolean grabPinch, grabGrip;

	public GameObject warpBallPrefab;

	private GameObject collidingObject, objectInHand, warpBall;
	private Vector3 hitPoint;

	void Start() {
		warpBall = Instantiate(warpBallPrefab);
	}

	void Update() {
		WarpBall warpComponent = warpBall.GetComponent<WarpBall>();

		if (warpComponent.warpable != null) {
			GameObject warpable = warpComponent.warpable;
			Unstick(warpable);
			warpable.transform.position = controllerPose.transform.position;
			warpComponent.warpable = null;
			warpComponent.growing = null;
		}

		if (warpComponent.growing != null && (bool) warpComponent.growing) {
			if (warpBall.transform.localScale.x < 2) {
				warpBall.transform.localScale *= 1.05f;
			} else {
				warpComponent.growing = false;
			}
		} else if (warpComponent.growing != null && (bool) !warpComponent.growing) {
			if (warpBall.transform.localScale.x > 0.2) {
				warpBall.transform.localScale *= 0.8f;
			} else {
				warpComponent.growing = null;
			}
		} else if (warpComponent.growing == null && warpBall.activeSelf) {
			warpBall.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			warpBall.SetActive(false);
		}

		if (grabPinch.GetLastStateDown(handType)) {
			if (collidingObject) {
				GrabObject();
			}
		}

		if (!objectInHand && grabGrip.GetLastStateDown(handType)) {
			SummonAxe();
		}

		if (grabPinch.GetLastStateUp(handType)) {
			if (objectInHand) {
				ReleaseObject();
			}
		}
	}

	private void Unstick(GameObject obj) {
		ProjectileStick comp;
		if (obj.TryGetComponent<ProjectileStick>(out comp)) {
			comp.Unstick();
		}
	}

	private void SummonAxe() {
		RaycastHit hit;

		if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100)) {
			hitPoint = hit.point;
			WarpBall();
		}
	}

	private void WarpBall() {
		warpBall.SetActive(true);
		warpBall.transform.position = hitPoint;

		warpBall.GetComponent<WarpBall>().growing = true;
	}

	public void OnTriggerEnter(Collider other) {
		if (other.tag != "Axe") {
			return;
		}
		SetCollidingObject(other);
	}

	public void OnTriggerStay(Collider other) {
		if (other.tag != "Axe") {
			return;
		}
		SetCollidingObject(other);
	}

	public void OnTriggerExit(Collider other) {
		if (!collidingObject)
			return;

		collidingObject = null;
	}

	private void GrabObject() {
		//FixedJoint joint;

		Unstick(collidingObject);

		objectInHand = collidingObject;
		collidingObject = null;

		objectInHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		//objectInHand.transform.position = controllerPose.transform.position + new Vector3(0, 0.3f, 0);
		//objectInHand.transform.LookAt(controllerPose.transform.forward);

		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
	}

	private void ReleaseObject() {
		if (GetComponent<FixedJoint>()) {
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());

			objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity() * 1.2f;
			objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity() * 4f;
		}

		objectInHand = null;
	}

	private FixedJoint AddFixedJoint() {
		FixedJoint joint = gameObject.AddComponent<FixedJoint>();
		joint.breakForce = 20000;
		joint.breakTorque = 20000;
		return joint;
	}

	private void SetCollidingObject(Collider obj) {
		if (collidingObject || !obj.GetComponent<Rigidbody>()) {
			return;
		}

		collidingObject = obj.gameObject;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class laser : MonoBehaviour
{


	public SteamVR_Input_Sources handType;
	public SteamVR_Behaviour_Pose controllerPose;
	public SteamVR_Action_Boolean teleportAction;

	public GameObject laserPrefab;
	private GameObject laserb;
	private Transform laserTransform;
	private Vector3 hitPoint;

	public Transform cameraRigTransform;
	public GameObject teleportReticlePrefab;
	private GameObject reticle;
	private Transform teleportReticleTransform;
	public Transform headTransform;
	public Vector3 teleportReticleOffset;
	public LayerMask teleportMask;
	private bool shouldTeleport;


	// Start is called before the first frame update
	void Start() {
		laserb = Instantiate(laserPrefab);
		laserTransform = laserb.transform;

		reticle = Instantiate(teleportReticlePrefab);
		teleportReticleTransform = reticle.transform;

	}

	// Update is called once per frame
	void Update() {
		if (teleportAction.GetState(handType)) {
			RaycastHit hit;

			if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100, teleportMask)) {
				hitPoint = hit.point;
				ShowLaser(hit);
				reticle.SetActive(true);
				teleportReticleTransform.position = hitPoint + teleportReticleOffset;
				shouldTeleport = true;
			}
		} else {
			laserb.SetActive(false);
			reticle.SetActive(false);

		}

		if (teleportAction.GetStateUp(handType) && shouldTeleport) {
			Teleport();
		}

	}

	private void ShowLaser(RaycastHit hit) {
		laserb.SetActive(true);
		laserTransform.position = Vector3.Lerp(controllerPose.transform.position, hitPoint, .5f);
		laserTransform.LookAt(hitPoint);
		laserTransform.localScale = new Vector3(laserTransform.localScale.x,
												laserTransform.localScale.y,
												hit.distance);
	}

	private void Teleport() {
		shouldTeleport = false;
		reticle.SetActive(false);
		Vector3 difference = cameraRigTransform.position - headTransform.position;
		difference.y = 0;
		cameraRigTransform.position = hitPoint + difference;
	}

}

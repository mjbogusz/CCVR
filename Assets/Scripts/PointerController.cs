using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour {
	private float maxSpeed = 20f;
	private float distanceAcceleration = 5f;

	private bool aimingMode = false;
	private float angle = 0f;
	private float height = 1f;
	private float distance = 1f;

	public GameObject dataCanvas;
	public GameObject player;

	// Use this for initialization
	void Start () {
		maxSpeed *= Time.fixedDeltaTime;
		distanceAcceleration *= Time.fixedDeltaTime;
	}

	// Update is called once per frame
	void Update () {
		// Display data canvas if fire4 is pressed
		if (Input.GetButton("Fire4")) {
			// Set canvas to visible
			dataCanvas.SetActive(true);

			// Rotate canvas to be perpendicular to player position
			Vector3 playerPosition = player.transform.position;
			Vector3 pointerPosition = transform.position;
			float canvasRotation = Mathf.Atan2(pointerPosition.x - playerPosition.x, pointerPosition.z - playerPosition.z) * 180 / Mathf.PI;
			dataCanvas.transform.eulerAngles = Quaternion.Euler(0f, canvasRotation, 0f).eulerAngles;

			// End - don't allow other actions as this button also sends "fire2"...
			return;
		} else {
			dataCanvas.SetActive(false);
		}

		// Toggle aiming/movement mode
		if (Input.GetButtonUp("Fire5")) {
			aimingMode = !aimingMode;
		}

		if (!aimingMode) {
			return;
		}

		// Reset pointer position
		if (Input.GetButtonUp("Fire3")) {
			angle = 0f;
			height = 1f;
			distance = 1f;
		}

		// vertical (up/down): move up/down
		float vertical = Input.GetAxis("Vertical");
		// horizontal (left/right): rotate around camera
		float horizontal = Input.GetAxis("Horizontal");
		float distanceChange = 0f;
		if (Input.GetButton("Fire1")) {
			distanceChange += 1f;
		}
		if (Input.GetButton("Fire2")) {
			distanceChange -= 1f;
		}

		///TODO: find player position and camera angles
		///TODO: apply changes
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour {
	private static readonly float maxSpeed = 20f;
	private static readonly float distanceAcceleration = 5f * Time.fixedDeltaTime;

	private bool aimingMode = false;
	private float angle = 0f;
	private float height = 1f;
	private float distance = 1f;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		// Toggle aiming/movement mode
		if (Input.GetButtonUp("Fire4")) {
			aimingMode = !aimingMode;
		}

		// Display data readouts
		if (Input.GetButton("Fire0")) {
			///TODO: display readouts
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

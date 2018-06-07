using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour {
	public GameObject dataCanvas;
	public GameObject player;
	public GameObject playerCamera;

	private bool aimingMode = false;
	private float speedMax = 2f;
	private float accelerationMax = 5f;
	private Vector3 speed;

	// Use this for initialization
	void Start () {
		speedMax *= Time.fixedDeltaTime;
		accelerationMax *= Time.fixedDeltaTime;
	}

	// Update is called once per frame
	void Update () {
		// Rotate canvas to be perpendicular to player/camera position
		transform.rotation = playerCamera.transform.rotation;

		// Display data canvas if fire4 (upper trigger) is pressed
		if (Input.GetButton("Fire4")) {
			// Set canvas to visible
			dataCanvas.SetActive(true);

			// Stop the pointer
			speed = new Vector3(0f, 0f, 0f);

			// End - don't allow other actions as this button also sends "fire2"...
			return;
		} else {
			dataCanvas.SetActive(false);
		}

		// Toggle aiming/movement mode (fire5 / lower trigger)
		if (Input.GetButtonUp("Fire5")) {
			aimingMode = !aimingMode;
		}
		if (!aimingMode) {
			return;
		}

		// Reset pointer position - fire3 / "D"
		if (Input.GetButtonUp("Fire3")) {
			transform.position = player.transform.position + Quaternion.Euler(playerCamera.transform.eulerAngles) * new Vector3(0f, 0f, 1f);
		}

		// vertical (up/down): move up/down
		// horizontal (left/right): rotate around camera
		// distance: self-explanatory
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		float distanceChange = 0f;
		if (Input.GetButton("Fire1")) {
			distanceChange += 0.5f;
		}
		if (Input.GetButton("Fire2")) {
			distanceChange -= 0.5f;
		}

		Vector3 speedTarget = new Vector3(horizontal, vertical, distanceChange);
		Vector3 acceleration = (speedTarget - speed) * Time.fixedDeltaTime;
		if (acceleration.magnitude > accelerationMax) {
			acceleration *= (accelerationMax / acceleration.magnitude);
		}

		speed += acceleration;
		transform.Translate(speed * speedMax);
	}
}

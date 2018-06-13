using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour {
	public GameObject dataCanvas;
	public GameObject playerObject;
	public GameObject playerCamera;

	public float speedMax;
	public float accelerationMax;

	public string displayDataInputButton;
	public string movingModeInputButton;
	public string resetPointerInputButton;
	public string increasePointerDistanceInputButton;
	public string decreasePointerDistanceInputButton;

	private bool aimingMode = false;
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

		// Display data canvas if upper trigger is pressed
		if (Input.GetButton(displayDataInputButton)) {
			// Set canvas to visible
			dataCanvas.SetActive(true);

			// Stop the pointer
			speed = new Vector3(0f, 0f, 0f);

			// End - don't allow other actions as this button also sends "fire2"...
			return;
		} else {
			dataCanvas.SetActive(false);
		}

		// Toggle aiming/movement mode (lower trigger)
		if (Input.GetButtonUp(movingModeInputButton)) {
			aimingMode = !aimingMode;
		}
		if (!aimingMode) {
			return;
		}

		// Reset pointer position
		if (Input.GetButtonUp(resetPointerInputButton)) {
			transform.position = playerObject.transform.position + Quaternion.Euler(playerCamera.transform.eulerAngles) * new Vector3(0f, 0f, 1f);
		}

		// vertical (up/down): move up/down
		// horizontal (left/right): rotate around camera
		// distance: self-explanatory
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		float distanceChange = 0f;
		if (Input.GetButton(increasePointerDistanceInputButton)) {
			distanceChange += 1f;
		}
		if (Input.GetButton(decreasePointerDistanceInputButton)) {
			distanceChange -= 1f;
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

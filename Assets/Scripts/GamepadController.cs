using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : MonoBehaviour {
	public float speedMax;
	public float accelerationMax;
	public string movingModeInputButton;

	public Transform mainCameraTransform;

	private Vector3 speed = new Vector3(0f, 0f, 0f);
	private bool movingMode = true;

	// Use this for initialization
	void Start() {
		speedMax *= Time.fixedDeltaTime;
		accelerationMax *= Time.fixedDeltaTime;
	}

	// Update is called once per frame
	void Update() {
		if (Input.touchCount > 0) {
			UnityEngine.XR.InputTracking.Recenter();
			#if UNITY_EDITOR
				GvrEditorEmulator.Instance.Recenter();
			#endif
		}

		if (Input.GetButtonUp(movingModeInputButton)) {
			movingMode = !movingMode;
		}

		Vector3 speedTarget = new Vector3(0f, 0f, 0f);

		if (movingMode) {
			float vertical = Input.GetAxis("Vertical");
			float horizontal = Input.GetAxis("Horizontal");

			float cameraAngle = mainCameraTransform.eulerAngles.y;
			speedTarget = Quaternion.Euler(0, cameraAngle, 0) * new Vector3(horizontal, 0, vertical);
		}

		Vector3 acceleration = (speedTarget - speed) * Time.fixedDeltaTime;
		if (acceleration.magnitude > accelerationMax) {
			acceleration *= (accelerationMax / acceleration.magnitude);
		}

		speed += acceleration;
		transform.Translate(speed * speedMax);
	}
}

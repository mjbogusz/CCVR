using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : MonoBehaviour {
	private static readonly float maxSpeed = 1.0f;
	private static readonly float acceleration = 3.0f * Time.fixedDeltaTime;
	private float speedX = 0f;
	private float speedZ = 0f;
	private bool movingMode = true;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp("Fire3")) {
			movingMode = !movingMode;
		}

		if (movingMode) {
			float vertical = Input.GetAxis("Vertical");
			float horizontal = Input.GetAxis("Horizontal");

			float speedDiffX = horizontal - speedX;
			if (speedDiffX > acceleration) {
				speedX += acceleration;
			} else if (speedDiffX < -acceleration) {
				speedX -= acceleration;
			} else {
				speedX += speedDiffX;
			}

			float speedDiffZ = vertical - speedZ;
			if (speedDiffZ > acceleration) {
				speedZ += acceleration;
			} else if (speedDiffZ < -acceleration) {
				speedZ -= acceleration;
			} else {
				speedZ += speedDiffZ;
			}
		} else {
			speedX = 0;
			speedZ = 0;
		}

		Vector3 translationVector = new Vector3(speedX, 0f, speedZ) * Time.fixedDeltaTime * maxSpeed;
		translationVector = Quaternion.Euler(transform.Find("mainCamera").eulerAngles) * translationVector;
		transform.Translate(translationVector);
	}
}

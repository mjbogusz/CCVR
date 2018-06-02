using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : MonoBehaviour {
	private static readonly float maxSpeed = 1.0f;
	private static readonly float acceleration = 2.0f;
	private float speedX = 0f;
	private float speedZ = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		float speedDiffX = horizontal - speedX;
		if (speedDiffX > acceleration * Time.fixedDeltaTime) {
			speedX += acceleration * Time.fixedDeltaTime;
		} else if (speedDiffX < -acceleration * Time.fixedDeltaTime) {
			speedX -= acceleration * Time.fixedDeltaTime;
		} else {
			speedX += speedDiffX;
		}

		float speedDiffZ = vertical - speedZ;
		if (speedDiffZ > acceleration * Time.fixedDeltaTime) {
			speedZ += acceleration * Time.fixedDeltaTime;
		} else if (speedDiffZ < -acceleration * Time.fixedDeltaTime) {
			speedZ -= acceleration * Time.fixedDeltaTime;
		} else {
			speedZ += speedDiffZ;
		}

		Vector3 translationVector = new Vector3(speedX, 0f, speedZ) * Time.fixedDeltaTime * maxSpeed;
		translationVector = Quaternion.Euler(transform.Find("Main Camera").eulerAngles) * translationVector;
		transform.Translate(translationVector);
	}
}

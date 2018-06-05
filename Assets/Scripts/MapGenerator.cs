using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float wallHeight = 2.5f;
		Vector2[] wallPoints = {
			new Vector2(5.0f, 5.0f),
			new Vector2(-3.0f, 5.0f),
			new Vector2(-5.0f, 3.0f),
			new Vector2(-5.0f, -5.0f),
			new Vector2(3.0f, -5.0f),
			new Vector2(3.0f, -3.0f),
			new Vector2(5.0f, -3.0f),
		};

		GameObject map = GameObject.Find("map");
		Material wallMaterial = Resources.Load("Materials/WallMaterial", typeof(Material)) as Material;

		for (int i = 0; i < wallPoints.Length; i++) {
			Vector2 p1 = wallPoints[i];
			Vector2 p2 = wallPoints[(i + 1) % wallPoints.Length];

			Vector3 wallPosition = new Vector3((p1.x + p2.x) / 2, wallHeight / 2, (p1.y + p2.y) / 2);
			float wallLength = Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
			float wallRotation = Mathf.Atan2(p2.x - p1.x, p2.y - p1.y) * 180 / Mathf.PI;

			GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
			wall.name = "Wall" + i.ToString();
			wall.transform.SetParent(map.transform);
			wall.transform.position = wallPosition;
			wall.transform.localScale = new Vector3(0.01f, wallHeight, wallLength);
			wall.transform.eulerAngles = new Vector3(0f, wallRotation, 0f);

			Material wallMaterialScaled = new Material(wallMaterial);
			wallMaterialScaled.mainTextureScale = new Vector2(wallLength, wallHeight);
			Renderer wallRenderer = wall.GetComponent<Renderer>();
			wallRenderer.material = wallMaterialScaled;
		}

		Transform ceilingTransform = map.transform.Find("Ceiling");
		ceilingTransform.position = new Vector3(0f, wallHeight, 0f);

		Transform ceilingLightTransform = map.transform.Find("Ceiling Light");
		ceilingLightTransform.position = new Vector3(0f, wallHeight - 0.1f, 0f);
	}

	// Update is called once per frame
	void Update () {

	}
}

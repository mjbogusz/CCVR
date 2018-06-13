using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
	public GameObject mapObject;
	public Material wallMaterial;
	public Material sensorMaterial;
	public GameObject playerObject;

	public string serverURL;
	public int serverPort;
	public string serverMapFilename;

	public string updateInputButton;

	private float wallHeight;
	private List<Vector2> wallPoints;

	void Start() {
		StartCoroutine(UpdateMap());
	}

	// Update is called once per frame
	void Update() {
		// Update map
		if (Input.GetButtonUp(updateInputButton)) {
			StartCoroutine(UpdateMap());
		}
	}

	// Use this for initialization
	IEnumerator UpdateMap() {
		wallHeight = 0f;
		wallPoints = new List<Vector2>();

		// Retrieve map data
		string mapURI = "http://" + serverURL + ":" + serverPort.ToString() + "/" + serverMapFilename;
		WWW mapRequest = new WWW(mapURI);
		yield return mapRequest;
		string mapData = mapRequest.text;

		if (mapRequest.error != null) {
			mapData = "";
		}

		// Default map data if none passed
		if (mapData.Length < 2) {
			mapData = "2.5\n";
			mapData += "5 5\n";
			mapData += "-3 5\n";
			mapData += "-5 3\n";
			mapData += "-5 -5\n";
			mapData += "3 -5\n";
			mapData += "3 -3\n";
			mapData += "5 -3\n";
		}

		// Parse map data
		NumberFormatInfo format = CultureInfo.InvariantCulture.NumberFormat;
		string[] mapEntries = mapData.Split('\n');
		int i = 0;
		for (; i < mapEntries.Length; i++) {
			if (mapEntries[i].Length < 1) {
				continue;
			}
			wallHeight = float.Parse(mapEntries[i], format);
			break;
		}
		i++;
		for (; i < mapEntries.Length; i++) {
			if (mapEntries[i].Length < 1) {
				continue;
			}
			string[] p = mapEntries[i].Split(' ');
			float x = float.Parse(p[0], format);
			float z = float.Parse(p[1], format);
			wallPoints.Add(new Vector2(x, z));
		}

		// Create wall objects
		for (i = 0; i < wallPoints.Count; i++) {
			Vector2 p1 = wallPoints[i];
			Vector2 p2 = wallPoints[(i + 1) % wallPoints.Count];

			Vector3 wallPosition = new Vector3((p1.x + p2.x) / 2, wallHeight / 2, (p1.y + p2.y) / 2);
			float wallLength = Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
			float wallRotation = Mathf.Atan2(p2.x - p1.x, p2.y - p1.y) * 180 / Mathf.PI;

			GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
			wall.name = "Wall" + i.ToString();
			wall.transform.SetParent(mapObject.transform);
			wall.transform.position = wallPosition;
			wall.transform.localScale = new Vector3(0.01f, wallHeight, wallLength);
			wall.transform.eulerAngles = new Vector3(0f, wallRotation, 0f);

			Material wallMaterialScaled = new Material(wallMaterial);
			wallMaterialScaled.mainTextureScale = new Vector2(wallLength, wallHeight);
			Renderer wallRenderer = wall.GetComponent<Renderer>();
			wallRenderer.material = wallMaterialScaled;
		}

		Transform ceilingTransform = mapObject.transform.Find("Ceiling");
		ceilingTransform.position = new Vector3(0f, wallHeight, 0f);

		Transform ceilingLightTransform = mapObject.transform.Find("Ceiling Light");
		ceilingLightTransform.position = new Vector3(0f, wallHeight - 0.1f, 0f);

		playerObject.transform.position = new Vector3(0f, 1.8f, 0f);
	}
}

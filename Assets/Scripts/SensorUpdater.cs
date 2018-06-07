using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SensorUpdater : MonoBehaviour {
	public string serverURL;
	public int serverPort;
	public string serverSensorFilename;

	public GameObject map;
	public Material sensorMaterial;
	public Text pointerText;

	private List<GameObject> sensorObjects;
	private string sensorURI;

	private struct SensorData {
		public Vector3 position;
		public float temperature;
		public float brightness;
		public float humidity;
	}

	// Use this for initialization
	void Start () {
		sensorObjects = new List<GameObject>();
		sensorURI = "http://" + serverURL + ":" + serverPort.ToString() + "/" + serverSensorFilename;

		StartCoroutine("Timer");
	}

	// Update is called once per frame
	void Update () {}

	private IEnumerator Timer() {
		while(true) {
			// Timer: 1s
			yield return new WaitForSeconds(1f);

			// Get new data
			WWW sensorRequest = new WWW(sensorURI);
			yield return sensorRequest;

			// Split data by newline
			string[] sensorStrings = sensorRequest.text.Split('\n');

			List<SensorData> sensors = new List<SensorData>();

			for (int i = 0; i < sensorStrings.Length; i++) {
				string[] data = sensorStrings[i].Split(' ');
				if (data.Length <= 1) {
					continue;
				}
				float x = float.Parse(data[0], CultureInfo.InvariantCulture.NumberFormat);
				float y = float.Parse(data[1], CultureInfo.InvariantCulture.NumberFormat);
				float z = float.Parse(data[2], CultureInfo.InvariantCulture.NumberFormat);

				sensors.Add(new SensorData {
					position = new Vector3(x, y, z),
					temperature = float.Parse(data[3], CultureInfo.InvariantCulture.NumberFormat),
					brightness = float.Parse(data[4], CultureInfo.InvariantCulture.NumberFormat),
					humidity = float.Parse(data[5], CultureInfo.InvariantCulture.NumberFormat),
				});
			}

			// Destroy previous sensor objects
			sensorObjects.ForEach(Destroy);

			// Create and place new sensor objects, sum up distances from pointer position
			float weightSum = 0f;
			sensors.ForEach((sensor) => {
				GameObject sensorSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sensorSphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				sensorSphere.transform.SetParent(map.transform);
				Renderer sensorRenderer = sensorSphere.GetComponent<Renderer>();
				sensorRenderer.material = sensorMaterial;

				sensorSphere.transform.position = sensor.position;
				sensorObjects.Add(sensorSphere);

				weightSum += 1 / (sensor.position - transform.position).magnitude;
			});

			// Calculate readings
			float temperature = 0f;
			float brightness = 0f;
			float humidity = 0f;
			sensors.ForEach((sensor) => {
				float distance = (sensor.position - transform.position).magnitude;
				float weight = 1 / distance / weightSum;
				temperature += sensor.temperature * weight;
				brightness += sensor.brightness * weight;
				humidity += sensor.humidity * weight;
			});

			// Set text for text object
			pointerText.text = "Temperature: " + temperature.ToString("0.0") + "°C\n";
			pointerText.text += "Brightness: " + brightness.ToString("0.0") + "lx\n";
			pointerText.text += "Humidity: " + humidity.ToString("0.0") + "%";
		}
	}
}

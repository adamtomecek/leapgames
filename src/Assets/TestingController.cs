using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class TestingController : MonoBehaviour {
	Calibrator calibrator = new Calibrator();
	Controller controller = new Controller();
	Frame frame;
	float timer = 15.0f;
	bool start = false;
	bool end = false;
	bool finished = false;

	float[] magnitudes = new float[5];
	int[] numberOfFingers = new int[5];
	int[] fingersDiff = new int[5];
	List<float> diffs = new List<float>();
	int numberOfMeasurements = 0;

	private Finger.FingerType[] types = new Finger.FingerType[5]; 


	// Use this for initialization
	void Start () {
		types [0] = Finger.FingerType.TYPE_INDEX;
		types [1] = Finger.FingerType.TYPE_MIDDLE;
		types [2] = Finger.FingerType.TYPE_RING;
		types [3] = Finger.FingerType.TYPE_PINKY;
		types [4] = Finger.FingerType.TYPE_THUMB;
	}
	
	// Update is called once per fram;
	void Update () {
		frame = controller.Frame ();
		if (!start && frame.Fingers.Count == 5) {
			start = true;
		}

		if (start) {
			for(int i = 0; i < 5; i++){
				numberOfMeasurements++;
				float last = magnitudes[i];
				FingerList finger = frame.Fingers.FingerType(types[i]);
				if(finger.Count == 0){
					Debug.Log (0);
					continue;
				}

				Vector3 pos = finger[0].TipPosition.ToUnity();
				float newmag = pos.magnitude;

				float diff = Mathf.Abs(newmag - last);
				if(diff > 10){
					fingersDiff[i]++;
				}

				diffs.Add(diff);

				magnitudes[i] = newmag;
				numberOfFingers[i]++;
			}
			Vector3 tip = frame.Fingers.Frontmost.TipPosition.ToUnity();
			timer -= Time.deltaTime;

			if(timer < 0){
				start = false;
				end = true;
			}
		}

		if (end && !finished) {
			finished = true;
			Debug.Log("Pocet snimani:");
			Debug.Log (numberOfMeasurements);

			Debug.Log("Sniman 1 prst:");
			Debug.Log(numberOfFingers[0]);
			Debug.Log(fingersDiff[0]);
			Debug.Log("Sniman 2 prst:");
			Debug.Log(numberOfFingers[1]);
			Debug.Log(fingersDiff[1]);
			Debug.Log("Sniman 3 prst:");
			Debug.Log(numberOfFingers[2]);
			Debug.Log(fingersDiff[2]);
			Debug.Log("Sniman 4 prst:");
			Debug.Log(numberOfFingers[3]);
			Debug.Log(fingersDiff[3]);
			Debug.Log("Sniman 5 prst:");
			Debug.Log(numberOfFingers[4]);
			Debug.Log(fingersDiff[4]);
			Debug.Log("________________");
			Debug.Log("Pocet nalezenych odchylek:");

			Debug.Log (string.Concat(numberOfMeasurements.ToString(), "\n",
			                         numberOfFingers[0], "\n", fingersDiff[0], "\n",
			                         numberOfFingers[1], "\n", fingersDiff[1], "\n",
			                         numberOfFingers[2], "\n", fingersDiff[2], "\n",
			                         numberOfFingers[3], "\n", fingersDiff[3], "\n",
			                         numberOfFingers[4], "\n", fingersDiff[4], "\n"
			           ));

			float avgDiff = 0;
			foreach (float diff in diffs){
				avgDiff += diff;
			}
			
			avgDiff /= diffs.Count;

			Debug.Log ("Prumerna odchylka");
			Debug.Log (avgDiff);
		}

	}
}

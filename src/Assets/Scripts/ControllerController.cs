using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Leap;

public class ControllerController : MonoBehaviour {

	Controller controller;
	Calibrator calibrator;

	Frame frame;
	GameObject[] respawns;

	bool loadedPoints = false;

	string text = "";


	public Transform racket;
	public Transform[] rackets;
	public GameObject line;

	private int finger1 = 0;
	private int finger2 = 1;
	private int finger3 = 2;
	private int finger4 = 3;

	private Finger.FingerType[] types = new Finger.FingerType[4]; 

	void OnGUI(){
		GUIText gtext = GameObject.Find ("Text").GetComponent<GUIText> ();
		gtext.guiText.text = text;

		if (!loadedPoints) {
			text = "Leap Motion and projector not calibrated. Press ESC to run calibrator";
		}

		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.LoadLevel("calibrator");
		}
	}

	// Use this for initialization
	void Start () {
		controller = new Controller ();
		calibrator = new Calibrator ();
		if (calibrator.LoadPoints ())
			loadedPoints = true;

		types [0] = Finger.FingerType.TYPE_INDEX;
		types [1] = Finger.FingerType.TYPE_MIDDLE;
		types [2] = Finger.FingerType.TYPE_RING;
		types [3] = Finger.FingerType.TYPE_PINKY;
		respawns = GameObject.FindGameObjectsWithTag("movable");
	}

	// Update is called once per frame
	void Update () {
		frame = controller.Frame ();
		if (frame.Fingers.Count > 0){
			MoveBox(frame);
		}
	}
	
	void MoveBox(Frame frame){
		Vector3 pos;

		//Debug.Log (angle);
		for(int i = 0; i < 4; i++){
			FingerList fingers = frame.Fingers.FingerType(types[i]);
			pos = fingers[0].TipPosition.ToUnity();
			pos.y = pos.z;
			pos.z = 0;
			Vector2 newPos = calibrator.GetPosition(pos);

			if (newPos.y > -1){
				newPos.y = -1;
			}

			rackets[i].position = newPos;
		}
	}

}

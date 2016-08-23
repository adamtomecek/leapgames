using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class CalibratorController : MonoBehaviour {
	List<Vector3> calibratingPoints = new List<Vector3>();
	Vector2[] points = new Vector2[4];
	
	bool gettingPoint = false;
	float timer = 8.0f;
	int index = 0;
	float calibrationTimer = 5.0f;
	
	bool done = false;
	
	Controller controller;
	Calibrator translator;
	Frame frame;
	
	public MeshRenderer[] objects;
	public Material redColor;
	public Material greenColor;
	
	public GameObject[] respawns;

	void Start(){
		controller = new Controller ();
		translator = new Calibrator ();
	}
	
	// Update is called once per frame
	void Update () {
		frame = controller.Frame ();
		
		if (frame.Fingers.Count > 0) {
			gettingPoint = true;
		} else {
			gettingPoint = false;
		}
		
		if(gettingPoint && index < 3 && !done){
			objects[index].material.color = new Color(1, 0, 0);
			timer -= Time.deltaTime * 3;
			
			if(timer < 0){
				translator.GetCoordinates(calibrationTimer, index, objects[index]);

				if(calibrationTimer < 0){
					calibrationTimer = 5.0f;
					index++;
					if(index == 3){
						done = true;
					}else{
						timer = 8.0f;
						calibrationTimer = 5.0f;
					}
				}
				calibrationTimer -= Time.deltaTime * 3;
			}
		}
		
		if (done){
			Move();
		}
	}

	/* calibration visual controll */
	void Move(){
		Vector3 pos;
		float newX, newY;
		
		float angle = Vector2.Angle (points [0] - points [2], new Vector2 (4, 4));
		
		for(int i = 0; i < frame.Fingers.Count; i++){
			pos = frame.Fingers[i].TipPosition.ToUnity();
			pos.y = pos.z;
			pos.z = 0;
			
			
			respawns[i].transform.position = translator.GetPosition(pos);
		}
	}
	
	void OnGUI(){
		GUIText text = GameObject.Find("Timer").GetComponent<GUIText>();
		if (frame.Fingers.Count == 0 && !done){
			text.guiText.text = "Start calibrating by showing your hand to Leap Motion";
		}else if (!done) {
			if(timer < 0)
				text.guiText.text = calibrationTimer.ToString("#");
			else
				text.guiText.text = timer.ToString("#");
		}else{
			text.guiText.text = "Calibration complete. You can test it now. Restart calibration by pressing ESCape. Finish with SPACE.";
		}
		
		if(Input.GetKeyDown(KeyCode.Escape)){
			timer = 8.0f;
			index = 0;
			gettingPoint = false;
			calibrationTimer = 5.0f;
			calibratingPoints.Clear();
			done = false;
			for(int i = 0; i < objects.Length; i++)
				objects[i].material.color = new Color(1, 1, 1);
		}else if(Input.GetKeyDown(KeyCode.Space)){
			Application.LoadLevel("main");
		}
	}
	/*
	void GetCoordinates(){
		objects[index].material.color = new Color(0, 0, 1);
		if(calibrationTimer < 0){
			gettingPoint = false;
			calibrationTimer = 5.0f;
			
			// count average
			Vector3 point = Vector2.zero;
			foreach (Vector3 pos in calibratingPoints){
				point.x += pos.x;
				point.y += pos.z;
			}
			
			point.x /= calibratingPoints.Count;
			point.y /= calibratingPoints.Count;
			calibratingPoints.Clear();
			points[index] = new Vector2 (point.x, point.y);
			objects[index].material.color = new Color(0, 1, 0);
			index++;
			if (index == 3){
				done = true;
				SaveCoordinates();
			}else
				timer = 5.0f;
			
			return ;
		}
		
		Finger finger = frame.Fingers.Frontmost;
		calibratingPoints.Add(finger.TipPosition.ToUnity());
	}
	*/
	
}

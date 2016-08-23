using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Leap;

public class LaserController : MonoBehaviour {
	Controller controller = new Controller();
	Frame frame;

	GameObject[] respawns;
	
	bool loadedPoints = false;
	Calibrator calibrator = new Calibrator();
	


	public GameObject line;
	
	private int finger1 = 0;
	private int finger2 = 1;
	private int finger3 = 2;
	private int finger4 = 3;
	
	private Finger.FingerType[] types = new Finger.FingerType[4]; 
	
	public LineRenderer lr;
	public EdgeCollider2D collid;

	bool collision = false;

	// Use this for initialization
	void Start () {
		//points = LeapTransaltor.LoadPoints ();
		if(calibrator.LoadPoints())
			loadedPoints = true;

		types [0] = Finger.FingerType.TYPE_INDEX;
		types [1] = Finger.FingerType.TYPE_MIDDLE;
		types [2] = Finger.FingerType.TYPE_RING;
		types [3] = Finger.FingerType.TYPE_PINKY;
	}
	

	// Update is called once per frame
	void Update () {
		frame = controller.Frame ();
		if (frame.Fingers.Count > 0) {
			CreateLaser (frame);
		} else {
			IdleLaser();
		}
	}

	void IdleLaser(){
		audio.Pause ();
		Vector3 pos1 = Vector3.zero;
		Vector3 pos2 = Vector3.zero;

		// line renderer
		lr.SetPosition(1, pos2);
		lr.SetPosition(0, pos1);
		
		// physics
		Vector2[] poses = new Vector2[2];
		poses [1] = pos2;
		poses [0] = pos1;
		collid.points = poses;
		transform.position = new Vector2 (-10.0f, -10.0f);
	}

	
	
	void CreateLaser(Frame frame){
		if(!audio.isPlaying)
			audio.Play ();

		transform.position = new Vector2 (0.0f, 0.0f);
		Vector3 pos;
		float newX, newY;
		
		Vector3 pos1 = Vector3.zero;
		Vector3 pos2 = Vector3.zero;


		FingerList fingers = frame.Fingers.FingerType(types[0]);

		pos = fingers[0].TipPosition.ToUnity();
		pos.y = pos.z;
		pos.z = 0;
		pos1 = calibrator.GetPosition(pos);

		pos = fingers[0].Bone(Bone.BoneType.TYPE_METACARPAL).Center.ToUnity();
		pos.y = pos.z;
		pos.z = 0;
		pos2 = calibrator.GetPosition(pos);

		pos1 = Vector3.Project(new Vector3(0, 150), (pos1 - pos2));

		// check collisions
		RaycastHit2D point = Physics2D.Linecast (pos2, pos1, LayerMask.GetMask("walls"));
		pos1 = point.point;

		// line renderer
		lr.SetPosition(1, pos2);
		lr.SetPosition(0, pos1);

		// physics
		Vector2[] poses = new Vector2[2];
		poses [1] = pos2;
		poses [0] = pos1;
		collid.points = poses;
	}
}
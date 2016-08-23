using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

using System;
using System.IO;


public class Calibrator{
	List<Vector3> calibratingPoints = new List<Vector3>();
	Vector2[] points = new Vector2[4];
	
	Controller controller = new Controller();
	Frame frame;
	

	public void GetCoordinates(float calibrationTimer, int index, MeshRenderer obj){
		frame = controller.Frame ();
		obj.material.color = new Color(0, 0, 1);
		if(calibrationTimer < 0){			
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
			obj.material.color = new Color(0, 1, 0);
			index++;
			if (index == 3){ // if all calibration points are loaded
				SaveCoordinates();
			}
			return ;
		}
		
		Finger finger = frame.Fingers.Frontmost;
		calibratingPoints.Add(finger.TipPosition.ToUnity());
	}
	
	public void SaveCoordinates(){
		string fileName = "coordinates";

		File.Delete (fileName);
		using (StreamWriter sw = new StreamWriter(fileName)) 
		{
			for(int i = 0; i < points.Length; i++){
				sw.WriteLine(points[i].ToString());
			}
		}
	}

	public Vector2 GetPosition(Vector2 pos){
		float newX = (pos.x - (points[1].x + points[0].x) * 0.5f) / ((points[1].x - points[0].x) * 0.5f);
		float newY = (pos.y - (points[0].y + points[2].y) * 0.5f) / ((points[0].y - points[2].y) * 0.5f);
		Vector3 newPos = Vector2.zero;
		newPos.x = newX * 4.0f;
		newPos.y = newY * 4.0f;


		return newPos;
	}

	public bool LoadPoints(){
		if (!File.Exists ("coordinates")) {
			return false;
		}
		
		char[] toTrim = {'(', ')'};
		string[] lines = File.ReadAllLines("coordinates");
		for (int i = 0; i < lines.Length; i++) {
			string[] positions = lines[i].Trim(toTrim).Split(',');
			points[i] = new Vector2(
				float.Parse(positions[0]),
				float.Parse(positions[1])
			);
		}

		return true;
	}

}

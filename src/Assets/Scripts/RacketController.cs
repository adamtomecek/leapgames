using UnityEngine;
using System.Collections;

public class RacketController : MonoBehaviour {
	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKey (KeyCode.LeftArrow)){
			transform.Translate(new Vector2(-speed, 0));
		}

		if(Input.GetKey (KeyCode.RightArrow)){
			transform.Translate(new Vector2(+speed, 0));
		}
	}
}

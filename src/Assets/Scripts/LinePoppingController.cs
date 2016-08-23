using UnityEngine;
using System.Collections;

public class LinePoppingController : MonoBehaviour {
	float popTimer = 1.0f;
	bool collision = false;
	PoppingController controller;

	void Start(){
		controller = GameObject.Find ("Controller").GetComponent<PoppingController> ();
	}

	void Update(){
		if (collision) {
			popTimer -= Time.deltaTime;

			if(popTimer <= 0){
				controller.BalloonPop(this.tag);
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name == "Line") {
			collision = true;
		}else if(col.gameObject.name == "Bottom"){
			controller.GroundHit(this.tag);
			Destroy(gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.name == "Line") {
			popTimer = 1.0f; // reset pop timer
			collision = false;
		}
	}

}

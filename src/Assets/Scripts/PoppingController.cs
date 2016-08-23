using UnityEngine;
using System.Collections;
using System.IO;

public class PoppingController : MonoBehaviour {

	public GameObject redPrefab;
	public GameObject greenPrefab;

	int lives = 3;
	int score = 0;
	float timer = 3;
	string text = "";

	float max = 4.0f;
	float min = 2.0f;

	public GUIText scoreText;
	public GUIText livesText;
	public GUIText timerText;
	bool calibrated = false;

	public AudioClip[] sounds;

	void RemoveLife(){
		lives--;
		if(lives < 0)
			timer = 3;
	}

	void OnGUI(){
		GUIText gtext = GameObject.Find ("Text").GetComponent<GUIText> ();
		gtext.guiText.text = text;
		
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.LoadLevel("calibrator");
		}
		
		livesText.text = string.Concat ("Lives: ", lives);
		scoreText.text = string.Concat ("Score: ", score);

		if (!calibrated) {
			//string ttext = "Not calibrated. Press ESC to run calibrator.";
			//timerText.text = ttext;
		} else if (timer > 0) {
			timer -= Time.deltaTime;
			if(lives >= 0){
				string ttext = string.Concat ("Prepare to play\n", timer.ToString("#"));
				timerText.text = ttext;
			}else{
				string ttext = "Game Over!\nRestarting level";
				timerText.text = ttext;
			}
		} else if (timer > -1) {
			if(lives < 0){
				Application.LoadLevel(Application.loadedLevel);
			}else{
				timerText.text = "";
				timer = -1;
				StartCoroutine ("Spawner");
			}
		}
	}

	// Use this for initialization
	void Start () {
		if (!File.Exists ("coordinates")) {
			text = "Not calibrated. Start calibration by pressing ESCape key";
			return;
		}

		calibrated = true;
	}

	IEnumerator Spawner(){
		min *= 0.98f;
		max *= 0.98f;

		GameObject newBubble;

		if (Random.Range (0.0f, 1.0f) < 0.25f) {
			newBubble = Instantiate (redPrefab) as GameObject;
		} else {
			newBubble = Instantiate (greenPrefab) as GameObject;
		}

		float xPos = Random.Range (-4.5f, 4.5f);
		float drag = Random.Range (4.0f, 15.0f);

		newBubble.transform.position = new Vector2 (xPos, 5.5f);
		newBubble.rigidbody2D.drag = drag;

		yield return new WaitForSeconds(Random.Range(min, max));
		StartCoroutine ("Spawner");
	}

	public void GroundHit(string tag){
		audio.PlayOneShot (sounds[0]);
		if (tag == "green") {
			RemoveLife();
		}
	}
	
	public void BalloonPop(string tag){
		audio.PlayOneShot (sounds[1]);
		if (tag == "red") {
			RemoveLife();
		} else if (tag == "green") {
			score += 20;
		}
	}
}

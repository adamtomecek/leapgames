using UnityEngine;
using System.Collections;
using System.IO;

public class BallController : MonoBehaviour {
	public float speed;
	public Material[] materials;
	public int actualColor = 0;
	public string[] colors;
	public Transform[] rackets;

	public AudioClip brickDestroyed;
	public AudioClip wallHit;
	public AudioClip racketHit;

	int lives = 3;
	int score = 0;
	float timer = 3;
	int destroyedBlocks = 0;

	int level1Blocks = 28;

	public GUIText scoreText;
	public GUIText livesText;
	public GUIText timerText;

	bool calibrated = false;

	// Use this for initialization
	void Start () {
		if (!File.Exists ("coordinates")) {
			return;
		}
		
		calibrated = true;
		this.renderer.material = materials [actualColor];
		this.tag = colors [actualColor];
		DisableRackets ();
	}
	
	void LevelDone(){
	}

	void OnGUI(){
		livesText.text = string.Concat ("Lives: ", lives);
		scoreText.text = string.Concat ("Score: ", score);
		if (!calibrated) {
			return;
		}else if (timer > 0) {
			timer -= Time.deltaTime;
			if(destroyedBlocks == level1Blocks){
				string ttext = "Level cleared!";
				timerText.text = ttext;
			}else if(lives >= 0){
				string ttext = string.Concat ("Prepare to play\n", timer.ToString("#"));
				timerText.text = ttext;
			}else{
				string ttext = "Game Over!\nRestarting level";
				timerText.text = ttext;
			}
		} else if (timer > -1) {
			if(lives < 0 || destroyedBlocks == level1Blocks){
				Application.LoadLevel(Application.loadedLevel);
			}else{
				timerText.text = "";
				timer = -1;
				PushBall();
			}
		}
	}

	void PushBall(){
		rigidbody2D.velocity = Vector2.up * -speed;
	}

	void DisableRackets(){
		for (int i = 0; i < rackets.Length; i++) {
			BoxCollider2D collider = rackets[i].GetComponentInChildren<BoxCollider2D>() as BoxCollider2D;
			if(rackets[i].tag.ToString() != this.tag.ToString()){
				collider.enabled = false;
			}else{
				collider.enabled = true;
			}
		}
	}


	void ChangeColor(){
		//yield return new WaitForSeconds (Random.Range(0.5f, 0.7f));
		int col = Random.Range (0, 4);
		this.renderer.material = materials [col];
		this.tag = colors [col];
		DisableRackets ();
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.name == "racket") {
				// Calculate hit Factor
			float x = hitFactor(transform.position, col.transform.position, ((BoxCollider2D)col.collider).size.x);
				
				// Calculate direction, set length to 1
			Vector2 dir = new Vector2(x, 1).normalized;
				
				// Set Velocity with dir * speed
			rigidbody2D.velocity = dir;
			audio.PlayOneShot (racketHit, 0.05f);
		}else if(col.gameObject.tag == "wall"){
			audio.PlayOneShot (wallHit, 0.3f);
		}else if(col.gameObject.tag == "block"){
			destroyedBlocks++;
			score += 10;
			audio.PlayOneShot (brickDestroyed);

			if(destroyedBlocks == level1Blocks){
				timer = 3;
				transform.position = new Vector3(0, 1.5f, 0);
				rigidbody2D.velocity = Vector2.zero;
			}
		}

		if (col.gameObject.name == "bottomWall") {
			RemoveLife();
		}
	}

	void RemoveLife(){
		transform.position = new Vector3(0, 1.5f, 0);
		rigidbody2D.velocity = Vector2.zero;
		lives--;
		timer = 3;
	}

	void OnCollisionExit2D(Collision2D col){
		float mag = rigidbody2D.velocity.magnitude;
		if(mag > 0)
			rigidbody2D.velocity *= (speed / mag);

		if (col.gameObject.name == "racket") {
			if(Random.Range (0, 100) > 70){
				ChangeColor ();
			}
		}
	}

	float hitFactor(Vector2 ballPos, Vector2 racketPos,
	                float racketWidth) {
		return (ballPos.x - racketPos.x) / racketWidth;
	}
}

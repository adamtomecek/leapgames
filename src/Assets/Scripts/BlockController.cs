using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {
	void OnCollisionExit2D(Collision2D collisionInfo) {
		// Destroy the whole Block
		Destroy(gameObject);
	}
}

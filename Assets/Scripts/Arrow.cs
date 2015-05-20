using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public PlayerController owner;
	public float initialVelocity;
	public bool explosive;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {

//		PlayerMovement playerHit = collision.collider.GetComponent<PlayerMovement> ();
//		if (playerHit != null && playerHit.controller == owner)
//			return;

//			Destroy (gameObject);

		if (collision.gameObject.layer != 11) { // Layer 11 = Arrow
			if (this.explosive){
				Camera.main.GetComponent<Animator>().Play("ScreenShake");
				this.GetComponent<Animator>().Play("Explosion");

				Destroy (gameObject);
			}
			else{
				Destroy (gameObject);
			}
		}
	}

}

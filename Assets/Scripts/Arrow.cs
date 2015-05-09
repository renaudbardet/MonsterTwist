using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public PlayerController owner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {

		PlayerMovement playerHit = collision.gameObject.GetComponent<PlayerMovement> ();
		if (playerHit != null && playerHit.controller == owner)
			return;

		Destroy (gameObject);
		
	}

}

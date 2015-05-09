using UnityEngine;
using System.Collections;

public class Monstre : MonoBehaviour {

	public PlayerController controller;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {

		PlayerController playerHitting = null;

		if (collision.gameObject.GetComponent<PlayerMovement> () != null) {
			Debug.Log ("monster hit by player");
			playerHitting = collision.gameObject.GetComponent<PlayerMovement> ().controller;
		}
		
		if (collision.gameObject.GetComponent<Arrow> () != null) {
			Debug.Log("monster hit by arrow");
			playerHitting = collision.gameObject.GetComponent<Arrow> ().owner;
			Debug.Log(playerHitting);
		}

		Debug.Log(playerHitting);

		if (playerHitting != null)
			GameManager.instance.PlayerSlaynMonster (playerHitting);
	}

}

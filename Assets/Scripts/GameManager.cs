using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameObject joueur1;
	public GameObject joueur2;
	public GameObject joueur3;
	public GameObject joueur4;

	public GameObject monstre;
	private PlayerController oldPlayer;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void PlayerSlaynMonster( PlayerController player ){
		// "Résurection" de l'ancien joueur monstre
		oldPlayer = monstre.GetComponent<PlayerMovement>().controller;
		Debug.Log ("oldPlayer = " + oldPlayer);
		if (oldPlayer) {
			oldPlayer.playerGraphic.gameObject.SetActive(true);
			oldPlayer.playerGraphic = oldPlayer.defaultPlayerGraphic;
			// le rend visible et matériel
			oldPlayer.playerGraphic.GetComponent<BoxCollider2D> ().enabled  = true;
			oldPlayer.playerGraphic.GetComponent<SpriteRenderer> ().enabled  = true;
			oldPlayer.canShoot  = true;
		}

		// Stoppe le joueur après l'avoir mis à son spawn puis le rend invisible et immatériel
		player.playerGraphic.transform.position = player.playerGraphic.GetComponent<PlayerMovement> ().spawn.transform.position;
		player.playerGraphic.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
		player.playerGraphic.GetComponent<BoxCollider2D> ().enabled  = false;
		player.playerGraphic.GetComponent<SpriteRenderer> ().enabled  = false;
		player.canShoot  = false;
		
		Debug.Log ("slayn by " + player);
		// Change le role du monstre
		player.playerGraphic = monstre;
		monstre.GetComponent<PlayerMovement> ().controller = player;
	}

	void BeMonster(PlayerController oldPlayer){

	}

}

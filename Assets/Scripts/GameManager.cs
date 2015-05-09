using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public Monstre monstre;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void PlayerHitMonster( PlayerController player ){

		Debug.Log ("player " + player + " hit the monster");

		--monstre.life;
		Debug.Log ("monster life " + monstre.life);
		if (monstre.life <= 0)
			PlayerSlaynMonster (player);

	}

	public void PlayerSlaynMonster( PlayerController player ){

		// "Résurection" de l'ancien joueur monstre
		PlayerController oldPlayer = monstre.GetComponent<PlayerMovement>().controller;
		Debug.Log ("oldPlayer = " + oldPlayer);
		if (oldPlayer) {
			oldPlayer.RevertToHuman();
		}
		
		Debug.Log ("slayn by " + player);

		// Change le role du monstre
		player.BecomeMonster( monstre.GetComponent<Monstre>() );
		monstre.GetComponent<PlayerMovement> ().controller = player;
		monstre.life = monstre.maxLife;

	}

}

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public PlayerController controller;
	public GameObject spawn;

	void Start () {
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.GetComponent<Arrow> () != null && this.GetComponent<Monstre> () == null) {
			if (collision.collider.GetComponent<Arrow> ().explosive)
				controller.TakeDamage ();
			else
				controller.BecomeStun ();
		}
	}

}

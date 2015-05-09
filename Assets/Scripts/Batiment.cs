using UnityEngine;
using System.Collections;

public class Batiment : MonoBehaviour {
	public PlayerController player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.GetComponent<Monstre> () != null) {
			// Entre en contacte avec le monstre
			Debug.Log ("Batiment hit by Monster");

			if (collision.gameObject.GetComponent<PlayerMovement> ().controller != player){
				collision.gameObject.GetComponent<Monstre>().nbCrush ++;
				this.GetComponent<BoxCollider2D> ().enabled  = false;
				this.GetComponent<SpriteRenderer> ().enabled  = false;

				if (collision.gameObject.GetComponent<Monstre>().nbCrush == 3){
					GameManager.instance.GetComponent<GameManager>().MonsterWin();
				
				}
			}
		}
	}

	public void Reapparai(){
		this.GetComponent<BoxCollider2D> ().enabled  = true;
		this.GetComponent<SpriteRenderer> ().enabled  = true;
	}
}

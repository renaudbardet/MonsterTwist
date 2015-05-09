using UnityEngine;
using System.Collections;

public class Monstre : MonoBehaviour {

	public PlayerController controller;
	public int nbCrush = 0;
	

	public int maxLife = 5;
	
	public GameObject lifeBar;
	private int _life;
	public int life {
		get { return _life; }
		set { 
			_life = Mathf.Max(0, value);
			float lifeBarScale = (float)_life/(float)maxLife;
			lifeBar.transform.localScale = new Vector3( lifeBarScale, 1, 1 );
			lifeBar.transform.localPosition = new Vector3(0,0,0);
		}
	}
	
	// Use this for initialization
	void Start () {
		life = maxLife;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {

		if (collision.gameObject.GetComponent<PlayerMovement> () != null && _life == 0) {
			GameManager.instance.PlayerBecomeMonster( collision.gameObject.GetComponent<PlayerMovement> ().controller );
			return;
		}

		PlayerController playerHitting = null;
		if (collision.gameObject.GetComponent<Arrow> () != null) {
			playerHitting = collision.gameObject.GetComponent<Arrow> ().owner;
		}
		if (playerHitting != null)
			GameManager.instance.PlayerHitMonster (playerHitting);

	}

	public void Respawn(){
		this.transform.localPosition = new Vector3 (0, 0, 0);
		nbCrush = 0;
	}
	
}

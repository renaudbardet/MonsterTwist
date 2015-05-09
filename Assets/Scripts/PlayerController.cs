using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject playerGraphic;
	public PlayerMovement defaultPlayerGraphic;
	public int joystickNumber;
	public bool canShoot = true;

	public Rigidbody2D arrow;
	private Vector3 movementVector;
	private float movementSpeed = 6;

	public double arrowCooldown = .5;
	private double lastArrowShot = 0;
	
	public double humanPunchCooldown = .3;
	public double monsterPunchCooldown = .7;
	private double GetPunchCooldown() {
		return isMonster ? monsterPunchCooldown : humanPunchCooldown;
	}
	private double lastPunch = 0;

	private double isStunSince = -100;
	public double stunDelay = 1.5;

	public bool isMonster = false;

	Orient currentHeading = Orient.Right;

	// Use this for initialization
	void Start () {
		playerGraphic = defaultPlayerGraphic.gameObject;
		//movementVector.y = transform.position.y;
		//characterController = GetComponent<CharacterController>();
		defaultPlayerGraphic.controller = this;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time - isStunSince < stunDelay)
			return;

		string joystickString = joystickNumber.ToString();

		float xAxis = Input.GetAxis("Horizontal_P" + joystickString) * movementSpeed;
		float yAxis = - Input.GetAxis("Vertical_P" + joystickString) * movementSpeed;
		movementVector.x = xAxis;
		movementVector.z = 0;
		movementVector.y = yAxis;

		float xAxisDelta = Mathf.Abs (xAxis);
		float yAxisDelta = Mathf.Abs (yAxis);

		if (xAxisDelta > yAxisDelta) {

			if (xAxis < 0) {
				setHeading (Orient.Left);
			} else if (xAxis > 0) {
				setHeading (Orient.Right);
			}

		} else {

			if (yAxis < 0) {
				setHeading (Orient.Down);
			} else if (yAxis > 0) {
				setHeading (Orient.Up);
			}

		}

		playerGraphic.GetComponent<Rigidbody2D>().velocity = movementVector;

		if (isCloseRange () || isMonster) {

			if (	Input.GetButton ("Fire_P" + joystickString) 
			    && ((Time.time - lastPunch) > GetPunchCooldown())
			) {

				hitCloseRange();

			}
			
		}
		else {

			if (	!isMonster && canShoot
	    		&& Input.GetButton ("Fire_P" + joystickString) 
	    		&& ((Time.time - lastArrowShot) > arrowCooldown)
		    	) {
				
				shootArrow();

			}
		}
	}

	void hitCloseRange(){
		Debug.Log("try hitting smthg");

		lastPunch = Time.time;
		playerGraphic.GetComponent<Collider2D> ().enabled = false;
		switch (currentHeading) {
		case Orient.Up:
			CheckHit( Physics2D.BoxCast (playerGraphic.transform.position, playerGraphic.GetComponent<Renderer>().bounds.size, .0f, Vector2.up, 1) );
			break;
		case Orient.Down:
			CheckHit( Physics2D.BoxCast (playerGraphic.transform.position, playerGraphic.GetComponent<Renderer>().bounds.size, .0f, -Vector2.up, 1) );
			break;
		case Orient.Left:
			CheckHit( Physics2D.BoxCast (playerGraphic.transform.position, playerGraphic.GetComponent<Renderer>().bounds.size, .0f, -Vector2.right, 1) );
			break;
		case Orient.Right:
			CheckHit( Physics2D.BoxCast (playerGraphic.transform.position, playerGraphic.GetComponent<Renderer>().bounds.size, .0f, Vector2.right, 1) );
			break;
		}
		playerGraphic.GetComponent<Collider2D> ().enabled = true;

	}
	void CheckHit(RaycastHit2D hit) {
		if (hit.collider == null) return;
		Debug.Log("hitting smthg");
		if (hit.collider.GetComponent<Monstre> () != null)
			GameManager.instance.PlayerHitMonster (this);
		else if (isMonster) {
			if (hit.collider.GetComponent<PlayerMovement> () != null) {
				Debug.Log("hitting someone");
				hit.collider.GetComponent<PlayerMovement> ().controller.TakeDamage ();
				++playerGraphic.GetComponent<Monstre>().life;
			} else if (hit.collider.GetComponent<Arrow> () != null) {
				Destroy (hit.collider.gameObject);
			}
		}
	}
		
	void shootArrow() {

		lastArrowShot = Time.time;
		
		Rigidbody2D aRigidBody = Instantiate (arrow, playerGraphic.transform.position, transform.rotation) as Rigidbody2D;
		
		Arrow a = aRigidBody.GetComponent<Arrow> ();
		a.owner = this;
		
		Physics2D.IgnoreCollision (a.GetComponent<Collider2D> (), playerGraphic.GetComponent<Collider2D> ());
		
		switch (currentHeading) {
		case Orient.Up:
			aRigidBody.velocity = new Vector3 (0, a.initialVelocity*2);
			aRigidBody.rotation = 90;
			break;
		case Orient.Down:
			aRigidBody.velocity = new Vector3 (0, -a.initialVelocity*2);
			aRigidBody.rotation = 90;

			break;
		case Orient.Left:
			aRigidBody.velocity = new Vector3 (-a.initialVelocity, 0);
			break;
		case Orient.Right:
			aRigidBody.velocity = new Vector3 (a.initialVelocity, 0);
			break;
		}

	}

	void setHeading( Orient newHeading ) {

		if (newHeading != currentHeading) {
			currentHeading = newHeading;
		}

	}

	public void BecomeMonster( Monstre monster ) {

		monster.nbCrush = 0;
		monster.life = monster.maxLife;

		// Stoppe le joueur après l'avoir mis à son spawn puis le rend invisible et immatériel
		this.playerGraphic.transform.position = playerGraphic.GetComponent<PlayerMovement> ().spawn.transform.position;
		this.playerGraphic.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
		this.playerGraphic.GetComponent<BoxCollider2D> ().enabled  = false;
		this.playerGraphic.GetComponent<SpriteRenderer> ().enabled  = false;
		this.canShoot  = false;

		isMonster = true;
		this.playerGraphic = monster.gameObject;
		
	}

	public void RevertToHuman() {
		
		isMonster = false;
		this.playerGraphic = this.defaultPlayerGraphic.gameObject;

		this.playerGraphic.gameObject.SetActive(true);
		// le rend visible et matériel
		this.playerGraphic.GetComponent<BoxCollider2D> ().enabled  = true;
		this.playerGraphic.GetComponent<SpriteRenderer> ().enabled  = true;
		this.canShoot = true;

	}

	public void Respawn() {

		RevertToHuman ();

		playerGraphic.transform.position = defaultPlayerGraphic.spawn.transform.position;

	}

	public bool isCloseRange() {

		bool ret = false;
		playerGraphic.GetComponent<Collider2D> ().enabled = false;
		switch (currentHeading) {
		case Orient.Up:
			RaycastHit2D hit = Physics2D.BoxCast (playerGraphic.transform.position, playerGraphic.GetComponent<Renderer>().bounds.size, .0f, Vector2.up, 1);
			if( hit.collider != null && hit.distance < 1 )
				ret = true;
			break;
		case Orient.Down:
			hit = Physics2D.BoxCast (playerGraphic.transform.position, playerGraphic.GetComponent<Renderer>().bounds.size, .0f, -Vector2.up, 1);
			if( hit.collider != null && hit.distance < 1 )
				ret = true;
			break;
		case Orient.Left:
			hit = Physics2D.BoxCast (playerGraphic.transform.position, playerGraphic.GetComponent<Renderer>().bounds.size, .0f, -Vector2.right, 1);
			if( hit.collider != null && hit.distance < 1 )
				ret = true;
			break;
		case Orient.Right:
			hit = Physics2D.BoxCast (playerGraphic.transform.position, playerGraphic.GetComponent<Renderer>().bounds.size, .0f, Vector2.right, 1);
			if( hit.collider != null && hit.distance < 1 )
				ret = true;
			break;
		}
		playerGraphic.GetComponent<Collider2D> ().enabled = true;

		return ret;

	}

	public void TakeDamage() {
		if (!isMonster) {
			Respawn();
		}
	}

	public void BecomeStun() {
		if (!isMonster) {
			isStunSince = Time.time;
		}
	}

}

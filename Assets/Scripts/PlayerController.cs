using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject playerGraphic;
	public GameObject defaultPlayerGraphic;
	public int joystickNumber;
	public bool canShoot = true;

	public Rigidbody2D arrow;
	private Vector3 movementVector;
	private float movementSpeed = 6;

	public double arrowCooldown = 1;
	private double lastArrowShot = 0;

	public bool isMonster = false;

	Orient currentHeading = Orient.Right;

	// Use this for initialization
	void Start () {
		playerGraphic = defaultPlayerGraphic;
		//movementVector.y = transform.position.y;
		//characterController = GetComponent<CharacterController>();
		playerGraphic.GetComponent<PlayerMovement>().controller = this;
	}
	
	// Update is called once per frame
	void Update () {

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
				setHeading (Orient.Up);
			} else if (yAxis > 0) {
				setHeading (Orient.Down);
			}

		}

		playerGraphic.GetComponent<Rigidbody2D>().velocity = movementVector;

		if (	!isMonster && canShoot
		    	&& Input.GetButton ("Fire_P" + joystickString) 
		    	&& ((Time.time - lastArrowShot) > arrowCooldown)
		    ) {
			
			lastArrowShot = Time.time;
			Debug.Log( lastArrowShot );

			Rigidbody2D aRigidBody = Instantiate( arrow, playerGraphic.transform.position, transform.rotation ) as Rigidbody2D;

			Arrow a = aRigidBody.GetComponent<Arrow>();
			a.owner = this;

			Physics2D.IgnoreCollision( a.GetComponent<Collider2D>(), playerGraphic.GetComponent<Collider2D>() );

			switch( currentHeading ){
			case Orient.Up:
				aRigidBody.velocity = new Vector3( 0, -a.initialVelocity );
				break;
			case Orient.Down:
				aRigidBody.velocity = new Vector3( 0, a.initialVelocity );
				break;
			case Orient.Left:
				aRigidBody.velocity = new Vector3( -a.initialVelocity, 0 );
				break;
			case Orient.Right:
				aRigidBody.velocity = new Vector3( a.initialVelocity, 0 );
				break;
			}

		}

	}
	
	void setHeading( Orient newHeading ) {

		if (newHeading != currentHeading) {
			currentHeading = newHeading;
		}

	}

	public void BecomeMonster( Monstre monster ) {

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
		this.playerGraphic = this.defaultPlayerGraphic;

		this.playerGraphic.gameObject.SetActive(true);
		// le rend visible et matériel
		this.playerGraphic.GetComponent<BoxCollider2D> ().enabled  = true;
		this.playerGraphic.GetComponent<SpriteRenderer> ().enabled  = true;
		this.canShoot = true;
	}

}

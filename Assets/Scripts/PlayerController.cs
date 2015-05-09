using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject playerGraphic;
	public GameObject defaultPlayerGraphic;
	public int joystickNumber;

	public Rigidbody2D arrow;
	private Vector3 movementVector;
	private float movementSpeed = 10;

	public double arrowCooldown = .1;
	private double lastArrowShot = 0;


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

		if (	Input.GetButton ("Fire_P" + joystickString) 
		    	&& (Time.time - lastArrowShot) > arrowCooldown
		    ) {

			Rigidbody2D a = Instantiate( arrow, playerGraphic.transform.position, transform.rotation ) as Rigidbody2D;

			a.GetComponent<Arrow>().owner = this;

			switch( currentHeading ){
			case Orient.Up:
				a.velocity = new Vector3( 0, -10 );
				break;
			case Orient.Down:
				a.velocity = new Vector3( 0, 10 );
				break;
			case Orient.Left:
				a.velocity = new Vector3( -10, 0 );
				break;
			case Orient.Right:
				a.velocity = new Vector3( 10, 0 );
				break;
			}

			lastArrowShot = Time.time;

		}

	}
	
	void setHeading( Orient newHeading ) {

		if (newHeading != currentHeading) {
			currentHeading = newHeading;
		}

	}

}

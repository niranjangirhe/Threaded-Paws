using UnityEngine;
using System.Collections;
using UnityEngine.UI; //namespaces

public class PlayerController : MonoBehaviour {

	public float speed; //when public, changes can be made in the editor!
	public Text countText;
	public Text winText;

	private Rigidbody rb; //holds the reference of the rigidbody we want to access
	private int count;

	void Start() {
		rb = GetComponent<Rigidbody>(); //gets reference to the attached rigidbody, if there is one

		count = 0;
		SetCountText ();

		winText.text = "";
	}

	void FixedUpdate() { //called just before performing physics calculations
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		//adds a force to a rigid body, making it move
		//force mode is default
		//movement is a vector3(x,y,z)
		//gives us control over the speed of the ball
		rb.AddForce (movement * speed);
	}

	//called every time we touch a trigger collider
	// called when the player object collides with a trigger collider
	// using this instead of others because we dont want the player to bounce off the other objects, just collect them
	void OnTriggerEnter(Collider other) {
		//destroy the game object the trigger collider is attached to when we touch another trigger collider
		//all of its components, as well as its children and components, are also removed from the scene
		//Destroy(other.gameObject);

		//however, we dont wanna remove anything from the scene but rather reactivate them
		if (other.gameObject.CompareTag("Pickup")) {
			other.gameObject.SetActive(false);
			count += 1;
			SetCountText ();
		}
	}

	void SetCountText() {
		countText.text = "Count: " + count.ToString ();

		if (count == 11) {
			winText.text = "You win!";
		}
	}

}
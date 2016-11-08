using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//Parallax scrolling script that should be assigned to a LAYER
public class ScrollingScript : MonoBehaviour {

	//scrolling speed
	public Vector2 speed = new Vector2(2, 2); //x,y

	//moving direction
	public Vector2 direction = new Vector2(-1, 0);

	//movement should be applied to the camera
	public bool isLinkedToCamera = false;

	//background is infinite
	//need a public variable to turn on the "looping" mode in the inspector view
	public bool isLooping = false;

	//List to store the layer children
	private List<SpriteRenderer> backgroundPart;

	//Get all the children
	//set the backgroundPart list with the children that have a renderer.
	void Start() {
		//for infinite background only
		if (isLooping) {

			//Get all the children of the layer with a renderer
			backgroundPart = new List<SpriteRenderer>();

			for (int i = 0; i < transform.childCount; i++) {
				Transform child = transform.GetChild (i);
				SpriteRenderer r = child.GetComponent<SpriteRenderer> ();

				//only add the visible children
				if (r != null) {
					backgroundPart.Add (r);
				}
			}

			//sort by position
			//Get the children from left to right. We would need to add a few conditions to handle
			//all the possible scrolling directions.
			//using LINQ, order them by their z position and put the leftmost at the first position of the array
			backgroundPart = backgroundPart.OrderBy( t => t.transform.position.x).ToList();
		}
	}

	// Update is called once per frame
	//if the isLooping flag is set to true, retrieve the first child stored in the backgroundPart list.
	//test if its completely outside of the camera field.
	//if it is, change its position to be after the last(rightmost) child. Finally, put it at the last position
	//of backgroundPart list.
	void Update () {
		
		//movement
		Vector3 movement = new Vector3(speed.x * direction.x, speed.y * direction.y, 0);

		movement *= Time.deltaTime;
		transform.Translate (movement);

		if (isLinkedToCamera) {
			Camera.main.transform.Translate (movement);
		}

		//loop
		if (isLooping) {
			// get the first object
			//the list is ordered from left (x position) to right
			SpriteRenderer firstChild = backgroundPart.FirstOrDefault();

			if (firstChild != null) {
				//check if the child is already (partly) before the camera.
				//test the position first, since the IsVisibleFrom method is heavier to execute
				if (firstChild.transform.position.x < Camera.main.transform.position.x) {
					//if the child is already on the left of the camera, then we test if it's
					//completely outside of it and needs to be recycled
					if (firstChild.IsVisibleFrom (Camera.main) == false) {
						//get the last child position
						SpriteRenderer lastChild = backgroundPart.LastOrDefault();

						Vector3 lastPosition = lastChild.transform.position;
						Vector3 lastSize = (lastChild.bounds.max - lastChild.bounds.min);

						//set the position of the recycled one to be AFTER the last child.
						//only works for horizontal scrolling
						firstChild.transform.position = new Vector3(lastPosition.x + lastSize.x + firstChild.transform.position.y,
							firstChild.transform.position.z);

						//set the recycled child to the last position of the brackgroundPart list
						backgroundPart.Remove(firstChild);
						backgroundPart.Add (firstChild);
					}
				}
			}
		}
	}	
}
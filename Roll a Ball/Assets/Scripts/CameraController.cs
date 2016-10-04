using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player; //in the editor, need to drag the player gameobject in the corresponding field
	Vector3 offset;

	// Use this for initialization
	void Start () {
		//current transform position of camera - (minus) that of the player
		offset = transform.position - player.transform.position;
	}

	// Update is called once per frame
	void Update () { //runs every frame, but is guaranteed to only do so after all items have been processed in update. that way, we know that the player has absolutely moved for that frame.
		//as the player moves, the camera is aligned with the players object before every frame is shown
		transform.position = player.transform.position + offset;
	}
}
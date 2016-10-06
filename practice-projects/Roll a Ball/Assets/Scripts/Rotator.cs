using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	//to make the cube rotate, need to change its angles every frame.
	//rotate the transform

	// Update is called once per frame
	void Update () {
		//frame independent = deltatime
		transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
	}
}

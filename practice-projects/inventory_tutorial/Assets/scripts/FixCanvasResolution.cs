using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FixCanvasResolution : MonoBehaviour {

	private CanvasScaler scaler;

	//set canvas scaler to scale with screen size AFTER the game starts
	void Start () {
		scaler = GetComponent<CanvasScaler> ();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
	}
}

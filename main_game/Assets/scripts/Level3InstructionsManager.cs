using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3InstructionsManager : MonoBehaviour {

	public GameObject level3_part1;
	public GameObject level3_part2;

	public GameObject level3_disableFunctionality;

	public void level3_getPanel1() {

		level3_part1.SetActive (true);
		level3_part2.SetActive (false);
	}

	public void level2_getPanel2() {

		level3_part2.SetActive (true);
		level3_part1.SetActive (false);
	}

	public void level2_activateAllFunctionality() {
		
		level3_disableFunctionality.SetActive (false);
	}

	public void skipInstructions() {

		level3_part1.SetActive (false);
		level3_part2.SetActive (false);

		level2_activateAllFunctionality ();
	}
		
	void Start() {

		level3_part1.SetActive (true);
		level3_part2.SetActive (false);

		level3_disableFunctionality.SetActive (true);
	}
}

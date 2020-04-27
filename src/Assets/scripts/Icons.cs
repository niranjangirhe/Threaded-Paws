﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icons : MonoBehaviour {

	public GameObject informationPanel;
	public GameObject agendaPanel;

	// Use this for initialization
	void Start () {

		// informationWindow = GameObject.Find ("InformationPanel");

		try {
			informationPanel.SetActive (false);
		} catch {
			Debug.Log ("Information Panel can't be found.");
		}

		try {
			agendaPanel.SetActive (false);
		} catch {
			Debug.Log ("Agenda Panel can't be found.");
		}
	}

	public void toggleInformationWindow () {

		// Debug.Log ("Information button clicked");

		try {
			if (informationPanel.activeSelf) {
				informationPanel.SetActive (false);
			} else {
				informationPanel.SetActive (true);

				LogManager.instance.logger.sendChronologicalLogs("InfoButton", "", LogManager.instance.UniEndTime().ToString());
				LogManager.instance.infoCount++;
				// agendaPanel.SetActive(false);
			}
		} catch {
			Debug.Log ("Information Panel can't be found.");
		}
	}

	public void toggleAgendaWindow () {

		try {
			if (agendaPanel.activeSelf) {
				agendaPanel.SetActive (false);
			} else {
				agendaPanel.SetActive (true);
				LogManager.instance.logger.sendChronologicalLogs("AgendaButton", "", LogManager.instance.UniEndTime().ToString());

				LogManager.instance.agendaCount++;
				informationPanel.SetActive (false);
			}
		} catch {
			Debug.Log ("Agenda Panel can't be found.");
		}

	}
}
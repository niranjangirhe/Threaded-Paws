using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icons : MonoBehaviour {

	public GameObject informationPanel;
	public GameObject agendaPanel;
	private bool agendaFadeIn;
	private bool agendaFadeOut;
	private bool infoFadeIn;
	private bool infoFadeOut;
	private float speed = 8;

	// Use this for initialization
	void Start () {

		// informationWindow = GameObject.Find ("InformationPanel");

		try {
			infoFadeOut = true;
			infoFadeIn = false;
		} catch {
			Debug.Log ("Information Panel can't be found.");
		}

		try {
			agendaFadeOut = true;
			agendaFadeOut = false;
		} catch {
			Debug.Log ("Agenda Panel can't be found.");
		}
	}

	public void toggleInformationWindow () {

		// Debug.Log ("Information button clicked");

		try {
			if (informationPanel.activeSelf) {
				infoFadeOut = true;
				infoFadeIn = false;
			} else {
				infoFadeOut = false; ;
				infoFadeIn = true;

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
				agendaFadeOut = true;
				agendaFadeIn = false;
			} else {
				agendaFadeIn = true;
				agendaFadeOut = false;
				LogManager.instance.logger.sendChronologicalLogs("AgendaButton", "", LogManager.instance.UniEndTime().ToString());

				LogManager.instance.agendaCount++;
				infoFadeOut = true;
				infoFadeIn = false;
			}
		} catch {
			Debug.Log ("Agenda Panel can't be found.");
		}

	}

	public void closeAll()
    {
		try
		{
			if (agendaPanel.activeSelf)
			{
				//agendaPanel.SetActive(false);
				agendaFadeOut = true;
				agendaFadeIn = false;
			}
			if (informationPanel.activeSelf)
			{
				infoFadeOut = true;
				infoFadeIn = false;
			}
		}
		catch
		{ }
	}
	public void Update()
	{
		//Fadeout Agenda
		if(agendaFadeOut)
        {
			agendaFadeIn = false;
			CanvasGroup cg = agendaPanel.GetComponent<CanvasGroup>();
			if(cg.alpha>0)
            {
				cg.alpha -= Time.deltaTime*speed;
            }
			else
            {
				cg.alpha = 0;
				agendaFadeOut = false;
				agendaPanel.SetActive(false);
            }
        }
		//Fadein Agenda
		else if (agendaFadeIn)
		{ 
			agendaPanel.SetActive(true);
			CanvasGroup cg = agendaPanel.GetComponent<CanvasGroup>();
			if (cg.alpha < 1)
			{
				cg.alpha += Time.deltaTime * speed;
			}
			else
			{
				cg.alpha = 1;
				agendaFadeIn = false;
			}
		}

		//Fadeout InfoPanel
		if (infoFadeOut)
		{
			infoFadeIn = false;
			CanvasGroup cg = informationPanel.GetComponent<CanvasGroup>();
			if (cg.alpha > 0)
			{
				cg.alpha -= Time.deltaTime * speed;
			}
			else
			{
				cg.alpha = 0;
				infoFadeOut = false;
				informationPanel.SetActive(false);
			}
		}
		//Fadein InfoPanel
		else if (infoFadeIn)
		{
			informationPanel.SetActive(true);
			CanvasGroup cg = informationPanel.GetComponent<CanvasGroup>();
			if (cg.alpha < 1)
			{
				cg.alpha += Time.deltaTime * speed;
			}
			else
			{
				cg.alpha = 1;
				infoFadeIn = false;
			}
		}
	}
}
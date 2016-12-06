using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject menuButtons;
	public GameObject aboutPanel;

	// Use this for initialization
	void Start () {
	
	}

	public void LoadDemoScene () 
	{
		menuButtons.SetActive (false);

		SceneManager.LoadScene ("Act1_Final_Level");
	}

	public void LoadQuestScene ()
	{
		//menuButtons.SetActive (false);

		//SceneManager.LoadScene ("Quest");
	}

	public void LoadMainMenuScene ()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	public void ExitGame ()
	{
		Application.Quit();
	}

	public void ShowAbout ()
	{
		menuButtons.SetActive (false);
		aboutPanel.SetActive (true);
	}

	public void CloseAbout ()
	{
		menuButtons.SetActive (true);
		aboutPanel.SetActive (false);
	}
}

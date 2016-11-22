using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    /* TODO:
     * Fix proper sprites/images for everything
     * Discuss healthbar
     */

    [System.Serializable]
    public class Instrument
    {
        //TODO: Maybe implement variables for attack mode? 
        public Texture mainTexture;
        public Texture smallTexture;
        public Texture selectedTexture;
        public string instrumentName;
        public GameObject tooltip;
        public AudioClip clip;
        public Attack.Mode inMode;
        public RawImage rawImage;
    }

    public RawImage MainSlot;
    public RawImage OffSlot;

    public GameObject mainMenuPanel;
    public GameObject offMenuPanel;

    //main = main hand, the left hand side instrument. off = offhand, the right hand side instrument
    public Instrument[] mainhandInstruments;
    public Instrument[] offhandInstruments;
    [HideInInspector] public Instrument mainHand, offHand; //currently equipped instruments, could maybe be integrated and removed? Change if performance is an issue

    //true if the menu is open
    private bool mainMenu = false, offMenu = false;

    //position in menu, number of instruments, index of equipped instrument
    private int mainMenuPos = 0, offMenuPos = 0, num_mainInstruments = 2, num_offInstruments = 2, currentMain = 0, currentOff = 0;

    private Attack attack;

    private Light lights;

    void Start()
    {
        num_offInstruments = offhandInstruments.Length;
        num_mainInstruments = mainhandInstruments.Length;
        mainHand = mainhandInstruments[currentMain];
        offHand = offhandInstruments[currentOff];

		//COMMENT use UI overlay instead - Mr. H
        GameObject lightsource = GameObject.Find("Directional Light");
        lights = lightsource.GetComponent<Light>();

		GameObject theplayer = GameObject.FindWithTag("Player");
        attack = theplayer.GetComponentInChildren<Attack>();

		attack.currentAttackMode = mainHand.inMode;

        mainMenuPanel.SetActive(false);
        offMenuPanel.SetActive(false);
    }

    //called every frame
    void Update()
    {
        if (mainMenu || offMenu)
        {
            Time.timeScale = 0.1f;
            if (lights.intensity > 0.1f)
            {
                lights.intensity -= 5.0f * Time.deltaTime;
            }       
        }
        else
        {
            Time.timeScale = 1.0f;
            if (lights.intensity < 1.0f)
            {
                lights.intensity += 0.5f * Time.deltaTime;
            }
        }
        InputHandler();
    }

    void InputHandler()
    {
		if (CloseMenuInputTriggered() == true)
        {
            if (mainMenu)
            {
                closeMainMenu();
            }
            else if (offMenu)
            {
                closeOffMenu();
            }

        }
		if (NavRightInputTriggered() == true)
        {
            if (mainMenu)
            {
                if (mainMenuPos == num_mainInstruments - 1)
                {
                    mainMenuPos = 0;
                }
                else
                {
                    mainMenuPos++;
                }
            }

            else if (offMenu)
            {
                if (offMenuPos == num_offInstruments - 1)
                {
                    offMenuPos = 0;
                }
                else
                {
                    offMenuPos++;
                }
            }

        }

		if (NavLeftInputTriggered() == true)
        {
            if (mainMenu)
            {
                if (mainMenuPos == 0)
                {
                    mainMenuPos = num_mainInstruments - 1;
                }
                else
                {
                    mainMenuPos--;
                }
            }
            else if (offMenu)
            {
                if (offMenuPos == 0)
                {
                    offMenuPos = num_offInstruments - 1;
                }
                else
                {
                    offMenuPos--;
                }
            }
        }

		if (MainMenuInputTriggered() == true)
        {
            //I hate reading one line ifs. If you want them you can change this.
            //Turn menu on if it was off, close it if it was on
            if (mainMenu == true)
            {
                closeMainMenu();
            }
            else if (offMenu == false) //cant open both menus at once
            {
                mainMenu = true;
            }
        }
		else if (OffMenuInputTriggered() == true)
        {
            //Turn menu on if it was off, close it if it was on
            if (offMenu == true)
            {
                closeOffMenu();
            }
            else if (mainMenu == false) //cant open both menus at once
            {
                offMenu = true;
            }
        }
    }

    /*
     * Closes mainhand (left) menu, either by hitting the menu key again or Enter/Return
     */
    void closeMainMenu()
    {
        if (currentMain != mainMenuPos)
        {
            currentMain = mainMenuPos;
            mainHand = mainhandInstruments[currentMain];
            attack.currentAttackMode = mainHand.inMode;
            MainSlot.texture = mainHand.mainTexture;
        }
        mainMenuPos = currentMain;
        mainMenu = false;
        mainMenuPanel.SetActive(false);
    }

    /*
     * Closes offhand (right) menu, either by hitting the menu key again or Enter/Return
     */
    void closeOffMenu()
    {
        if (currentOff != offMenuPos)
        {
            currentOff = offMenuPos;
            offHand = offhandInstruments[currentOff];
            OffSlot.texture = offHand.mainTexture;
        }
        offMenuPos = currentOff;
        offMenu = false;
        offMenuPanel.SetActive(false);
    }

    /*
     * All the GUI objects to be rendered. Also called every frame.
     */
    void OnGUI()
    {
        if (mainMenu) //if mainhand menu is currently open
        {
            mainMenuPanel.SetActive(true);
            for (int i = 0; i < num_mainInstruments; i++)
            {
                if (mainMenuPos == i) //if selected display selected texture/sprite
                {
                    mainhandInstruments[i].rawImage.texture = mainhandInstruments[i].selectedTexture;
                    mainhandInstruments[i].tooltip.SetActive(true);
                }
                else 
                {
                    mainhandInstruments[i].rawImage.texture = mainhandInstruments[i].smallTexture;
                    mainhandInstruments[i].tooltip.SetActive(false);
                }
            }
        }

        if (offMenu) //if offhand menu is currently open
        {
            offMenuPanel.SetActive(true);
            for (int i = 0; i < num_offInstruments; i++)
            {
                if (offMenuPos == i) //if selected display selected texture/sprite
                {
                    offhandInstruments[i].rawImage.texture = offhandInstruments[i].selectedTexture;
                    offhandInstruments[i].tooltip.SetActive(true);
                }
                else //This instrument i is not selected
                {
                    offhandInstruments[i].rawImage.texture = offhandInstruments[i].smallTexture;
                    offhandInstruments[i].tooltip.SetActive(false);
                }
            }
        }

    }

	bool MainMenuInputTriggered() {
		return Input.GetKeyDown (KeyCode.G) || Input.GetKeyDown (GlobalConstants.XBOX_BTN_X);
	}

	bool OffMenuInputTriggered() {
		return Input.GetKeyDown (KeyCode.H) || Input.GetKeyDown (GlobalConstants.XBOX_BTN_B);
	}

	bool NavLeftInputTriggered() {
		return Input.GetKeyDown (KeyCode.J) || Input.GetKeyDown (GlobalConstants.XBOX_BTN_LB);
	}

	bool NavRightInputTriggered() {
		return Input.GetKeyDown (KeyCode.K) || Input.GetKeyDown (GlobalConstants.XBOX_BTN_RB);
	}

	bool CloseMenuInputTriggered() {
		return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown (GlobalConstants.XBOX_BTN_Y);
	}
}
///Author(s): Samuel Ekne, Julia von Heijne
///Date: 10-11-2016
///Last revision: 16-11-2016
using UnityEngine;
using System.Collections;

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
        public string tooltip;
        public AudioClip clip;
        public Attack.Mode inMode;
    }

    //main = main hand, the left hand side instrument. off = offhand, the right hand side instrument
    public Instrument[] mainhandInstruments;
    public Instrument[] offhandInstruments;
    [HideInInspector] public Instrument mainHand, offHand; //currently equipped instruments, could maybe be integrated and removed? Change if performance is an issue

    public Texture mainUI;
    public Texture mainMenuTexture;
    public Texture offMenuTexture;

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
        }
        mainMenuPos = currentMain;
        mainMenu = false;
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
        }
        offMenuPos = currentOff;
        offMenu = false;
    }

    /*
     * All the GUI objects to be rendered. Also called every frame.
     */
    void OnGUI()
    {
        //tooltip formatting
        GUIStyle tooltipStyle = "box";
        tooltipStyle.wordWrap = true;

        //basic UI bars (not including top bars)
        GUI.Box(new Rect(Screen.width / 20, Screen.height - 350, Screen.width, Screen.height / 2.5f), mainUI, GUIStyle.none);

        //mainhand slot
        GUI.Box(new Rect((Screen.width / 10) - 5, Screen.height - 260, 200, Screen.height / 6), mainhandInstruments[currentMain].mainTexture, GUIStyle.none);

        //offhand slot
        GUI.Box(new Rect(9 * (Screen.width / 11)-10, Screen.height - 260, 200, Screen.height / 6), offhandInstruments[currentOff].mainTexture, GUIStyle.none);

        if (mainMenu) //if mainhand menu is currently open
        {
            //Top mainhand menu bar (left)
            GUI.Box(new Rect((Screen.width/32)-15, Screen.height/2+10, Screen.width/4, Screen.height/4), mainMenuTexture, GUIStyle.none);

            //maybe need to change this to be based on screen size. Looked OK when I tested it. 
            int mainSpacing = Screen.width/16+5;

            for (int i = 0; i < num_mainInstruments; i++)
            {
                if (mainMenuPos == i) //if selected display selected texture/sprite
                {
                    //Display tooltip above selected instrument
                    GUI.Box(new Rect(mainSpacing-10, Screen.height - 480, 150, 50), mainhandInstruments[i].tooltip, tooltipStyle);
                    //Display selected instrument sprite for instrument i
                    GUI.Box(new Rect(mainSpacing, Screen.height - 440, 100, 100), mainhandInstruments[i].selectedTexture, GUIStyle.none);
                }
                else 
                {
                    //Display non-selected instrument sprite in the same spot
                    GUI.Box(new Rect(mainSpacing, Screen.height - 440, 100, 100), mainhandInstruments[i].smallTexture, GUIStyle.none);
                }
                mainSpacing += 150; //move to the next slot, to the right
            }
        }

        if (offMenu) //if offhand menu is currently open
        {
            //Top offhand menu bar (right)
            GUI.Box(new Rect(23*(Screen.width/32)+30, Screen.height/2+10, Screen.width / 4, Screen.height / 4), offMenuTexture, GUIStyle.none);

            //position of first element (farthest to the left)
            int offSpacing = 14*(Screen.width /16)+2; //dont know why I need +125 here, is -= 125 evaluated first?

            for (int i = 0; i < num_offInstruments; i++)
            {
                if (offMenuPos == i) //if selected display selected texture/sprite
                {
                    //Diplay tooltip for selected instrument
                    GUI.Box(new Rect(offSpacing-20, Screen.height - 480, 150, 50), offhandInstruments[i].tooltip, tooltipStyle);
                    //Display selected instrument sprite
                    GUI.Box(new Rect(offSpacing, Screen.height - 440, 100, 100), offhandInstruments[i].selectedTexture, GUIStyle.none);
                }
                else //This instrument i is not selected
                {
                    //Display non-selected instrument sprite
                    GUI.Box(new Rect(offSpacing, Screen.height - 440, 100, 100), offhandInstruments[i].smallTexture, GUIStyle.none);
                }
                offSpacing -= 150; //move to next slot, to the left
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
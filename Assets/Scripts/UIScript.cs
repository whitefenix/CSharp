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
        public Texture mainTexture;
        public Texture smallTexture;
        public Texture selectedTexture;
        public string instrumentName;
        public string tooltip;
    }

    //main = main hand, the left hand side instrument. off = offhand, the right hand side instrument
    public Instrument[] mainhandInstruments;
    public Instrument[] offhandInstruments;
    public Instrument mainHand, offHand; //currently equipped instruments, could maybe be integrated and removed? Change if performance is an issue

    //true if the menu is open
    private bool mainMenu = false, offMenu = false;

    //position in menu, number of instruments, index of equipped instrument
    private int mainMenuPos = 0, offMenuPos = 0, num_mainInstruments = 2, num_offInstruments = 2, currentMain = 0, currentOff = 0;

    void Start()
    {
        //TODO: This will need to be changed if we ever get more instruments in the game
        num_offInstruments = offhandInstruments.Length;
        num_mainInstruments = mainhandInstruments.Length;
        mainHand = mainhandInstruments[currentMain];
        offHand = offhandInstruments[currentOff];
    }

    //called every frame
    void Update()
    {
        InputHandler();
    }

    void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Return) == true)
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
        if (Input.GetKeyDown(KeyCode.K) == true)
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

        if (Input.GetKeyDown(KeyCode.J) == true)
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

        if (Input.GetKeyDown(KeyCode.G) == true)
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
        else if (Input.GetKeyDown(KeyCode.H) == true)
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

        //mainhand slot
        GUI.Box(new Rect(100, Screen.height - 200, 200, 200), mainhandInstruments[currentMain].mainTexture, GUIStyle.none);

        //offhand slot
        GUI.Box(new Rect(Screen.width - 300, Screen.height - 200, 200, 200), offhandInstruments[currentOff].mainTexture, GUIStyle.none);

        if (mainMenu)
        {
            GUI.Box(new Rect(50, Screen.height - 250, 300, 75), "");

            //maybe need to change this to be based on screen size. Looked OK when I tested it. 
            int mainSpacing = 75;

            for (int i = 0; i < num_mainInstruments; i++)
            {
                if (mainMenuPos == i) //if selected display selected texture/sprite
                {
                    //TODO: This is the selected instrument: Write proper tooltip for "mainInstruments[i]", which is the same instrument
                    //but different icon from smallMain[i] and smallMainSel[i]. Check inspector for this script to see what index is what instrument currently
                    GUI.Box(new Rect(mainSpacing, Screen.height - 320, 150, 50), i.ToString() + ": tooltips (placeholder)", tooltipStyle);
                    GUI.Box(new Rect(mainSpacing, Screen.height - 260, 100, 100), mainhandInstruments[i].selectedTexture, GUIStyle.none);
                }
                else //otherwise display normal texture/sprite
                {
                    GUI.Box(new Rect(mainSpacing, Screen.height - 260, 100, 100), mainhandInstruments[i].smallTexture, GUIStyle.none);
                }
                mainSpacing += 125;
            }
        }

        if (offMenu)
        {
            GUI.Box(new Rect(Screen.width - 350, Screen.height - 250, 300, 75), "");

            int offSpacing = (Screen.width - 325) + 125; //dont know why I need +125 here, is -= 125 evaluated first?
            for (int i = 0; i < num_offInstruments; i++)
            {
                if (offMenuPos == i) //if selected display selected texture/sprite
                {
                    //TODO: Selected instrument for offhand write proper tooltip here, same deal as mainhand above
                    // in the offHand, the harp is pre-selected and thus have index 0, giving the violin index 1 
                    GUI.Box(new Rect(offSpacing, Screen.height - 320, 150, 50), i.ToString() + ": tooltips (placeholder)", tooltipStyle);
                    GUI.Box(new Rect(offSpacing, Screen.height - 260, 100, 100), offhandInstruments[i].selectedTexture, GUIStyle.none);
                }
                else //otherwise display normal texture/sprite
                {
                    GUI.Box(new Rect(offSpacing, Screen.height - 260, 100, 100), offhandInstruments[i].smallTexture, GUIStyle.none);
                }
                offSpacing -= 125;
            }
        }

        /* hearts
         * looks like shit right now so is not displayed.
         * Five boxes between the instruments
         * Should probably be switched to a lifebar? Idk
         */
        /*
        GUI.Box(new Rect(Screen.width - 375, Screen.height - 125, 50, 50), "");
        GUI.Box(new Rect(Screen.width - 475, Screen.height - 125, 50, 50), "");
        GUI.Box(new Rect(Screen.width - 575, Screen.height - 125, 50, 50), "");
        GUI.Box(new Rect(Screen.width - 675, Screen.height - 125, 50, 50), "");
        GUI.Box(new Rect(Screen.width - 775, Screen.height - 125, 50, 50), "");
        */
    }
}

///Author(s): Samuel Ekne, Julia von Heijne
///Date: 10-11-2016
///Last revision: 13-11-2016
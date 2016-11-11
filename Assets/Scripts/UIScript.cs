using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour
{
    /* TODO:
     * Fix proper sprites/images for everything
     * Discuss healthbar
     * Set tooltips (Julia?)
     */

    //main = main hand, the left hand side instrument. off = offhand, the right hand side instrument
    public Texture[] mainInstruments, smallMain, smallMainSel, offInstruments, smallOff, smallOffSel; //texture arrays for all available menu icons
    public Texture mainHand, offHand; //currently equipped instruments, could maybe be integrated and removed? Change if performance is an issue

    //true if the menu is open
    private bool mainMenu = false, offMenu = false;

    //position in menu, number of instruments, index of equipped instrument
    private int mainMenuPos = 0, offMenuPos = 0, num_mainInstruments = 2, num_offInstruments = 2, currentMain = 0, currentOff = 0;

    void Start()
    {
        //TODO: This will need to be changed if we ever get more instruments in the game
        num_offInstruments = offInstruments.Length;
        num_mainInstruments = mainInstruments.Length;
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
            mainHand = mainInstruments[currentMain];
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
            offHand = offInstruments[currentOff];
        }
        offMenuPos = currentOff;
        offMenu = false;
    }

    /*
     * All the GUI objects to be rendered. Also called every frame.
     */
    void OnGUI()
    {
        //mainhand slot
        GUI.Box(new Rect(100, Screen.height - 200, 200, 200), mainHand, GUIStyle.none);

        //offhand slot
        GUI.Box(new Rect(Screen.width - 300, Screen.height - 200, 200, 200), offHand, GUIStyle.none);

        if (mainMenu)
        {
            GUI.Box(new Rect(50, Screen.height - 250, 300, 75), "");

            //maybe need to change this to be based on screen size. Looked OK when I tested it. 
            int mainSpacing = 75;

            for (int i = 0; i < num_mainInstruments; i++)
            {
                if (mainMenuPos == i) //if selected display selected texture/sprite
                {
                    //TODO: This is the selected instrument: Display tooltip for "mainInstruments[i]", which is the same instrument
                    //but different icon from smallMain[i] and smallMainSel[i]. Check inspector for this script to see what index is what instrument currently
                    GUI.Box(new Rect(mainSpacing, Screen.height - 260, 100, 100), smallMainSel[i], GUIStyle.none);
                }
                else //otherwise display normal texture/sprite
                {
                    GUI.Box(new Rect(mainSpacing, Screen.height - 260, 100, 100), smallMain[i], GUIStyle.none);
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
                    //TODO: Selected instrument for offhand so put tooltip here, same deal as mainhand above
                    GUI.Box(new Rect(offSpacing, Screen.height - 260, 100, 100), smallOffSel[i], GUIStyle.none);
                }
                else //otherwise display normal texture/sprite
                {
                    GUI.Box(new Rect(offSpacing, Screen.height - 260, 100, 100), smallOff[i], GUIStyle.none);
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

///Author(s): Samuel Ekne
///Date: 10-11-2016
///Last revision: 11-11-2016
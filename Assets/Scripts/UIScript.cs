﻿using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour
{


    /* TODO:
     * Fix displaying selected/not selected instrument in the menu so its not hard coded
     * Fix proper sprites/images for everything
     * Discuss healthbar
     * Set tooltips (Julia?)
     */

    //main = main hand, the left hand side instrument. off = offhand, the right hand side instrument

    public Texture[] mainInstruments, smallMain, smallMainSel, offInstruments, smallOff, smallOffSel; //texture arrays for all available menu icons
    public Texture mainHand, offHand; //currently equipped instruments, could maybe be integrated and removed?

    //true if the menu is open
    private bool mainMenu = false, offMenu = false;

    //position in menu, number of instruments, index of equipped instrument
    private int mainMenuPos = 0, offMenuPos = 0, num_mainInstruments = 2, num_offInstruments = 2, currentMain = 0, currentOff = 0;

    void Start()
    {
        num_offInstruments = offInstruments.Length;
        num_mainInstruments = mainInstruments.Length;
    }

    void Update()
    {
        InputHandler();
    }

    /*
     * Deals with all user input
     */
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
                if (mainMenuPos == num_mainInstruments-1)
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
                    mainMenuPos = num_mainInstruments-1;
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
     * All the GUI objects to be rendered 
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

            //TODO: For loop for every instrument
            //TODO: Display tooltip for "smallMainSel[x]"
            if (mainMenuPos == 0)
            {
                GUI.Box(new Rect(75, Screen.height - 260, 100, 100), smallMainSel[0], GUIStyle.none);
                GUI.Box(new Rect(200, Screen.height - 260, 100, 100), smallMain[1], GUIStyle.none);
            }
            else if (mainMenuPos == 1)
            {
                GUI.Box(new Rect(75, Screen.height - 260, 100, 100), smallMain[0], GUIStyle.none);
                GUI.Box(new Rect(200, Screen.height - 260, 100, 100), smallMainSel[1], GUIStyle.none);
            }
        }

        if (offMenu)
        {
            GUI.Box(new Rect(Screen.width - 350, Screen.height - 250, 300, 75), "");

            //TODO: For loop for every instrument. 
            //TODO: Display tooltip for "smallOffSel[x]"
            if (offMenuPos == 0)
            {
                GUI.Box(new Rect(Screen.width - 325, Screen.height - 260, 100, 100), smallOffSel[0], GUIStyle.none);
                GUI.Box(new Rect(Screen.width - 200, Screen.height - 260, 100, 100), smallOff[1], GUIStyle.none);
            }
            else if (offMenuPos == 1) 
            {
                GUI.Box(new Rect(Screen.width - 325, Screen.height - 260, 100, 100), smallOff[0], GUIStyle.none);
                GUI.Box(new Rect(Screen.width - 200, Screen.height - 260, 100, 100), smallOffSel[1], GUIStyle.none);
            }
           
        }

       /* hearts
        * looks like shit right now so is not displayed.
        * Five boxes between the instruments
        * Should probably be switched to a lifebar? Idfk
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

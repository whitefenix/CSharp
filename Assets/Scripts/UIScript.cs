using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour
{

    public Texture[] mainInstruments, smallMain, smallMainSel, offInstruments, smallOff, smallOffSel;
    public Texture mainHand, offHand;

    private bool mainMenu = false;
    private bool offMenu = false;

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

    void InputHandler()
    {
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
                if (currentMain != mainMenuPos)
                {
                    currentMain = mainMenuPos;
                    mainHand = mainInstruments[currentMain];
                    mainMenuPos = 0;
                }
                mainMenu = false;
            }
            else
            {
                mainMenu = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.H) == true)
        {
            //Turn menu on if it was off, close it if it was on
            if (offMenu == true)
            {
                if (currentOff != offMenuPos)
                {
                    currentOff = offMenuPos;
                    offHand = offInstruments[currentOff];
                    offMenuPos = 0;
                }
                offMenu = false;
            }
            else
            {
                offMenu = true;
            }
        }
    }

    void OnGUI()
    {
        //main hand
        GUI.Box(new Rect(100, Screen.height - 200, 200, 200), mainHand, GUIStyle.none);

        //off hand
        GUI.Box(new Rect(Screen.width - 300, Screen.height - 200, 200, 200), offHand, GUIStyle.none); 

        if (mainMenu)
        {
            GUI.Box(new Rect(50, Screen.height - 250, 300, 75), "");

            //TODO: For loop for every instrument
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

        //hearts
        /*
        GUI.Box(new Rect(Screen.width - 375, Screen.height - 125, 50, 50), "");
        GUI.Box(new Rect(Screen.width - 475, Screen.height - 125, 50, 50), "");
        GUI.Box(new Rect(Screen.width - 575, Screen.height - 125, 50, 50), "");
        GUI.Box(new Rect(Screen.width - 675, Screen.height - 125, 50, 50), "");
        GUI.Box(new Rect(Screen.width - 775, Screen.height - 125, 50, 50), "");
        */
    }
}

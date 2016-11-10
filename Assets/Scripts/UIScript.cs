using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour
{

    public Texture violinExample, harpExample, mainHand, offHand;

    private bool mainHandMenu = false;
    private bool offHandMenu = false;

    private int mainMenuPos = 0, offHandMenuPos = 0;

    void Update()
    {
        InputHandler();
    }

    void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.G) == true)
        {
            //I hate reading one line ifs. If you want them you can change this.
            //Turn menu on if it was off, close it if it was on
            if (mainHandMenu == false)
            {
                mainHandMenu = true;
            }
            else
            {
                mainHandMenu = false;
            }

            if (mainHand == violinExample)
            {
                mainHand = harpExample;
            }
            else
            {
                mainHand = violinExample;
            }
        }
        else if (Input.GetKeyDown(KeyCode.H) == true)
        {
            //Turn menu on if it was off, close it if it was on
            if (offHandMenu == false)
            {
                offHandMenu = true;
            }
            else
            {
                offHandMenu = false;
            }
            if (offHand == violinExample)
            {
                offHand = harpExample;
            }
            else
            {
                offHand = violinExample;
            }
        }
    }

    void OnGUI()
    {
        //main hand
        GUI.Box(new Rect(100, Screen.height - 200, 200, 200), mainHand, GUIStyle.none);

        //off hand
        GUI.Box(new Rect(Screen.width - 300, Screen.height - 200, 200, 200), offHand, GUIStyle.none); 

        if (mainHandMenu)
        {
            GUI.Box(new Rect(50, Screen.height - 250, 300, 75), "");
        }

        if (offHandMenu)
        {
            GUI.Box(new Rect(Screen.width - 350, Screen.height - 250, 300, 75), "");
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

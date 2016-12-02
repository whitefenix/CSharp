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
    public class MainHandInstrument
    {
        //TODO: Maybe implement variables for attack mode? 
        public Texture mainTexture;
        public Texture smallTexture;
        public Texture selectedTexture;
        public string instrumentName;
        public GameObject tooltip;
        public AudioClip clip;
        public RawImage rawImage;

        public PlayerAttack.Type type;
    }

    [System.Serializable]
    public class OffHandInstrument
    {
        //TODO: Maybe implement variables for attack mode? 
        public Texture mainTexture;
        public Texture smallTexture;
        public Texture selectedTexture;
        public string instrumentName;
        public GameObject tooltip;
        public AudioClip clip;
        public RawImage rawImage;

        public PlayerAttack.Perk perk;
    }

    [System.Serializable]
    public class HealthDiamond
    {
        public Texture texture;
        public string diamondName;
        public RawImage rawImage;
    }

    public RawImage MainSlot;
    public RawImage OffSlot;
    public RawImage HealthBar;

    public GameObject mainMenuPanel;
    public GameObject offMenuPanel;

    //main = main hand, the left hand side instrument. off = offhand, the right hand side instrument
    public MainHandInstrument[] mainhandInstruments;
    public OffHandInstrument[] offhandInstruments;
    public HealthDiamond[] healthDiamonds;

    [HideInInspector]
    public MainHandInstrument mainHand; //currently equipped instruments, could maybe be integrated and removed? Change if performance is an issue
    [HideInInspector]
    public OffHandInstrument offHand;

    //true if the menu is open
    private bool mainMenu = false, offMenu = false;

    //position in menu, number of instruments, index of equipped instrument
    private int mainMenuPos = 0, offMenuPos = 0, num_mainInstruments = 2, num_offInstruments = 2, currentMain = 0, currentOff = 0;
        //num_healthDiamonds = 12;

    private PlayerAttack attack;

    public GameObject overlay;
    private Image overlayImage;

    void Start()
    {
        num_offInstruments = offhandInstruments.Length;
        num_mainInstruments = mainhandInstruments.Length;
        mainHand = mainhandInstruments[currentMain];
        offHand = offhandInstruments[currentOff];

       // num_healthDiamonds = healthDiamonds.Length;
        //currentHealth = healthDiamonds[currentDiamond]; //arrayindex out of bounds

        //COMMENT use UI overlay instead - Mr. H

        attack = GetComponent<PlayerAttack>();

        //attack.SetCurrentInstrument(mainHand.type);
        //attack.SetCurrentPerk(offHand.perk);

        overlayImage = overlay.GetComponent<Image>();

        mainMenuPanel.SetActive(false);
        offMenuPanel.SetActive(false);
        //healthBar.SetActive(true);

        //ok?
        //set healthslots to be active?
        updateHealthDiamonds();
    }

    //called every frame
    void Update()
    {
        var tempColor = overlayImage.color;
        if (mainMenu || offMenu)
        {
            Time.timeScale = 0.1f;
            if (tempColor.a < 0.35f)
            {
                tempColor.a += 5.0f * Time.deltaTime;
                overlayImage.color = tempColor;
            }
        }
        else
        {
            Time.timeScale = 1.0f;
            if (tempColor.a > 0.01f)
            {
                tempColor.a -= 0.5f * Time.deltaTime;
                overlayImage.color = tempColor;
            }
        }       
        InputHandler();
        //test - should this really be called in update? Inefficient
        updateHealthDiamonds();
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
            mainHand = mainhandInstruments[currentMain]; //retrieve correct GameObject
            //attack.currentInstrument = mainHand.type;
            attack.SetCurrentInstrument(mainHand.type);
            MainSlot.texture = mainHand.mainTexture; //set texture of main object (a raw image) to the texture of the correct GameObject
            //build 12 slots to be filled?!
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
            attack.SetCurrentPerk(offHand.perk);
            OffSlot.texture = offHand.mainTexture;
        }
        offMenuPos = currentOff;
        offMenu = false;
        offMenuPanel.SetActive(false);
    }

    void updateHealthDiamonds()
    {
        //Get the current health-value
        GameObject thePlayer = GameObject.Find("Player");
        Health playerHealth = thePlayer.GetComponent<Health>();
        float tmpHealth = (playerHealth.health / playerHealth.maximumHealth) * 12;

        //round to closest int
        int pic = (int)Mathf.Round(tmpHealth) - 1;
        if (pic > 11)
        {
            pic = 11;
        }
        if (pic < 0)
        {
            pic = 0;
        }

        //change health bar picture
        HealthDiamond currentHealth = healthDiamonds[pic];
        HealthBar.texture = currentHealth.texture;


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

    bool MainMenuInputTriggered()
    {
        return Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(GlobalConstants.XBOX_BTN_X);
    }

    bool OffMenuInputTriggered()
    {
        return Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(GlobalConstants.XBOX_BTN_B);
    }

    bool NavLeftInputTriggered()
    {
        return Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(GlobalConstants.XBOX_BTN_LB);
    }

    bool NavRightInputTriggered()
    {
        return Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(GlobalConstants.XBOX_BTN_RB);
    }

    bool CloseMenuInputTriggered()
    {
        return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(GlobalConstants.XBOX_BTN_Y);
    }
}
///Author(s): Samuel Ekne, Julia von Heijne
///Date: 10-11-2016
///Last revision: 16-11-2016

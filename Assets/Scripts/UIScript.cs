using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    public bool isDead = false;
    public bool isDone = false;

    [System.Serializable]
    public class MainHandInstrument
    {
        //TODO: Maybe implement variables for attack mode? 
        public Sprite mainSprite;
        public Sprite smallSprite;
        public Sprite selectedSprite;
        public string instrumentName;
        public GameObject tooltip;
        public AudioClip clip;
        public Image image;

        public PlayerAttack.Type type;
    }

    [System.Serializable]
    public class OffHandInstrument
    {
        //TODO: Maybe implement variables for attack mode? 
        public Sprite mainSprite;
        public Sprite smallSprite;
        public Sprite selectedSprite;
        public string instrumentName;
        public GameObject tooltip;
        public AudioClip clip;
        public Image image;

        public PlayerAttack.Perk perk;
    }

    public Image MainSlot;
    public Image OffSlot;
    public RawImage HealthBar;

    public GameObject mainMenuPanel;
    public GameObject offMenuPanel;

	private GameObject offensiveSkillBook1;
	private GameObject offensiveSkillBook2;
	private GameObject defensiveSkillBook1;
	private GameObject defensiveSkillBook2;
	private GameObject offensiveSkillBook1TT;
	private GameObject offensiveSkillBook2TT;
	private GameObject defensiveSkillBook1TT;
	private GameObject defensiveSkillBook2TT;

	private GameObject overlay;
	private GameObject pauseMenu;
    private GameObject continueMenu;
	private GameObject gameOverMenu;

    PlayerController control;
    PlayerAttack attacking;

    PlayerQuests quests;

    //main = main hand, the left hand side instrument. off = offhand, the right hand side instrument
    public MainHandInstrument[] mainhandInstruments;
    public OffHandInstrument[] offhandInstruments;
    public Texture[] healthDiamonds;

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
    private PlayerSkills skills;

    void Start()
    {
        num_offInstruments = offhandInstruments.Length;
        num_mainInstruments = mainhandInstruments.Length;
        mainHand = mainhandInstruments[currentMain];
        offHand = offhandInstruments[currentOff];

       // num_healthDiamonds = healthDiamonds.Length;
        //currentHealth = healthDiamonds[currentDiamond]; //arrayindex out of bounds

		overlay = GameObject.Find("Canvas/Overlay");
		overlay.SetActive (false);

		pauseMenu = GameObject.Find ("Canvas/PauseMenu");
		pauseMenu.SetActive (false);

        continueMenu = GameObject.Find("Canvas/ContinueMenu");
        continueMenu.SetActive(false);

        control = GetComponent<PlayerController>();
        attack = GetComponent<PlayerAttack>();

		gameOverMenu = GameObject.Find ("Canvas/GameOver");
		gameOverMenu.SetActive (false);

		offensiveSkillBook1 = GameObject.Find("Canvas/Panel/OffensivePowerUps/SkillBook1");
		offensiveSkillBook1TT = GameObject.Find("Canvas/Panel/OffensivePowerUps/SkillBook1/ToolTip");
		offensiveSkillBook1.SetActive (false);
		offensiveSkillBook2 = GameObject.Find("Canvas/Panel/OffensivePowerUps/SkillBook2");
		offensiveSkillBook2TT = GameObject.Find("Canvas/Panel/OffensivePowerUps/SkillBook2/ToolTip");
		offensiveSkillBook2.SetActive (false);
		defensiveSkillBook1 = GameObject.Find("Canvas/Panel/DefensivePowerUps/SkillBook1");
		defensiveSkillBook1TT = GameObject.Find("Canvas/Panel/DefensivePowerUps/SkillBook1/ToolTip");
		defensiveSkillBook1.SetActive (false);
		defensiveSkillBook2 = GameObject.Find("Canvas/Panel/DefensivePowerUps/SkillBook2");
		defensiveSkillBook2TT = GameObject.Find("Canvas/Panel/DefensivePowerUps/SkillBook2/ToolTip");
		defensiveSkillBook2.SetActive (false);

        attacking = GetComponent<PlayerAttack>();
		skills = GetComponent<PlayerSkills>();

        //attack.SetCurrentInstrument(mainHand.type);
        //attack.SetCurrentPerk(offHand.perk);

        mainMenuPanel.SetActive(false);
        offMenuPanel.SetActive(false);
        //healthBar.SetActive(true);

        quests = GetComponent<PlayerQuests>();

        //ok?
        //set healthslots to be active?
        updateHealthDiamonds();
    }

    //called every frame
    void Update()
    {
        //var tempColor = overlayImage.color;
        if (mainMenu || offMenu)
        {
            Time.timeScale = 0.1f;
//            if (tempColor.a < 0.35f)
//            {
//                tempColor.a += 5.0f * Time.deltaTime;
//                overlayImage.color = tempColor;
//            }
        }
        else
        {
            Time.timeScale = 1.0f;
//            if (tempColor.a > 0.01f)
//            {
//                tempColor.a -= 0.5f * Time.deltaTime;
//                overlayImage.color = tempColor;
//            }
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

			ShowSkillBookToolTips (mainMenu || offMenu); 
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
			overlay.SetActive(true);

			ShowSkillBookToolTips (mainMenu || offMenu); 
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
			overlay.SetActive(true);

			ShowSkillBookToolTips (mainMenu || offMenu); 
        }

		if (PauseInputTriggered () == true) 
		{
			if (pauseMenu.activeInHierarchy == true) 
			{
				closePauseMenu ();
			} 
			else if (pauseMenu.activeInHierarchy == false) 
			{
				openPauseMenu ();
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
            MainSlot.sprite = mainHand.mainSprite; //set texture of main object (a raw image) to the texture of the correct GameObject
            //build 12 slots to be filled?!
        }
        mainMenuPos = currentMain;
        mainMenu = false;
        mainMenuPanel.SetActive(false);

		overlay.SendMessage("FadeOut");
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
            OffSlot.sprite = offHand.mainSprite;
        }
        offMenuPos = currentOff;
        offMenu = false;
        offMenuPanel.SetActive(false);

		overlay.SendMessage("FadeOut");
    }

	public void openPauseMenu()
	{
		if (mainMenu)
		{
			closeMainMenu();
		}
		else if (offMenu)
		{
			closeOffMenu();
		}

		if (!pauseMenu.activeInHierarchy) 
		{
			Time.timeScale = 0.0f;
			pauseMenu.SetActive (true);
			GameObject.Find ("Canvas/PauseMenu/MenuButtons/Resume").GetComponent<Button> ().Select ();
		}
	}

	public void closePauseMenu()
	{
		pauseMenu.SetActive (false);
		Time.timeScale = 1.0f;
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
		HealthBar.texture = healthDiamonds[pic];
    }

    /*
     * All the GUI objects to be rendered. Also called every frame.
     */
    void OnGUI()
    {
        if (quests.mainQuestCount == 0 && !isDone)
        {
            isDone = true;
        }

        if(isDone){
            onDone();
            control.enabled = false;
            attacking.enabled = false;
            Time.timeScale = 0.1f;
            OverlayFade fade = overlay.GetComponent<OverlayFade>();
            overlay.SetActive(true);
            fade.overlayTransparency = 1.0f;
            fade.speed = 3;
            if (!fade.fadeIn) //when we are done fading in
            {
                if (!continueMenu.activeInHierarchy)
                {
                    continueMenu.SetActive(true);
                    //GameObject.Find("Canvas/GameOver/MenuButtons/Restart").GetComponent<Button>().Select();
                    Time.timeScale = 0.0f;
                    control.enabled = true;
                    attacking.enabled = true;
                }
            }
        }

        if (isDead)
        {
            control.enabled = false;
            attacking.enabled = false;
            Time.timeScale = 0.1f;
            OverlayFade fade = overlay.GetComponent<OverlayFade>();
            overlay.SetActive(true);
            fade.overlayTransparency = 1.0f;
            fade.speed = 3;
            if (!fade.fadeIn) //when we are done fading in
            {       
				//do it once!
				if (!gameOverMenu.activeInHierarchy) 
				{
					gameOverMenu.SetActive (true);
					GameObject.Find ("Canvas/GameOver/MenuButtons/Restart").GetComponent<Button> ().Select ();

					Time.timeScale = 0.0f;
					control.enabled = true;
					attacking.enabled = true;
				}
            }
            //trigger fadeout
            //if fadeout is done
                //trigger gameoverscreen and pause
        }
        if (mainMenu) //if mainhand menu is currently open
        {
            mainMenuPanel.SetActive(true);
            for (int i = 0; i < num_mainInstruments; i++)
            {
                if (mainMenuPos == i) //if selected display selected texture/sprite
                {
                    mainhandInstruments[i].image.sprite = mainhandInstruments[i].selectedSprite;
                    mainhandInstruments[i].tooltip.SetActive(true);
                }
                else
                {
                    mainhandInstruments[i].image.sprite = mainhandInstruments[i].smallSprite;
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
                    offhandInstruments[i].image.sprite = offhandInstruments[i].selectedSprite;
                    offhandInstruments[i].tooltip.SetActive(true);
                }
                else //This instrument i is not selected
                {
                    offhandInstruments[i].image.sprite = offhandInstruments[i].smallSprite;
                    offhandInstruments[i].tooltip.SetActive(false);
                }
            }
        }
    }

	private void ShowSkillBookToolTips(bool show = true)
	{
		offensiveSkillBook1TT.SetActive (show);
		offensiveSkillBook2TT.SetActive (show);
		defensiveSkillBook1TT.SetActive (show);
		defensiveSkillBook2TT.SetActive (show);
	}

	public void OnEqipSkillBook () 
	{
		offensiveSkillBook1.SetActive (false);
		offensiveSkillBook2.SetActive (false);
		defensiveSkillBook1.SetActive (false);
		defensiveSkillBook2.SetActive (false);

		ShowSkillBookToolTips (true);

		foreach (BonusItem bi in skills.bonusItems)
		{
			if (bi.type == SkillBookType.OFFENSIVE) 
			{
				if (!offensiveSkillBook1.activeInHierarchy) 
				{
					offensiveSkillBook1.SetActive (true);
					offensiveSkillBook1TT.GetComponentInChildren<Text> ().text = string.Format ("<b>{0}</b>\n{1}", bi.name, bi.toolTipDescription);
					offensiveSkillBook1TT.SetActive (false);
				}
				else if (!offensiveSkillBook2.activeInHierarchy) 
				{
					offensiveSkillBook2.SetActive (true);
					offensiveSkillBook2TT.GetComponentInChildren<Text> ().text = string.Format ("<b>{0}</b>\n{1}", bi.name, bi.toolTipDescription);
					offensiveSkillBook2TT.SetActive (false);
				}
			} 
			else
			{
				if (!defensiveSkillBook1.activeInHierarchy) 
				{
					defensiveSkillBook1.SetActive (true);
					defensiveSkillBook1TT.GetComponentInChildren<Text> ().text = string.Format ("<b>{0}</b>\n{1}", bi.name, bi.toolTipDescription);
					defensiveSkillBook1TT.SetActive (false);
				}
				else if (!defensiveSkillBook2.activeInHierarchy) 
				{
					defensiveSkillBook2.SetActive (true);
					defensiveSkillBook2TT.GetComponentInChildren<Text> ().text = string.Format ("<b>{0}</b>\n{1}", bi.name, bi.toolTipDescription);
					defensiveSkillBook2TT.SetActive (false);
				}
			}
		}


	}

	public void OnDeath()
	{
        isDead = true; //trigger fadeout
        MusicScript music = GetComponent<MusicScript>();
        music.isDead = true;
	}

    public void onDone()
    {
        MusicScript music = GetComponent<MusicScript>();
        music.isDone = true;
    }

	public void OnLevelComplete()
	{
		//TODO show outro movie
		Debug.Log("Main Quests finished");
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
        return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(GlobalConstants.XBOX_BTN_BACK);
    }

	bool PauseInputTriggered()
	{
		return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(GlobalConstants.XBOX_BTN_START);
	}
}
///Author(s): Samuel Ekne, Julia von Heijne
///Date: 10-11-2016
///Last revision: 16-11-2016

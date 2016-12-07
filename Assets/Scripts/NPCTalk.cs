using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCTalk : MonoBehaviour {

	[Header("NPC:")]
	public string npcName;
	public string[] beforeQuestConversation;
	public string[] afterQuestConversation;

	[Header("Speech Bubble:")]
	public GameObject textBubble;
	public int textSortingOrder = 100;

	[Header("Quests:")]
	public PlayerQuests.QuestItem quest;

	[Header("Reward:")]
	public BonusItem reward;

	private TextMesh text;
	private int textIdx = 0;
	private bool questGiven = false;
	private string[] currentConversation;

	public bool mute = false;

	// Use this for initialization
	void Start () 
	{
		text = textBubble.GetComponent<TextMesh> ();

		currentConversation = beforeQuestConversation;

		MeshRenderer mr = textBubble.GetComponent<MeshRenderer> ();
		mr.sortingOrder = textSortingOrder;
	}

	// Update is called once per frame
	void Update () 
	{
	}

	public bool HasGift()
	{
		if (HasQuest()) 
		{
			return quest.questFinished == true && reward != null && reward.isset == true;
		} 
		else 
		{
			return reward != null && reward.isset == true;
		}
	}

	public bool HasOpenQuest()
	{
		return HasQuest() && quest.questFinished == false && questGiven == false;
	}

	public bool HasQuest()
	{
		return quest != null && quest.isset == true;
	}

	public PlayerQuests.QuestItem RequestQuest()
	{
		questGiven = true;
		return quest;
	}

	public bool Talk()
	{
		if (mute) 
		{
			return false;
		}

		gameObject.SendMessage ("OnTalk", SendMessageOptions.DontRequireReceiver);

		if (HasOpenQuest ()) 
		{
			currentConversation = beforeQuestConversation;
		} 
		else if (HasQuest () && quest.questFinished == true && currentConversation != afterQuestConversation) 
		{
			currentConversation = afterQuestConversation;
			textIdx = -1;
		}
		else if (HasQuest () && quest.questFinished == false)
		{
			return false;
		}

		if (textIdx >= currentConversation.Length - 1) 
		{
			text.text = "";

			return false;
		} 
		else 
		{
			text.text = currentConversation[++textIdx];

			return true;
		}
	}

	void OnTriggerEnter (Collider other) 
	{
		if (other.tag == "Player") 
		{
			text.text = currentConversation[textIdx];
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (other.tag == "Player") 
		{
			text.text = "";
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerQuests : MonoBehaviour {

	public List<Quest.QuestItem> quests; 

	// Use this for initialization
	void Start ()
	{
		foreach (Quest.QuestItem q in quests) 
		{
			Quest.PrepareQuest (q);
		}
	}

	public void AddQuest (Quest.QuestItem q) 
	{
		quests.Add (q);
		Quest.PrepareQuest (q);
	}
}

﻿using UnityEngine;
using System.Collections;

public class QuestTarget : MonoBehaviour {

	public enum Type
	{
		//Start with 16 so we can combine it with the quest type to a uniqe int key!
		NONE = 16,
		ENEMY_MINION,
		ENEMY_WALKER,
		ENEMY_GOLEM,
		ITEM_HP_POTION
	}

	public Type targetType;
	private PlayerQuests playerQuests;
	private PlayerQuests.QuestCondition condition;

	// Use this for initialization
	void Start () 
	{
		playerQuests = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerQuests> ();
	}

	public void RegisterQuest(PlayerQuests.QuestCondition qc)
	{
		condition = qc;
	}

	public void OnDeath()
	{
		if (condition != null && condition.action == PlayerQuests.Action.KILL) 
		{
			condition.conditionMet = true;
			playerQuests.CheckQuest (condition.parentItem);
		}

		playerQuests.NotifyTypeQuest (targetType, PlayerQuests.Action.KILL_TYPE);
	}

	public void OnCollected()
	{
		if (condition != null && condition.action == PlayerQuests.Action.COLLECT) 
		{
			condition.conditionMet = true;
			playerQuests.CheckQuest (condition.parentItem);
		}

		playerQuests.NotifyTypeQuest (targetType, PlayerQuests.Action.COLLECT_TYPE);
	}

	public void OnTalk()
	{
		if (condition != null && condition.action == PlayerQuests.Action.TALK) 
		{
			condition.conditionMet = true;
			playerQuests.CheckQuest (condition.parentItem);
		}
	}
}

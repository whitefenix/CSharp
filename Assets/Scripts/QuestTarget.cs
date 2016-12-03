using UnityEngine;
using System.Collections;

public class QuestTarget : MonoBehaviour {

	private PlayerQuests.QuestCondition condition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

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
			PlayerQuests.CheckQuest (condition.parentItem);
		}
	}

	public void OnCollected()
	{
		if (condition != null && condition.action == PlayerQuests.Action.COLLECT) 
		{
			condition.conditionMet = true;
			PlayerQuests.CheckQuest (condition.parentItem);
		}
	}

	public void OnTalk()
	{
		if (condition != null && condition.action == PlayerQuests.Action.TALK) 
		{
			condition.conditionMet = true;
			PlayerQuests.CheckQuest (condition.parentItem);
		}
	}
}

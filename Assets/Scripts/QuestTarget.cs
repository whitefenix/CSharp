using UnityEngine;
using System.Collections;

public class QuestTarget : MonoBehaviour {

	private Quest.QuestCondition condition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void RegisterQuest(Quest.QuestCondition qc)
	{
		condition = qc;
	}

	public void OnDeath()
	{
		if (condition != null && condition.action == Quest.Action.KILL) 
		{
			condition.conditionMet = true;
			Quest.CheckQuest (condition.parentItem);
		}
	}

	public void OnCollected()
	{
		if (condition != null && condition.action == Quest.Action.COLLECT) 
		{
			condition.conditionMet = true;
			Quest.CheckQuest (condition.parentItem);
		}
	}

	public void OnTalk()
	{
		if (condition != null && condition.action == Quest.Action.TALK) 
		{
			condition.conditionMet = true;
			Quest.CheckQuest (condition.parentItem);
		}
	}
}

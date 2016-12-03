using UnityEngine;
using System.Collections;

public class Quest {

	public enum Action {
		COLLECT = 0,
		KILL,
		TALK
	}

	[System.Serializable]
	public class QuestItem
	{
		[Header("General:")]
		[System.NonSerialized]
		public QuestItem predecessor;
		public bool isMainQuest;

		[HideInInspector]
		public bool questFinished;

		[Header("Display:")]
		public string title;
		public string longDescription;

		[Header("Conditions:")]
		public QuestCondition[] conditionList;
	}

	[System.Serializable]
	public class QuestCondition
	{
		//[HideInInspector]
		[System.NonSerialized]
		public QuestItem parentItem;
		[HideInInspector]
		public bool conditionMet;

		public Action action;
		public GameObject target;

		public string shortDescription;
	}

	public static void PrepareQuest(QuestItem q)
	{
		QuestTarget tmp;

		foreach (QuestCondition qc in q.conditionList) 
		{
			qc.parentItem = q;

			if (qc.target != null) 
			{
				tmp = qc.target.GetComponent<QuestTarget> ();

				if (tmp != null) 
				{
					tmp.RegisterQuest (qc);
				} 
				else 
				{
					qc.conditionMet = true;
				}
			} 
			else 
			{
				qc.conditionMet = true;
			}
		}
	}

	public static void CheckQuest(QuestItem q)
	{
		bool questFinished = true;

		foreach (QuestCondition qc in q.conditionList) 
		{
			if (qc.conditionMet == false) 
			{
				questFinished = false;
				break;
			}
		}

		q.questFinished = questFinished;

		if (questFinished)
			Debug.Log (q.title + " COMPLETE!");
	}
}

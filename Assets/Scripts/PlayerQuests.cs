using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerQuests : MonoBehaviour {

	public enum Action {
		COLLECT = 0,
		KILL,
		TALK,
		COLLECT_TYPE,
		KILL_TYPE
	}

	[System.Serializable]
	public class QuestItem
	{
		[Header("General:")]
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

		public string shortDescription;
		public Action action;
		public GameObject target;

		[Header("Type Quests only:")]
		public QuestTarget.Type targetType = QuestTarget.Type.NONE;
		public int targetCount;
		[HideInInspector]
		public int currentCount = 0;
	}

	public List<QuestItem> quests; 
	public Dictionary<uint, List<QuestCondition>> typeQuests;
	public static Text questListText;

	// Use this for initialization
	void Start ()
	{
		typeQuests = new Dictionary<uint, List<QuestCondition>> ();
		questListText = GameObject.Find ("Canvas/Quests/QuestList").GetComponent<Text>();

		foreach (PlayerQuests.QuestItem q in quests) 
		{
			PrepareQuest (q);
		}

		UpdateQuestDisplay ();
	}

	public void AddQuest (PlayerQuests.QuestItem q) 
	{
		quests.Add (q);
		PrepareQuest (q);
	}

	string GetQuestRichtext(QuestItem q)
	{
		string text = "", cPrefix = "", cPostfix = "", count = "";
		if (q.questFinished) 
		{
			cPrefix = "<color=#B0B0B0FF>";
			cPostfix = "</color>";
		}

		text += string.Format("{0}<b>{1}</b>{2}\n", cPrefix, q.title, cPostfix);

		foreach (QuestCondition qc in q.conditionList) 
		{
			cPrefix = "";
			cPostfix = "";
			if (qc.conditionMet) 
			{
				cPrefix = "<color=#B0B0B0FF>";
				cPostfix = "</color>";
			}

			count = "";
			if (qc.action == Action.COLLECT_TYPE || qc.action == Action.KILL_TYPE) 
			{
				count = string.Format("({0}/{1})", qc.currentCount, qc.targetCount);
			}

			text += string.Format ("{0} - <i>{1}</i>{2} {3}\n", cPrefix, qc.shortDescription, count, cPostfix);
		}

		text += "\n";

		return text;
	}

	private void PrepareQuest(QuestItem q)
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
			else if (qc.targetType != QuestTarget.Type.NONE)
			{
				uint key = GetTargetTypeActionKey (qc);

				if (!typeQuests.ContainsKey (key)) 
				{
					typeQuests.Add(key, new List<QuestCondition>());
				} 

				typeQuests[key].Add (qc);
			}
			else 
			{
				qc.conditionMet = true;
			}
		}
	}

	public static uint GetTargetTypeActionKey(QuestCondition qc)
	{
		return GetTargetTypeActionKey(qc.targetType, qc.action);
	}

	public static uint GetTargetTypeActionKey(QuestTarget.Type type, Action action)
	{
		return (uint)type + (uint)action;
	}

	public void CheckQuest(QuestItem q)
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

		UpdateQuestDisplay ();
	}

	public void NotifyTypeQuest(QuestTarget.Type type, Action action)
	{
		var key = GetTargetTypeActionKey (type, action);

		if (typeQuests.ContainsKey (key)) 
		{
			foreach (QuestCondition qc in typeQuests [key]) 
			{
				qc.currentCount++;

				if (qc.currentCount >= qc.targetCount)
				{
					qc.conditionMet = true;
					typeQuests.Remove (key);

					CheckQuest (qc.parentItem);
				}
			}

			UpdateQuestDisplay ();
		}
	}

	public void UpdateQuestDisplay() 
	{
		questListText.text = "";

		foreach (PlayerQuests.QuestItem q in quests) 
		{
			questListText.text += GetQuestRichtext (q);
		}
	}
}

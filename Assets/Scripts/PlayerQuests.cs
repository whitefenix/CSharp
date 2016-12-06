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
		KILL_TYPE,
		TALK_TYPE
	}

	[System.Serializable]
	public class QuestItem
	{
		public bool isset = true;

		[HideInInspector]
		public uint id;

		[Header("General:")]
		public bool isMainQuest;
		public bool mandatoryOrder;

		[HideInInspector]
		public int conditionIdx;

		[HideInInspector]
		public bool questFinished;

		[HideInInspector]
		public bool prepared;

		[Header("Display:")]
		public string title;
		public string longDescription;

		[Header("Conditions:")]
		public QuestCondition[] conditionList;
	}

	[System.Serializable]
	public class QuestCondition
	{
		[HideInInspector]
		public int idx;

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

	private static uint QUEST_ID = 0;
	private List<uint> mainQuestsFinished;
	public int mainQuestCount;

	// Use this for initialization
	void Start ()
	{
		typeQuests = new Dictionary<uint, List<QuestCondition>> ();
		mainQuestsFinished = new List<uint> ();
		questListText = GameObject.Find ("Canvas/Quests/QuestLog/Text").GetComponent<Text>();

		foreach (PlayerQuests.QuestItem q in quests) 
		{
			PrepareQuest (q);
		}

		UpdateQuestDisplay ();
	}

	public void AddQuests (List<QuestItem> qs) 
	{
		foreach (QuestItem q in qs)
		{
			quests.Add (q);
			PrepareQuest (q);
		}
		UpdateQuestDisplay ();
	}

	public void AddQuests (QuestItem q) 
	{
		quests.Add (q);
		PrepareQuest (q);
		UpdateQuestDisplay ();
	}

	string GetQuestRichtext(QuestItem q)
	{
		string text = "", cPrefix = "", cPostfix = "", count = "";
		if (q.isMainQuest) 
		{
			cPrefix = "<color=#ECBE47FF>";
			cPostfix = "</color>";
		} 
		else 
		{
			cPrefix = "<color=#EC479AFF>";
			cPostfix = "</color>";
		}

		//text += string.Format("{0}<b>{1}</b>{2}\n", cPrefix, q.title, cPostfix);
		text += cPrefix + q.title + cPostfix + '\n';

		foreach (QuestCondition qc in q.conditionList) 
		{
			if (q.mandatoryOrder && qc.idx > q.conditionIdx)
				continue;

			cPrefix = "";
			cPostfix = "";
			if (qc.conditionMet) 
			{
				cPrefix = "<color=#B0B0B0FF>";
				cPostfix = "</color>";
			} 

			count = "";
			if (qc.action == Action.COLLECT_TYPE || qc.action == Action.KILL_TYPE || qc.action == Action.TALK_TYPE) 
			{
				count = string.Format("({0}/{1})", qc.currentCount, qc.targetCount);
			}

			//text += string.Format ("{0} - <i>{1}</i>{2} {3}\n", cPrefix, qc.shortDescription, count, cPostfix);
			text += string.Format ("{0} - {1} {2} {3}\n", cPrefix, qc.shortDescription, count, cPostfix);
		}

		return text;
	}

	private void PrepareQuest(QuestItem q)
	{
		if (q.isMainQuest) 
		{
			q.id = QUEST_ID++;
		}	

		QuestTarget tmp;
		int idx = 0;

		foreach (QuestCondition qc in q.conditionList) 
		{
			qc.parentItem = q;
			qc.idx = idx++;

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

		q.prepared = true;
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
			if (qc.conditionMet == false) {
				questFinished = false;
				break;
			} 
			else if (qc.conditionMet == true && q.conditionIdx <= qc.idx)
			{
				q.conditionIdx = qc.idx + 1;
			}
		}

		q.questFinished = questFinished;

		if (q.isMainQuest && q.questFinished && !mainQuestsFinished.Contains(q.id)) 
		{
			mainQuestsFinished.Add (q.id);
			mainQuestCount--;

			if (mainQuestCount == 0) 
			{
				gameObject.SendMessage ("OnLevelComplete", SendMessageOptions.DontRequireReceiver);
			}
		}

		UpdateQuestDisplay ();
	}

	public void NotifyTypeQuest(QuestTarget.Type type, Action action)
	{
		var key = GetTargetTypeActionKey (type, action);

		if (typeQuests.ContainsKey (key)) 
		{
			List<QuestCondition> tmpToRemove = new List<QuestCondition> ();

			foreach (QuestCondition qc in typeQuests [key]) 
			{
				qc.currentCount++;

				if (qc.currentCount >= qc.targetCount)
				{
					qc.conditionMet = true;

					tmpToRemove.Add (qc);

					CheckQuest (qc.parentItem);
				}
			}

			foreach (QuestCondition qc in tmpToRemove) 
			{
				typeQuests[key].Remove(qc);
			}

			UpdateQuestDisplay ();
		}
	}

	public void NotifyQuestCondition(QuestCondition qc)
	{
		if (PlayerQuests.CorrectOrder (qc)) 
		{
			qc.conditionMet = true;
			CheckQuest (qc.parentItem);
		}
	}

	public static bool CorrectOrder(QuestCondition qc)
	{
		if (qc.parentItem.mandatoryOrder) 
		{
			return qc.idx == qc.parentItem.conditionIdx;
		} 
		else 
		{
			return true;
		}
	}

	public void UpdateQuestDisplay() 
	{
		questListText.text = "";

		foreach (PlayerQuests.QuestItem q in quests) 
		{
			if (!q.questFinished) 
			{
				questListText.text += GetQuestRichtext (q);
			}
		}
	}
}

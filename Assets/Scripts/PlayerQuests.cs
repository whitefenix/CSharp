using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerQuests : MonoBehaviour {

	public enum Action {
		COLLECT = 0,
		KILL,
		TALK
	}

	[System.Serializable]
	public class QuestItem
	{
		[System.NonSerialized]
		public PlayerQuests owner;

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

		public Action action;
		public GameObject target;

		public string shortDescription;
	}

	public List<PlayerQuests.QuestItem> quests; 
	public static Text questListText;

	// Use this for initialization
	void Start ()
	{
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
		string text = "";

		if (q.questFinished)
			text += "<color=#B0B0B0FF>";

		text += "<b>" + q.title + "</b>";

		if(q.questFinished)
			text += "</color>";

		text += "\n";

		foreach (QuestCondition qc in q.conditionList) 
		{
			text += " - <i>";

			if (qc.conditionMet)
				text += "<color=#B0B0B0FF>";

			text += qc.shortDescription;

			if(qc.conditionMet)
				text += "</color>";

			text += "</i>\n";
		}

		text += "\n";

		return text;
	}

	private void PrepareQuest(QuestItem q)
	{
		q.owner = this;

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

		q.owner.UpdateQuestDisplay ();
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

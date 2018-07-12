using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubjectGroupWatchCount: MonoBehaviour 
{
	public UILabel  [] subjectList;
	void Awake()
	{		
		actorSubjectUpdate = EventManager.Subscribe(PlayerDataPublisher.NAME + ":" + PlayerDataPublisher.EVENT_SUBJECT_UPDATE);
		actorSubjectUpdate.Handler = delegate (object[] args)
		{
			UpdateSubject();
		};
		
		UpdateSubject();
	}
		
	void OnDestroy()
	{
		if (null != actorSubjectUpdate)
			actorSubjectUpdate.Unsubscribe();
		actorSubjectUpdate = null;
	}
	
	void UpdateSubject()
	{
		
		PlayerData playerData = Globals.Instance.MGameDataManager.MActorData;
		
		List<int>  subjectPropertyValue = new List<int>();
		subjectPropertyValue.Clear();
		subjectPropertyValue.Add(playerData.SubjectData.Literature);
		subjectPropertyValue.Add(playerData.SubjectData.Math);
		subjectPropertyValue.Add(playerData.SubjectData.Language);
		subjectPropertyValue.Add(playerData.SubjectData.Technology);
		subjectPropertyValue.Add(playerData.SubjectData.Sports);
		subjectPropertyValue.Add(playerData.SubjectData.Art);
		
		for (int i=0;i<subjectList.GetLength(0);i++)
		{
			subjectList[i].text = subjectPropertyValue[i].ToString();
		}
	}
	
	
	ISubscriber actorSubjectUpdate = null;
}
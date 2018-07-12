using UnityEngine;
using System.Collections;

public class SubjectGroup : MonoBehaviour 
{
	public UILabel LiterateText;
	public UILabel MathText;
	public UILabel ArtText;
	public UILabel TechnologText;
	public UILabel SportText;
	public UILabel EnglishText;
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
		LiterateText.text = Globals.Instance.MGameDataManager.MActorData.SubjectData.Literature.ToString();
		MathText.text = Globals.Instance.MGameDataManager.MActorData.SubjectData.Math.ToString();
		EnglishText.text = Globals.Instance.MGameDataManager.MActorData.SubjectData.Language.ToString();
		TechnologText.text = Globals.Instance.MGameDataManager.MActorData.SubjectData.Technology.ToString();
		SportText.text = Globals.Instance.MGameDataManager.MActorData.SubjectData.Sports.ToString();
		ArtText.text = Globals.Instance.MGameDataManager.MActorData.SubjectData.Art.ToString();
	}
	
	
	ISubscriber actorSubjectUpdate = null;
}
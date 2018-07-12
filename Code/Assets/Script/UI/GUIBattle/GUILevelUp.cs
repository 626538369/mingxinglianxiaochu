using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GUILevelUp : GUIWindow
{		
	public delegate void OnCloseLevelUpEvent();
	[HideInInspector] public event GUILevelUp.OnCloseLevelUpEvent CloseLevelUpEvent = null;
	
	public GameObject Eff_LevelUp;
	
	public GameObject LevelUpReward;
	public UILabel OldLevel;
	public UILabel NewLevel;
	public UILabel ActLabel;
	public UILabel CharmLabel;
	public UILabel EnergyLabel;
	public UILabel OpenAreasLabel;
	public UIButton BtnNull;

	protected override void Awake()
	{
		if (null == Globals.Instance.MGUIManager)
			return;
		base.Awake();
		NGUITools.SetActive(LevelUpReward.gameObject,false);
		UIEventListener.Get(BtnNull.gameObject).onClick += OnClickBtnNull;
	}
	protected virtual void Start ()
	{
		base.Start();
	}
	
	public override void InitializeGUI ()
	{
		if(_mIsLoaded){
			return;
		}
		_mIsLoaded = true;
		base.GUILevel = 20;
		
		Eff_LevelUp.GetComponent<ParticleSystem>().Play();
	
		StartCoroutine(WaitTime(Eff_LevelUp.GetComponent<ParticleSystem>().duration-3.0f));
	}
	
	void Update()
	{
		base.Update();
	}
	
	IEnumerator WaitTime(float time)
	{
		yield return new WaitForSeconds(time);  
		
		ShowRewardInformation();
	}
	
	void ShowRewardInformation()
	{
		NGUITools.SetActive(LevelUpReward.gameObject,true);
		sg.GS2C_Role_Level_Up_Res res = (sg.GS2C_Role_Level_Up_Res)Globals.Instance.MTeachManager.m_bufferRoleLevelUpPacket.m_object;
		System_Property Property = Globals.Instance.MDataTableManager.GetConfig<System_Property>();
		
		OldLevel.text = res.oldRoleRank.ToString();
		NewLevel.text = res.newRoleRank.ToString();
		ActLabel.text = Globals.Instance.MDataTableManager.GetWordText(2002)+Property.GetConfValue(47)*(res.newRoleRank - res.oldRoleRank);
		CharmLabel.text = Globals.Instance.MDataTableManager.GetWordText(2003)+Property.GetConfValue(48)*(res.newRoleRank - res.oldRoleRank);
		EnergyLabel.text = Globals.Instance.MDataTableManager.GetWordText(2004)+Property.GetConfValue(21)*(res.newRoleRank - res.oldRoleRank);
		
		Globals.Instance.MTeachManager.m_bufferRoleLevelUpPacket = null;
	}
	
	void OnClickBtnNull(GameObject obj)
	{
		if(CloseLevelUpEvent != null)
		{
			CloseLevelUpEvent();
		}
		this.Close();
	}
}



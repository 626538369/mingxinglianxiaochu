using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class WealthGroup : MonoBehaviour 
{
	public delegate void WealthEventDelegate();
	[HideInInspector] public event WealthGroup.WealthEventDelegate wealthGroupEventDelegate = null;
	
	public UIButton Diamond;
	public UIButton EnergySprite;
	public UIButton Money;
	public UIButton Fans;
	
	public UILabel FansText;
	public UILabel diamondText;
	public UILabel moneyText;
	public UILabel jingliText;
	public UILabel ReplyCountDown;
	public GameObject CollectRewardItem;
	
	string[] mIconName = {"IconFensi","IconJinqian","IconZuanshi"};
	Vector3[] mTargetLocation = {new Vector3(55,-130,0),new Vector3(600,-130,0),new Vector3(1090,-130,0)};
	private PlayerData	playerData;
	
	bool UpdateNow = true;
	int mBaseMoney;
	int mBaseDiamond;
	int mBaseFans;
	int mAddSpeed = 1;
	
	public bool ExtraUpdateDiamond = true;
	
	bool mUpdateMoney = false;
	bool mUpdateFans = false;
	bool mUpdateDiamond = false;
	bool UpdateEnd = false;
	int mBeforeUpgrading = 0;
	int DisplayLevel;
	int LevelNeedExp;
	public GameObject GradeInformation;
	public UISlider GradeSlider;
	public UILabel GradeLabel;
	
	
	void Awake()
	{
		playerData = Globals.Instance.MGameDataManager.MActorData;
		
		Diamond.transform.localScale = Vector3.one;
		EnergySprite.transform.localScale = Vector3.one;
		Fans.transform.localScale = Vector3.zero;
		Money.transform.localScale = Vector3.one;
		SetFansProgress(false);
		showJingLi();
		UIEventListener.Get(Diamond.gameObject).onClick += AddDiamond;
		UIEventListener.Get(EnergySprite.gameObject).onClick += AddJingLi;
		UIEventListener.Get(Money.gameObject).onClick += AddMoney;
		UIEventListener.Get(Fans.gameObject).onClick += AddFans;
	}
	
	void Start()
	{
		actorWealthUpdate = EventManager.Subscribe(PlayerDataPublisher.NAME + ":" + PlayerDataPublisher.EVENT_WORTH_UPDATE);
		actorWealthUpdate.Handler = delegate (object[] args)
		{
			if(UpdateNow)
			{
				UpdateWealth();
			}
		};
		UpdateWealth();
	}
	
	void OnDestroy()
	{
		if (null != actorWealthUpdate)
			actorWealthUpdate.Unsubscribe();
		actorWealthUpdate = null;
	}
	
	void UpdateWealth()
	{
		GradeLabel.text = Globals.Instance.MDataTableManager.GetWordText(2007)+playerData.BasicData.Level.ToString();
		GradeSlider.value = (playerData.BasicData.CurrentExp*1.0f)/playerData.BasicData.NextLevelExp;
		mBaseMoney = playerData.WealthData.Money;
		mBaseFans = playerData.RankData.CurrentRankFeat;
		
		mBeforeUpgrading = playerData.BasicData.PreCurrentExp;
		if(ExtraUpdateDiamond)
		{
			diamondText.text = playerData.WealthData.GoldIngot.ToString("N0");
			mBaseDiamond = playerData.WealthData.GoldIngot;
		}
		moneyText.text = playerData.WealthData.Money.ToString("N0");
		jingliText.text = playerData.WealthData.Oil + "/"+ playerData.WealthData.MaxOil;	
		FansText.text = playerData.RankData.CurrentRankFeat.ToString("N0");
		
		AddJingLiCountDown();
		
	}
	
	void AddJingLiCountDown()
	{
		if(playerData.WealthData.incEnergyRemainTime > 0)
		{
			ReplyCountDown.gameObject.transform.localScale = Vector3.one;
		}else
		{
		
			ReplyCountDown.gameObject.transform.localScale = Vector3.zero;
		}
	}
	
	public void Update()
	{
		if(playerData.WealthData.incEnergyRemainTime > 0)
		{
			ReplyCountDown.text = "0"+((int)playerData.WealthData.incEnergyRemainTime)/60+":"+((int)playerData.WealthData.incEnergyRemainTime)%60/10+((int)playerData.WealthData.incEnergyRemainTime)%60%10;
		}
		
		if(mUpdateMoney)
		{
			mBaseMoney += mAddSpeed;
			if(mBaseMoney >= playerData.WealthData.Money)
			{
				mUpdateMoney = false;
				moneyText.text = playerData.WealthData.Money.ToString("N0");
			}else
				moneyText.text = mBaseMoney.ToString("N0");
		}
		if(mUpdateFans)
		{
			mBaseFans += mAddSpeed;
			if(mBaseFans >= playerData.RankData.CurrentRankFeat)
			{
				mUpdateFans = false;
				FansText.text = playerData.RankData.CurrentRankFeat.ToString("N0");
			}else
				FansText.text = mBaseFans.ToString("N0");
		}
		if(mUpdateDiamond && ExtraUpdateDiamond)
		{
			mBaseDiamond += mAddSpeed;
			if(mBaseDiamond >= playerData.WealthData.GoldIngot)
			{
				mUpdateDiamond = false;
				diamondText.text = playerData.WealthData.GoldIngot.ToString("N0");
			}
			else
				diamondText.text = mBaseDiamond.ToString("N0");
				
		}
		if(UpdateEnd)
		{
//			if(!mUpdateDiamond&&!mUpdateFans&&!mUpdateMoney)
//			{
			UpdateEnd = false;
//			UpdateWealth();
			SetFansProgress(true);
			if(mBeforeUpgrading != playerData.RankData.CurrentRankFeat||Globals.Instance.MTeachManager.m_bufferRoleLevelUpPacket != null)
			{
				Role_Rank_Feat roleRank = Globals.Instance.MDataTableManager.GetConfig<Role_Rank_Feat>();
				if (Globals.Instance.MTeachManager.m_bufferRoleLevelUpPacket != null)
				{
					sg.GS2C_Role_Level_Up_Res res = (sg.GS2C_Role_Level_Up_Res)Globals.Instance.MTeachManager.m_bufferRoleLevelUpPacket.m_object;
					DisplayLevel = res.oldRoleRank;
					LevelNeedExp = roleRank.GetCurrentGradeExp(res.oldRoleRank) - roleRank.GetCurrentGradeExp(res.oldRoleRank-1);
					GradeLabel.text = Globals.Instance.MDataTableManager.GetWordText(2007)+res.oldRoleRank.ToString();
				}else
				{
					DisplayLevel = playerData.BasicData.Level;
					LevelNeedExp = playerData.BasicData.NextLevelExp;
				}
				InvokeRepeating("GainExperience",0f,0.01f);
			}else
			{
				if(wealthGroupEventDelegate != null)
				{
					wealthGroupEventDelegate();
					UpdateWealth();
				}
			}
//			}
		}
	}
	
	void GainExperience()
	{
		if(Globals.Instance.MTeachManager.m_bufferRoleLevelUpPacket != null)
		{
			mBeforeUpgrading+= LevelNeedExp/100 > 1 ?LevelNeedExp/100:1;
			GradeSlider.value = (mBeforeUpgrading*1.0f)/LevelNeedExp;
			if(mBeforeUpgrading >= LevelNeedExp)
			{
				sg.GS2C_Role_Level_Up_Res res = (sg.GS2C_Role_Level_Up_Res)Globals.Instance.MTeachManager.m_bufferRoleLevelUpPacket.m_object;
				DisplayLevel = res.newRoleRank;
				Role_Rank_Feat roleRank = Globals.Instance.MDataTableManager.GetConfig<Role_Rank_Feat>();
				LevelNeedExp = playerData.BasicData.NextLevelExp;
				GradeLabel.text = Globals.Instance.MDataTableManager.GetWordText(2007)+res.newRoleRank.ToString();
				mBeforeUpgrading = 0;
				GradeSlider.value = 0f;
				CancelInvoke("GainExperience");
				InvokeRepeating("UpgradeGainExperience",0f,0.01f);
			}
			
		}else
		{
			mBeforeUpgrading+=  (playerData.BasicData.CurrentExp-playerData.BasicData.PreCurrentExp)/150 > 1 ?(playerData.BasicData.CurrentExp-playerData.BasicData.PreCurrentExp)/150:1;
			if(mBeforeUpgrading >= playerData.BasicData.CurrentExp)
			{
				GradeSlider.value = (playerData.BasicData.CurrentExp*1.0f)/LevelNeedExp;
				CancelInvoke("GainExperience");
				if(wealthGroupEventDelegate != null)
				{
					wealthGroupEventDelegate();
					UpdateWealth();
				}
			}else
			{
				GradeSlider.value = (mBeforeUpgrading*1.0f)/LevelNeedExp;
			}
		}
	}
	void UpgradeGainExperience()
	{
		mBeforeUpgrading+= playerData.BasicData.CurrentExp/70 > 1 ?playerData.BasicData.CurrentExp/70:1;
		if(mBeforeUpgrading >= playerData.BasicData.CurrentExp)
		{
			GradeSlider.value = (playerData.BasicData.CurrentExp*1.0f)/LevelNeedExp;
			CancelInvoke("UpgradeGainExperience");
			
			Globals.Instance.MGUIManager.CreateWindow<GUILevelUp>(delegate(GUILevelUp LevelUp){
				
				LevelUp.CloseLevelUpEvent += delegate(){
					if(wealthGroupEventDelegate != null)
					{
						wealthGroupEventDelegate();
						UpdateWealth();
					}
				};
			});
			
		}else
		{
			GradeSlider.value = (mBeforeUpgrading*1.0f)/LevelNeedExp;
		}
	}
	
	
	
	public void EffectsPlay(Vector3[] mInitialLocation,int[] mIconType, WealthGroup.WealthEventDelegate complete = null)
	{
		if(Globals.Instance.MTeachManager.m_bufferRoleLevelUpPacket == null&&mBaseFans == playerData.RankData.CurrentRankFeat&&mBaseMoney >= playerData.WealthData.Money&&mBaseDiamond >= playerData.WealthData.GoldIngot)
		{
			UpdateEnd = true;
		}else
		{
			GenerateIcon(mInitialLocation,mIconType);
		}
		
		if(complete != null)
		{
			wealthGroupEventDelegate = complete;
		}
	}
	
	void GenerateIcon(Vector3[] mLocation,int[] mIconType)
	{
		for(int i = 0; i < mLocation.Length; i++)
		{
			for(int j = 0; j < 5; j++)
			{
				GameObject item = GameObject.Instantiate(CollectRewardItem)as GameObject;
				item.transform.parent = this.transform;
				item.transform.localPosition = mLocation[i];
				item.transform.localScale = Vector3.one;
				item.name = "" + mIconType[i];
				item.GetComponent<UISprite>().spriteName = mIconName[mIconType[i]];
				TweenPosition tween =  TweenPosition.Begin(item.gameObject,0.6f,mTargetLocation[mIconType[i]]);
				tween.delay = j*0.2f;
				
				switch(mIconType[i])
				{
					case 0:
						if(mBaseFans == playerData.RankData.CurrentRankFeat&&Globals.Instance.MTeachManager.m_bufferRoleLevelUpPacket == null)
						{
							Destroy(item);
						}
						break;
					case 1:
						if(mBaseMoney >= playerData.WealthData.Money)
						{
							Destroy(item);
						}
						break;
					case 2:
						if(mBaseDiamond >= playerData.WealthData.GoldIngot)
						{
							Destroy(item);
						}
						break;
				}
				
				if(j == 4)
				{
					EventDelegate.Add(tween.onFinished,delegate()
					{
						switch(item.name)
						{
							case "0":
								iTween.ShakePosition(Fans.gameObject,new Vector3(5,5,0),0.6f);
								break;
							case "1":
								iTween.ShakePosition(Money.gameObject,new Vector3(5,5,0),0.6f);
								break;
							case "2":
								iTween.ShakePosition(Diamond.gameObject,new Vector3(5,5,0),0.6f);
								break;
						}
						Destroy(item);
						if(mBaseFans != playerData.RankData.CurrentRankFeat||Globals.Instance.MTeachManager.m_bufferRoleLevelUpPacket != null)
						{
							mUpdateFans = true;
							UpdateEnd = true;
							mBeforeUpgrading = playerData.BasicData.PreCurrentExp;
							mAddSpeed = (playerData.RankData.CurrentRankFeat - mBaseFans)/70 > mAddSpeed? (playerData.RankData.CurrentRankFeat - mBaseFans)/70:mAddSpeed;
						}
						if(mBaseMoney < playerData.WealthData.Money)
						{
							mUpdateMoney = true;
							UpdateEnd = true;
							mAddSpeed = (playerData.WealthData.Money - mBaseMoney)/70 > mAddSpeed? (playerData.WealthData.Money - mBaseMoney)/70:mAddSpeed;
						}
						if(mBaseDiamond < playerData.WealthData.GoldIngot&&ExtraUpdateDiamond)
						{
							mUpdateDiamond = true;
							UpdateEnd = true;
							mAddSpeed = (playerData.WealthData.GoldIngot - mBaseDiamond)/70 > mAddSpeed? (playerData.WealthData.GoldIngot - mBaseDiamond)/70:mAddSpeed;
						}
						if(mBaseFans == playerData.RankData.CurrentRankFeat&&mBaseMoney >= playerData.WealthData.Money&&mBaseDiamond >= playerData.WealthData.GoldIngot)
						{
							UpdateEnd = true;
						}
					},true);
				}else
				{
					EventDelegate.Add(tween.onFinished,delegate()
					{
						switch(item.name)
						{
							case "0":
								iTween.ShakePosition(Fans.gameObject,new Vector3(5,5,0),0.4f);
								break;
							case "1":
								iTween.ShakePosition(Money.gameObject,new Vector3(5,5,0),0.4f);
								break;
							case "2":
								iTween.ShakePosition(Diamond.gameObject,new Vector3(5,5,0),0.4f);
								break;
						}
						Destroy(item);
						
					},true);
				}
			}
		}
	}
	
	public void SetFansProgress(bool whether)
	{
		if(whether)
		{
			GradeInformation.transform.localScale = Vector3.one;
			NGUITools.SetActive(GradeInformation,true);
		}else{
			GradeInformation.transform.localScale = Vector3.zero;
			NGUITools.SetActive(GradeInformation,false);
		}
	}
	
	public void showFansInfo()
	{
		EnergySprite.transform.localScale = Vector3.zero;
		Fans.transform.localScale = Vector3.one;
	}
	
	public void showJingLi()
	{
		EnergySprite.transform.localScale = Vector3.one;
		Fans.transform.localScale = Vector3.zero;
	}
	
	
	public void AddDiamond(GameObject obj)
	{
		
	}
	public void AddJingLi(GameObject obj)
	{
		if (Globals.Instance.MNpcManager.getSuoPingState())
		return;
			
		NetSender.Instance.RequestBuyOilPrice();
	}
	public void AddMoney(GameObject obj)
	{
		
	}
	
	void AddFans(GameObject obj)
	{
		
	}
	
	public void SetUpdateNow(bool IsHas)
	{
		UpdateNow = IsHas;
	}
	
	public int GetBaseMoney
	{
		get
		{
			return mBaseMoney;
		}
	}
	public int GetBaseDiamond
	{
		get
		{
			return mBaseDiamond;
		}
	}
	public int CurrentBaseMoney
	{
		set
		{
			mBaseMoney -= value;
			moneyText.text = mBaseMoney.ToString("N0");
		}
	}
	public int CurrentBaseDiamond
	{
		set
		{
			mBaseDiamond -= value;
			diamondText.text = mBaseDiamond.ToString("N0");
		}
	}
	
		
	ISubscriber actorWealthUpdate = null;
}
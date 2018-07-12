using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class NewWealthGroup : MonoBehaviour 
{

	private PlayerData playerData;
	public UIButton DiamondBtn;
	public UIButton MoneyBtn;

	public UILabel DiamondLabel;
	public UILabel MoneyLabel;

	public bool UpdateNow = true;

	void Awake()
	{
		playerData = Globals.Instance.MGameDataManager.MActorData;

		UIEventListener.Get(DiamondBtn.gameObject).onClick += AddDiamond;
		UIEventListener.Get(MoneyBtn.gameObject).onClick += AddMoney;

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

	
	public void UpdateWealth()
	{
		DiamondLabel.text = playerData.WealthData.GoldIngot.ToString();

		MoneyLabel.text = playerData.WealthData.Money.ToString();
	}

	public void SetUpdateNow(bool IsHas)
	{
		UpdateNow = IsHas;
	}

	private void AddDiamond(GameObject obj)
	{
		Globals.Instance.MGUIManager.CreateWindow<GUIPurchase>(delegate(GUIPurchase guiPage) {
			guiPage.ShowPurshaseDiamondInfor();
		});
	}

	private void AddMoney(GameObject obj)
	{
		Globals.Instance.MGUIManager.CreateWindow<GUIPurchase>(delegate(GUIPurchase guiPage) {
			guiPage.ShowPurshaseMoneyInfor();
		});
	}

	void OnDestroy()
	{
		if (null != actorWealthUpdate)
			actorWealthUpdate.Unsubscribe();
		actorWealthUpdate = null;
	}

	ISubscriber actorWealthUpdate = null;
}
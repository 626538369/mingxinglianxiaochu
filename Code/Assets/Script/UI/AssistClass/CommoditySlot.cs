using UnityEngine;
using System.Collections;

public class CommoditySlot : MonoBehaviour 
{
	public UIToggle itemCheckedBtn;
	
	// public UIButton itemIconBtn;
	// public PackedSprite itemIcon;
	// public PackedSprite itemCoverIcon;
	public PackageItemIconSlot pckageItemIcon;
	
	public SpriteText nameText;
	public SpriteText descText;
	
	public SpriteText pinzhiText;
	public SpriteText pingfenText;
	
	public SpriteText origPriceText;
	public SpriteText currPriceText;
	
	public PackedSprite currencyIcon;
	public SpriteText currencyText;
	
	// public SpriteText bargainEventText;
	public PackedSprite bargainPriceLine;
	// public PackedSprite freeGoodsIcon;
	
	[HideInInspector] public CommodityData ItemData = null;
	
	void Awake()
	{
		pckageItemIcon.IgnoreCollider(true);
	}
	
	public void SetChecked(bool check)
	{
		itemCheckedBtn.isChecked = check;
	}
	
	public void UpdateGUI(CommodityData data)
	{
		ItemData = data;
		itemCheckedBtn.Data = data;
		
		pckageItemIcon.SetItemData(data);
		
		nameText.Text = data.BasicData.Name;
		descText.Text = data.BasicData.Description;
		
		float showOrigPrice = (float)data.originalPrice;
		float showCurrPrice = (float)data.currPrice;
		if (data.currency == CurrencyType.RmbYuan)
		{
			showOrigPrice = (float)data.originalPrice;
			showCurrPrice = (float)data.currPrice;
		}
		else if (data.currency == CurrencyType.RmbJiao)
		{
			showOrigPrice = (float)data.originalPrice * 0.1f;
			showCurrPrice = (float)data.currPrice * 0.1f;
		}
		else if (data.currency == CurrencyType.RmbFen)
		{
			showOrigPrice = (float)data.originalPrice * 0.01f;
			showCurrPrice = (float)data.currPrice * 0.01f;
		}
		
		// Show the RMB-Yuan
		origPriceText.Text = showOrigPrice.ToString("F0");
		
		if (data.comType == CommodityType.GameInner)
		{
			currPriceText.Text = showCurrPrice.ToString("F0");
		}
		else
		{
			currPriceText.Text = showCurrPrice.ToString("F2");
		}
		
		currencyText.Text = "￥";
		
		if (data.comType == CommodityType.GameInner)
		{
			currencyText.transform.localScale = Vector3.zero;
			//currencyIcon.transform.localScale = Vector3.one;
			
			pinzhiText.transform.localScale = Vector3.one;
			pingfenText.transform.localScale = Vector3.one;
			
			pinzhiText.Text = data.GetDisplayQualityName();
			pingfenText.Text = "--";
		}
		else
		{
			currencyText.transform.localScale = Vector3.zero;
			//currencyIcon.transform.localScale = Vector3.zero;
			
			pinzhiText.transform.localScale = Vector3.zero;
			pingfenText.transform.localScale = Vector3.zero;
		}
		
		// if (null == data.bargainEvent)
		// {
		// 	bargainEventText.transform.localScale = Vector3.zero;
		// 	
		// 	// Give as freeGoods, so show the text
		// 	if (null != data.handselEvent)
		// 	{
		// 		bargainEventText.transform.localScale = Vector3.one;
		// 		bargainEventText.Text = "赠送截止于:" + data.handselEvent.endTime;
		// 	}
		// }
		// else
		// {
		// 	bargainEventText.transform.localScale = Vector3.one;
		// 	bargainEventText.Text = "特价截止于:" + data.bargainEvent.endTime;
		// }
		
		if (data.IsDiscounted)
		{
			bargainPriceLine.transform.localScale = Vector3.one;
			
			int defTextNum = 8;
			float lineScale = (float)(origPriceText.Text.Length + 2) / defTextNum;
			bargainPriceLine.transform.localScale = new Vector3(lineScale, 1, 1);
			
			currPriceText.transform.localScale = Vector3.one;
		}
		else
		{
			bargainPriceLine.transform.localScale = Vector3.zero;
			currPriceText.transform.localScale = Vector3.zero;
		}
		
		// if (null == data.freeGoodsList || 0 == data.freeGoodsList.Count)
		// {
		// 	freeGoodsIcon.transform.localScale = Vector3.zero;
		// }
		// else
		// {
		// 	freeGoodsIcon.transform.localScale = Vector3.one;
		// }
		
		origPriceText.transform.localScale = Vector3.zero;
		currencyIcon.transform.localScale = Vector3.zero;
	}
}

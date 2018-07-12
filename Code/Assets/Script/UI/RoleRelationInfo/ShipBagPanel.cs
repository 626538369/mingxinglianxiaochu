using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBagPanel : RoleBasePanel 
{
	public PackageUnit OwnPackageUnit;
	
	protected override void Awake()
	{
		base.Awake();
		
		// Common PackageUnit Component
		OwnPackageUnit.SetItemIconTapDelegate(ItemIconTapDelegate);
		OwnPackageUnit.SetOpBtnDelegate(OpBtnDelegate);
	}
	
	public override void UpdateGUI()
	{
		if (IsNeedCreate)
		{
			IsNeedCreate = false;
			OwnPackageUnit.UpdatePackageItem(ItemSlotType.CLOTH_BAG);
		}
		else
		{
			OwnPackageUnit.UpdatePackageItem(ItemSlotType.CLOTH_BAG);
		}
	}
	
	public override void HideGUI()
	{
		// OtherBtn.Value = false;
	}
	
	public override void OnClickNonFunctionalArea(GameObject obj) 
	{
		OwnPackageUnit.ClearSelectedIcon();
	}
	
	public void HidePackageUnitOpBtn(bool hide)
	{
		OwnPackageUnit.HideOpBtn(hide);
	}
	
	public void SetItemDescText(ItemSlotData data)
	{
		OwnPackageUnit.SetItemDescTips(data, true);
	}
	
	void ItemIconTapDelegate(PackageUnit.IconData iData)
	{
		HidePackageUnitOpBtn(false);
		
		if (null != iData.itemData.MItemData)
		{
		}
		else
		{
		}
	}
	
	void OpBtnDelegate(PackageUnit.IconData iData)
	{
		if (null == iData)
			return;
		
		NetSender.Instance.RequestUseItem(-1, iData.itemData);
	}
}

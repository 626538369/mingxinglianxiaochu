using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackageItemIconSlot : MonoBehaviour 
{
	public UIToggle ItemBGBtn;
	
	/// <summary>
	/// The icon.
	/// </summary>
	public PackedSprite Icon;
	
	/// <summary>
	/// The count sprite text
	/// </summary>
	public SpriteText Count;
	
	public PackedSprite Plus;
	
	public PackedSprite[]		QianghuaIcons;
	
	[HideInInspector] public static readonly string IconLock = "ItemLock";
	[HideInInspector] public static readonly string IconNullNormal = "ItemNull";
	
	void Awake()
	{
		IconShipEquipNulls[(int)ShipEquipmentLocation.GUN_PRIMARY] = "WeaponNull";
		IconShipEquipNulls[(int)ShipEquipmentLocation.GUN_1] = "WeaponNull";
		IconShipEquipNulls[(int)ShipEquipmentLocation.HULL_ARMOR_1] = "AdornNull";
		IconShipEquipNulls[(int)ShipEquipmentLocation.SHIP_SIDE_ARMOR_1] = "ArmorNull";
		IconShipEquipNulls[(int)ShipEquipmentLocation.SHIP_SIDE_ARMOR_2] = "ArmorNull";
		IconShipEquipNulls[(int)ShipEquipmentLocation.ASSISTANT_EQUIP_1] = "AnqiNull";
	}
	
	public void IgnoreCollider(bool ignore)
	{
		if (null != ItemBGBtn && null != ItemBGBtn.transform.GetComponent<Collider>())
		{
			ItemBGBtn.transform.GetComponent<Collider>().enabled = !ignore;
		}
	}
	
	public void SetItemData(ItemSlotData data)
	{
		ItemBGBtn.Data = data;
		
		setIconQiangHuaVisible(false);
		
		if (data.IsUnLock())
		{
			if (null == data.MItemData)
			{
				if (data.SlotType == ItemSlotType.SHIP_EQUIPMENT)
				{
					Icon.PlayAnim(IconShipEquipNulls[data.LocationID]);
					Plus.transform.localScale = Vector3.one;
				}
				else if (data.SlotType == ItemSlotType.CLOTH_BAG)
				{
					Icon.PlayAnim(IconNullNormal);
					Plus.transform.localScale = Vector3.zero;
				}
				else
				{
					Plus.transform.localScale = Vector3.zero;
				}
				
				Count.transform.localScale = Vector3.zero;
			}
			else
			{
				Plus.transform.localScale = Vector3.zero;
				
				Icon.PlayAnim(data.MItemData.BasicData.Icon);
				SetItemCount(data);
				
		
			}
		}
		else
		{
			Icon.PlayAnim(IconLock);
		}
	}
	
	void SetItemCount(ItemSlotData data)
	{
		if (data.MItemData.BasicData.Count > 1)
		{
			Count.transform.localScale = Vector3.one;
			Count.Text = data.MItemData.BasicData.Count.ToString();
		}
		else
		{
			Count.transform.localScale = Vector3.zero;
		}
	}
	
	public void SetItemData(CommodityData data)
	{
		ItemBGBtn.Data = data;
		
		if (null == data)
		{
			Plus.transform.localScale = Vector3.zero;
			Count.transform.localScale = Vector3.zero;
		}
		else
		{
			Plus.transform.localScale = Vector3.zero;
			
			Icon.PlayAnim(data.BasicData.Icon);
			SetItemCount(data);
		}
	}
	
	void SetItemCount(CommodityData data)
	{
		if (data.BasicData.Count > 1)
		{
			Count.transform.localScale = Vector3.one;
			Count.Text = data.BasicData.Count.ToString();
		}
		else
		{
			Count.transform.localScale = Vector3.zero;
		}
	}
	
	public void Hide(bool hide){
		Plus.Hide(hide);
		Count.Hide(hide);
		Icon.Hide(hide);
	}
	
	public void setIconQiangHuaVisible(bool visible)
	{
		int testLeng = QianghuaIcons.GetLength(0);
		
		for (int i=0; i<QianghuaIcons.GetLength(0);i++)
		{
			PackedSprite packeQianghua = QianghuaIcons[i];
			packeQianghua.transform.localScale = Vector3.zero;
		}
	}
	
	Dictionary<int, string> IconShipEquipNulls = new Dictionary<int, string>();
}

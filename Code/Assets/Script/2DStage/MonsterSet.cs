using UnityEngine;
using System.Collections;

public class MonsterSet : MonoBehaviour 
{
	SpriteText nameText = null;
	PackedSprite avatar = null;
	
	MonsterProperty property = null;
	
	void Awake()
	{
		transform.localScale = Vector3.one;
		
		if (null == nameText)
		{
			nameText = transform.Find("NPCName").GetComponent<SpriteText>() as SpriteText;
			avatar = transform.Find("NPCIcon").GetComponent<PackedSprite>() as PackedSprite;
		}
	}
	
	public void UpdateData(CopyMonsterData.MonsterData data)
	{
		// nameText.Text = "";
		// avatar.PlayAnim("");
		
		property = gameObject.GetComponent<MonsterProperty>() as MonsterProperty;
		if (null == property) property = gameObject.AddComponent<MonsterProperty>() as MonsterProperty;
		
		property.FleetID = data.FleetID;
		property.Name = data.MonsterName;
		property.DialogText = data.MonsterDialog;
		property.ModelName = data.ModelName;
		
		nameText.Text = property.Name;
		avatar.PlayAnim(property.ModelName);
		
		BattleTrigger btTrigger = gameObject.GetComponent<BattleTrigger>() as BattleTrigger;
		if (null == btTrigger) btTrigger = gameObject.AddComponent<BattleTrigger>() as BattleTrigger;
		
		btTrigger.BoxSize = new Vector3(avatar.width, avatar.height, 1.0f);
		
		btTrigger.TriggerEnterEvents += GameStatusManager.Instance.MCopyStatus.OnRequestBattle;
	}
}

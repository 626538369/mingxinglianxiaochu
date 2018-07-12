using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PackageItemIcon : MonoBehaviour {
	
	/// <summary>
	/// The icon.
	/// </summary>
	public PackedSprite Icon;
	
	/// <summary>
	/// The name sprite text
	/// </summary>
	//public SpriteText Name;
	
	/// <summary>
	/// The count sprite text
	/// </summary>
	//public SpriteText Count;
	
	public PackedSprite	Cover;
		
	/// <summary>
	/// The own button.
	/// </summary>
	//public UIToggle OwnBtn;
	
	private ItemSlotData mItemData;
	
	public ItemSlotData itemData
	{
		get{
			return mItemData;
		}
	}
	
	/// <summary>
	/// The cover invisible z.
	/// </summary>
	private static readonly float		CoverVisibleZ = -2;
	
	/// <summary>
	/// Gets the item high light. by hxl 20121214
	/// </summary>
	/// <returns>
	/// The item high light.
	/// </returns>
	public bool HighLightState
	{
		get{
			return HelpUtil.FloatEquals(Cover.transform.localPosition.z,CoverVisibleZ);
		}
	}
	
	protected void Awake(){
		//OwnBtn = GetComponent<UIToggle>();
		
		// hide the cover
		Cover.transform.localPosition = new Vector3(0,0,0);		
	}
		
	/// <summary>
	/// Gets or sets the text of name
	/// </summary>
	/// <value>
	/// The text.
	/// </value>
	public string Text{
		//get{return OwnBtn.Text;}
		//set{OwnBtn.Text = value;}
		get{return "";}
	}
	
	/// <summary>
	/// Hide this instance.
	/// </summary>
	public void Hide(bool hide){
		//OwnBtn.Hide (hide);
		//Name.Hide (hide);
		//Count.Hide(hide);
		Icon.Hide(hide);
		Cover.Hide(hide);
	}
	
	// Update is called once per frame
	public void SetItemIcon (ItemSlotData data,bool lockedOrEmpty) {
		mItemData = data;
		HelpUtil.SetItemIcon(transform,data,lockedOrEmpty);
	}
	
	/// <summary>
	/// Set Item HighLight. by hxl 20121214
	/// </summary>
	/// <value>
	/// state = true is HighLight; state = false is Cancel HighLight;
	/// </value>
	public void SetItemHighLight(bool state){
		if(state){
			Cover.transform.localPosition = new Vector3(0,0,CoverVisibleZ);
		}else{
			Cover.transform.localPosition = new Vector3(0,0,0);
		}
		
	}
}

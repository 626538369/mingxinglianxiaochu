using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropInfoDec : MonoBehaviour {

	public UIButton CloseBtn;
	public UITexture PropIcon;
	public UILabel PropName;
	public UILabel UseMagicNum;
	public UILabel PropDec;
	public UILabel UseStatus;

	GPropInfo gPropInfo;

	void Awake(){
		UIEventListener.Get (CloseBtn.gameObject).onClick += delegate(GameObject go) {
			this.gameObject.SetActive(false);
		};
	}

	public void ShowPropInfo(ItemSlotData slotData , int isUse){
		if(gPropInfo == null){
			gPropInfo = Globals.Instance.MDataTableManager.GetConfig<GPropInfo> ();
		}
		if (gPropInfo.GetPropInfoElementList ().ContainsKey (slotData.MItemData.BasicData.ItemConfigElement.Item_Theme)) {
			GPropInfo.PropInfoElement info = gPropInfo.GetPropInfoElementList () [slotData.MItemData.BasicData.ItemConfigElement.Item_Theme];
			PropIcon.mainTexture = Resources.Load ("Icon/ItemIcon/" + slotData.MItemData.BasicData.Icon, typeof(Texture2D)) as Texture2D;
			PropName.text = slotData.MItemData.BasicData.Name;
			UseMagicNum.text = info.MagicPoint.ToString ();
			PropDec.text = Globals.Instance.MDataTableManager.GetWordText(info.PropDesc);
			if(isUse == 0){
				UseStatus.text = Globals.Instance.MDataTableManager.GetWordText("6022");
			}else if(isUse == 1){
				UseStatus.text = Globals.Instance.MDataTableManager.GetWordText("6023");
			}else if(isUse == 2){
				UseStatus.text = Globals.Instance.MDataTableManager.GetWordText("6011");
			}else if(isUse == 3){
				UseStatus.text = Globals.Instance.MDataTableManager.GetWordText("6024");
			}
			this.gameObject.SetActive (true);
		} else {
			this.gameObject.SetActive (false);
		}
	}
}

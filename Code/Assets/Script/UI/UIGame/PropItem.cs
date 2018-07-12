using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropItem : MonoBehaviour {

	public UIButton UsePropBtn;

	public GameObject NoPropBg;
	public GameObject NoPropLock;
	public GameObject PropBg;
	public UITexture PropIcon;
	public GameObject PropLock;
	public GameObject UseObj;

	public GameObject MagicObj;
	public UILabel MagicNum;

	public PropInfoDec MPropInfoDec;
	public UsePropDialog UsePropTipsObj;

	ItemSlotData MSlotData = null;
	GPropInfo.PropInfoElement MPropInfo;
	GPropInfo gPropInfo;

	private int UseMaxNum = 1;
	private int PropStatus = 0;

	void Awake(){

		gPropInfo = Globals.Instance.MDataTableManager.GetConfig<GPropInfo> ();


		UIEventListener.Get (UsePropBtn.gameObject).onClick += delegate(GameObject go) {

			if(EliminationMgr.Instance.GameStatus != GameStatusEnum.Playing){

				if(MSlotData != null){
					MPropInfoDec.ShowPropInfo(MSlotData,PropStatus);
					if(this.transform.localPosition.x > 0){
						MPropInfoDec.transform.localPosition = new Vector3(this.transform.localPosition.x - 30f, MPropInfoDec.transform.localPosition.y,MPropInfoDec.transform.localPosition.z);
					}else{
						MPropInfoDec.transform.localPosition = new Vector3(this.transform.localPosition.x + 30f, MPropInfoDec.transform.localPosition.y,MPropInfoDec.transform.localPosition.z);
					}
				}
				return;
			}

			if(PropStatus == 1){
				Globals.Instance.MGUIManager.ShowSimpleCenterTips(6009);
				return;
			}else if(PropStatus == 2){
				Globals.Instance.MGUIManager.ShowSimpleCenterTips(6011);
				return;
			}else if(PropStatus == 3){
				Globals.Instance.MGUIManager.ShowSimpleCenterTips(6024);
				return;
			}
			if(MSlotData != null){
				if (gPropInfo.GetPropInfoElementList ().ContainsKey (MSlotData.MItemData.BasicData.ItemConfigElement.Item_Theme)) {
					GPropInfo.PropInfoElement info = gPropInfo.GetPropInfoElementList()[MSlotData.MItemData.BasicData.ItemConfigElement.Item_Theme];
					if(UseMaxNum <= 0){
						Globals.Instance.MGUIManager.ShowSimpleCenterTips(6008);	
						return;
					}
					if(EliminationMgr.Instance.MagicPower >= info.MagicPoint){
//					info = gPropInfo.GetPropInfoElementList()[12300501];
						UsePropTipsObj.ShowPropInfo(MSlotData.MItemData.BasicData.Name,info.PropDesc,delegate() {
							EliminationMgr.Instance.UsePropMethod (info, PropIcon);
							UseMaxNum-=1;
							if(UseMaxNum <= 0){
								UseObj.SetActive(true);
							}					
						});
					}else{
						Globals.Instance.MGUIManager.ShowSimpleCenterTips(6010);	
					}
				}
			}
		};
	}

	// 未开放
	public void NoOpenProp(){
		if(MSlotData == null){
			PropStatus = 3; //未开放
			NoPropBg.SetActive (true);
			NoPropLock.SetActive (true);
			PropBg.SetActive (false);
			MagicObj.SetActive (false);
		}

		UseMaxNum = 1;
	}

	public void InitUse(ItemSlotData slotData){
		PropStatus = 0;


		NoPropBg.SetActive (false);
		PropBg.SetActive (true);
		MagicObj.SetActive (false);

		PropLock.SetActive (false);
		UseObj.SetActive (false);

		MSlotData = slotData;

		PropIcon.mainTexture = Resources.Load ("Icon/ItemIcon/" + slotData.MItemData.BasicData.Icon,typeof(Texture2D)) as Texture2D;

		if (gPropInfo.GetPropInfoElementList ().ContainsKey (MSlotData.MItemData.BasicData.ItemConfigElement.Item_Theme)) {
			MPropInfo = gPropInfo.GetPropInfoElementList()[MSlotData.MItemData.BasicData.ItemConfigElement.Item_Theme];
		}

		UseMaxNum = 1;
	}
	//这个部位的衣服 相性不匹配
	public void InitNoLock(ItemSlotData slotData){
		NoPropBg.SetActive (false);
		PropBg.SetActive (true);
		PropLock.SetActive (true);
		MagicObj.SetActive (false);
		UseObj.SetActive (false);

		PropStatus = 1;

		MSlotData = slotData;

		PropIcon.mainTexture = Resources.Load ("Icon/ItemIcon/" + slotData.MItemData.BasicData.Icon,typeof(Texture2D)) as Texture2D;

		if (gPropInfo.GetPropInfoElementList ().ContainsKey (MSlotData.MItemData.BasicData.ItemConfigElement.Item_Theme)) {
			MPropInfo = gPropInfo.GetPropInfoElementList()[MSlotData.MItemData.BasicData.ItemConfigElement.Item_Theme];
		}

		UseMaxNum = 1;
	}

	//没有这个部位的衣服
	public void InitNoUse(){
		if(MSlotData == null){
			NoPropBg.SetActive (true);
			NoPropLock.SetActive (false);
			PropBg.SetActive (false);
			MagicObj.SetActive (false);
			PropStatus = 2;
		}


		UseMaxNum = 1;
	}

	public void UpdateInfo(){
		if (MSlotData != null && MPropInfo != null) {
			MagicObj.SetActive (true);
			UpdateMagicNum ();
		} else {
			MagicObj.SetActive (false);
		}

		MPropInfoDec.gameObject.SetActive (false);
	}

	public void UpdateMagicNum(){
		if(MSlotData != null && MPropInfo != null){
			if (EliminationMgr.Instance.MagicPower >= MPropInfo.MagicPoint) {
				MagicNum.text = "[FA840D]" + MPropInfo.MagicPoint.ToString ();
			} else {
				MagicNum.text = "[FF0000]"+MPropInfo.MagicPoint.ToString ();	
			}
		}
	}
}

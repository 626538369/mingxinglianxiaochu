using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePropDialog : MonoBehaviour {

	public UILabel PropName;
	public UILabel PropDesc;

	public UIButton UseBtn;
	public UIButton CancelBtn;

	private System.Action UseAction = null;
	void Awake(){
		UIEventListener.Get (UseBtn.gameObject).onClick += OnClickOkBtn;
		UIEventListener.Get (CancelBtn.gameObject).onClick += OnClickCancelBtn;
	}

	public void ShowPropInfo(string name ,string dec ,System.Action action){

		if(!EliminationMgr.Instance.IsDragBlock()){
			return;
		}
		EliminationMgr.Instance.MDragLock = true;

		this.gameObject.SetActive (true);
		PropName.text = name;
		PropDesc.text = Globals.Instance.MDataTableManager.GetWordText(dec);
		UseAction = action;
	}

	public void OnClickOkBtn(GameObject obj){
//		if(!EliminationMgr.Instance.IsDragBlock()){
//			return;
//		}
		EliminationMgr.Instance.MDragLock = false;
		this.gameObject.SetActive (false);
		if(UseAction != null){
			UseAction ();
		}
	}

	public void OnClickCancelBtn(GameObject obj){
		this.gameObject.SetActive (false);
		EliminationMgr.Instance.MDragLock = false;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// tzz
/// Battle general cmd (将领战斗喊话).
/// </summary>
public class BattleGeneralCmd : MonoBehaviour {
	
	public static readonly float	GeneralCmdPosY		= 118;
	
	private static readonly float	GeneralCmdMoveTime	= 1;
	
	private static readonly float	GeneralCmdWaitTime	= 1.88f;
	
	/// <summary>
	/// The sm current cmd list.
	/// </summary>
	private static List<BattleGeneralCmd>		smCurrCmdList = new List<BattleGeneralCmd>();
	
	/// <summary>
	/// The avatar prefab.
	/// </summary>
	public PackedSprite		AvatarPrefab;
	private PackedSprite	mAvatar;
	
	/// <summary>
	/// The avatar parent.
	/// </summary>
	public Transform		AvatarParent;
	
	/// <summary>
	/// The general command text.
	/// </summary>
	public SpriteText		GeneralCommandText;
	
	/// <summary>
	/// The self packed sprite.
	/// </summary>
	private PackedSprite	mSelfPackedSprite;
	
	private float			mStartPosX;
	private float			mEndPosX;
	
	private float			mTimer	= 0;
	
	private bool			mMoveOrWait = true;
	
	// Use this for initialization
	void Awake () {
		mAvatar = (PackedSprite)Instantiate(AvatarPrefab);
		
		mAvatar.transform.parent		= AvatarParent;
		mAvatar.transform.localPosition = Vector3.zero;
		mAvatar.transform.localScale	= Vector3.one;
		
		mSelfPackedSprite				= GetComponent<PackedSprite>();
		
		transform.parent 		= Globals.Instance.MGUIManager.MGUICamera.transform;		
		transform.localPosition = new Vector3(-1000,GeneralCmdPosY,GUIManager.GUI_NEAREST_Z + 10);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(mMoveOrWait){
			
			mTimer += Time.deltaTime;
			if(mTimer >= GeneralCmdMoveTime){
				mMoveOrWait = false;
				mTimer = GeneralCmdMoveTime;
			}
			
			float tPosX = iTween.easeInOutSine(mStartPosX,mEndPosX,mTimer / GeneralCmdMoveTime);
			transform.localPosition = new Vector3(tPosX,transform.localPosition.y,transform.localPosition.z);
			
			if(mMoveOrWait == false){
				mTimer = 0;
			}		
			
		}else{
			
			mTimer += Time.deltaTime;
			if(mTimer >= GeneralCmdWaitTime){
				DestroyCmd();
				
				smCurrCmdList.Remove(this);
			}
		}
	}
	
	/// <summary>
	/// Destroies the cmd.
	/// </summary>
	private void DestroyCmd(){
		GameObject.Destroy(mAvatar);
		GameObject.Destroy(gameObject);
	}
		
	/// <summary>
	/// Popup the specified avatar and cmd.
	/// </summary>
	/// <param name='avatar'>
	/// Avatar.
	/// </param>
	/// <param name='cmd'>
	/// Cmd.
	/// </param>
	private void Popup(string avatar,string cmd){
		
		mAvatar.PlayAnim(avatar);
		GeneralCommandText.Text = cmd;
		
		mStartPosX	= -(Screen.width + mSelfPackedSprite.width) / 2;
		mEndPosX	= -(Screen.width - mSelfPackedSprite.width) / 2;
		
		// process former BattleGeneralCmd
		//
		for(int i = 0;i < smCurrCmdList.Count;i++){
			BattleGeneralCmd c = smCurrCmdList[i];
			
			c.transform.localPosition = new Vector3(c.transform.localPosition.x,
													((i + 1) * (mSelfPackedSprite.height + 20)) + GeneralCmdPosY,
													c.transform.localPosition.z);
		}
		
		smCurrCmdList.Add(this);
	}
	
	/// <summary>
	/// Shows the general cmd.
	/// </summary>
	/// <param name='avatar'>
	/// Avatar.
	/// </param>
	/// <param name='cmd'>
	/// Cmd.
	/// </param>
	public static void ShowGeneralCmd(string avatar,string cmd){
		if(Application.isPlaying && Globals.Instance.M3DItemManager != null){
			BattleGeneralCmd tGeneralCmd = (BattleGeneralCmd)GameObject.Instantiate(Globals.Instance.M3DItemManager.BattleGeneralCmdPrefab);
			tGeneralCmd.Popup(avatar,cmd);	
		}
	}
	
	/// <summary>
	/// Destroies all battle general cmd.
	/// </summary>
	public static void DestroyAllBattleGeneralCmd(){
		foreach(BattleGeneralCmd c in smCurrCmdList){
			c.DestroyCmd();
		}
		
		smCurrCmdList.Clear();
	}
}

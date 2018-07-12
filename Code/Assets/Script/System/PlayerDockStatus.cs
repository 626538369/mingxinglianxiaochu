using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDockStatus : GameStatus, IFingerEventListener {
	
	/// <summary>
	/// The finger event active.
	/// </summary>
	private bool _mIsFingerEventActive = false;
	
	/// <summary>
	/// The former camera position in port
	/// </summary>
	private Vector3	mFormerCameraPos;
	
	/// <summary>
	/// The former camera eular in port
	/// </summary>
	private Vector3 mFormerCameraEular;
	
	/// <summary>
	/// The player warship dock position.
	/// </summary>
	private List<Vector3> mPlayerDockPos = new List<Vector3>();
	
	/// <summary>
	/// The m page total number.
	/// </summary>
	private int mPageTotalNum	= 0;
	
	/// <summary>
	/// The index of the m curr page.
	/// </summary>
	private int mCurrPageIndex	= 0;
	
	/// <summary>
	/// The m player warship list.
	/// </summary>
	private List<PlayerWarship>	mPlayerWarshipList = new List<PlayerWarship>();
	
	/// <summary>
	/// The m current selected player.
	/// </summary>
	private PlayerWarship	mCurrentSelectedPlayer = null;
	
	/// <summary>
	/// The m selected effect.
	/// </summary>
	private GameObject	mSelectedEffect = null;
	
	/// <summary>
	/// Gets the curr selected player.
	/// </summary>
	/// <returns>
	/// The curr selected player.
	/// </returns>
	public PlayerWarship GetCurrSelectedPlayer(){
		return mCurrentSelectedPlayer;
	}

	public override void Initialize ()
	{
		// Get the FingerEvent Component, Register FingerEventListener
		Globals.Instance.MFingerEvent.Add3DEventListener(this);
		SetFingerEventActive(true);
		
		mFormerCameraPos	= Globals.Instance.MSceneManager.mMainCamera.transform.position;
		mFormerCameraEular	= Globals.Instance.MSceneManager.mMainCamera.transform.eulerAngles;
		
		// find the right dock position
		//
		GameObject port = GameObject.Find("Scene/m_other_Wharf");
		if(port == null){
			throw new System.Exception("GameObject.Find(\"Scene/m_other_Wharf\") == null");
		}
		
		Transform cameraPos = port.transform.Find("CameraPostion");
		Globals.Instance.MSceneManager.mMainCamera.transform.position		= cameraPos.position;
		Globals.Instance.MSceneManager.mMainCamera.transform.eulerAngles	= cameraPos.eulerAngles;
		
		// find the right dock warship position
		//
		mPlayerDockPos.Clear();
		
		int index = 1;
		while(index < 10){
			Transform pos = port.transform.Find("postion0" + index);
			
			if(pos == null){
				break;
			}
			
			mPlayerDockPos.Insert(0,pos.position);
			index++;
		}
		
		// open the GUIPlayerDock
		//
		GUIRadarScan.Show();
		
		bool playerDockState = GameStatusManager.Instance.EnterPKBattleByPlayerDock;
		//Globals.Instance.MGUIManager.CreateWindow<GUIPlayerDock>(delegate(GUIPlayerDock gui){
		//	
		//	if(playerDockState){
		//		ShowPlayerWarship(mCurrPageIndex);				
		//	}else{
		//		NetSender.Instance.RequestPlayerDock();
		//	}
		//});
		
		// create the selected player effect
		//
		if(mSelectedEffect == null){
			mSelectedEffect = GraphicsTools.ManualCreateObject("TempArtist/Prefab/Particle/S_Point", "Indicator", true);;
			mSelectedEffect.SetActiveRecursively(false);
		}
		
	}
	
	/// <summary>
	/// Shows the player warship.
	/// </summary>
	/// <param name='pageIdx'>
	/// Page index.
	/// </param>
	public void ShowPlayerWarship(int pageIdx){
		
		ClearWarshipList();
		
		sg.Player_Dock_Res tPlayerList = GameDataManager.GetServerData<sg.Player_Dock_Res>();
		
		// calculate the page number
		if(mPlayerDockPos.Count != 0){
			
			mPageTotalNum = tPlayerList.totalPlayer / mPlayerDockPos.Count + 1;
			if(tPlayerList.totalPlayer % mPlayerDockPos.Count == 0 && tPlayerList.totalPlayer != 0){
				mPageTotalNum--;
			}
		}
		
		if(pageIdx < mPageTotalNum){
			mCurrPageIndex = pageIdx;
			
			int tFormerIdx = mCurrPageIndex * mPlayerDockPos.Count;
			int posIdx = 0;
			
			for(;tFormerIdx < tPlayerList.playerDockInfo.Count && posIdx < mPlayerDockPos.Count; tFormerIdx++,posIdx++){
				sg.Player_Dock_Res.Player_Dock_Info info = tPlayerList.playerDockInfo[tFormerIdx];
				
//				PlayerWarship warship = new PlayerWarship(info,mPlayerDockPos[posIdx],posIdx);
//				mPlayerWarshipList.Add(warship);
			}
		}
		
		//GUIPlayerDock guiPlayerDock = Globals.Instance.MGUIManager.GetGUIWindow<GUIPlayerDock>();
		//if(guiPlayerDock != null){
		//	guiPlayerDock.RefreshPage(mCurrPageIndex,mPageTotalNum);
		//}
		
	}
	
	/// <summary>
	/// Flips the page (load the warship model)
	/// </summary>
	/// <param name='pageUpOrDown'>
	/// Page up or down.
	/// </param>
	public void FlipPage(bool pageUpOrDown){
		if(pageUpOrDown){
			if(mCurrPageIndex - 1 >= 0){
				mCurrPageIndex--;
				ShowPlayerWarship(mCurrPageIndex);	
			}
		}else{
			if(mCurrPageIndex + 1 < mPageTotalNum){
				mCurrPageIndex++;
				ShowPlayerWarship(mCurrPageIndex);
			}			
		}
	}
	
	/// <summary>
	/// Clears the warship list.
	/// </summary>
	private void ClearWarshipList(){
		
		mCurrentSelectedPlayer = null;
		mSelectedEffect.SetActiveRecursively(false);
		
		foreach(PlayerWarship war in mPlayerWarshipList){
			GameObject.Destroy(war.playerWarship);
		//	GameObject.Destroy(war.playerHeader.gameObject);
		}
		
		mPlayerWarshipList.Clear();
	}

	public override void Release ()
	{
		SetFingerEventActive(false);
		
		// restore the main camera position
		Globals.Instance.MSceneManager.mMainCamera.transform.position		= mFormerCameraPos;
		Globals.Instance.MSceneManager.mMainCamera.transform.eulerAngles	= mFormerCameraEular;
		
		//GUIPlayerDock gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIPlayerDock>();
		//if(gui != null){
		//	gui.Close();
		//}
		
		ClearWarshipList();
	}
	
	public override void Pause ()
	{
		
	}
	
	public override void Resume ()
	{
		
	}
	
	public override void Update ()
	{
		
	}
	
	#region Handle Finger Event
	public bool IsFingerEventActive()
	{
		return _mIsFingerEventActive;
	}
	
	public void SetFingerEventActive(bool active)
	{
		_mIsFingerEventActive = active;
	}

	public bool OnFingerDownEvent( int fingerIndex, Vector2 fingerPos )
	{
		Ray ray = Globals.Instance.MSceneManager.mMainCamera.ScreenPointToRay( fingerPos );		
        RaycastHit hit;
		
		bool t_hited = Physics.Raycast(ray, out hit, Mathf.Infinity,(1 << 14));
		//GUIPlayerDock guiDock = Globals.Instance.MGUIManager.GetGUIWindow<GUIPlayerDock>();
		
		if (t_hited){
			// search the right ship
			GameObject go = hit.collider.gameObject.transform.parent.parent.gameObject;
			foreach(PlayerWarship ship in mPlayerWarshipList){
				if(ship.playerWarship == go){
					
					mCurrentSelectedPlayer = ship;
					
					// show the operation buttons
					//guiDock.ButtonsParent.gameObject.SetActiveRecursively(true);
					
					// set the selected effect
					mSelectedEffect.transform.position = go.transform.position;
					mSelectedEffect.SetActiveRecursively(true);
						
					break;
				}
			}
		}else{
			// hide the operation buttons
			//guiDock.ButtonsParent.gameObject.SetActiveRecursively(false);
			
			// hide the selected effect
			mSelectedEffect.SetActiveRecursively(false);
			
		}
		return t_hited;
	}
	
   	public bool OnFingerUpEvent( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
	{
		return false;
	}
	
	public bool OnFingerMoveBeginEvent( int fingerIndex, Vector2 fingerPos )
	{
		
		return false;	
	}
	
	public bool OnFingerMoveEvent( int fingerIndex, Vector2 fingerPos )
	{
		return false;	
	}
	
	public bool OnFingerMoveEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		return false;	
	}
	
	public bool OnFingerPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		return false;
	}
	
	public bool OnFingerPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
	{
		PortStatus.OnFingerPinchMove_imple(delta);	
		return true;
	}
	
	public bool OnFingerPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		return false;
	}
	
	public bool OnFingerStationaryBeginEvent (int fingerIndex, Vector2 fingerPos)
	{
		return false;
	}
	
	public bool OnFingerStationaryEndEvent (int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		return false;
	}
	
	public bool OnFingerStationaryEvent (int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		return false;
	}
	
	public bool OnFingerDragBeginEvent( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
	{
		return true;
	}
	public bool OnFingerDragMoveEvent( int fingerIndex, Vector2 fingerPos, Vector2 delta )
	{
		return true;
	}
	public bool OnFingerDragEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		return true;
	}
	
	private void MoveWarShipByFingerPos(Vector2 fingerPos)
	{
		
	}
	#endregion
	
	public class PlayerWarship{
		
		/// <summary>
		/// The player info.
		/// </summary>
		public sg.Player_Dock_Res.Player_Dock_Info playerInfo;
		
		/// <summary>
		/// The player header.
		/// </summary>
		//public PlayerDockHeader	playerHeader;
		
		/// <summary>
		/// The player warship prefab gameobject
		/// </summary>
		public GameObject		playerWarship = null;
		
		private float[]			mDockHeaderOffset = 
		{
			20,
			0,
			20,
			-10,
			20,
			0,
		};
		
				
		/// <summary>
		/// Initializes a new instance of the <see cref="PlayerDockStatus.PlayerWarship"/> class.
		/// </summary>
		/// <param name='info'>
		/// Info.
		/// </param>
		/// <param name='warshipWorldPos'>
		/// Warship world position.
		/// </param>
//		public PlayerWarship(sg.Player_Dock_Res.Player_Dock_Info info,Vector3 warshipWorldPos,int index){
//			playerInfo = info;
//						
//			//playerHeader = (PlayerDockHeader)GameObject.Instantiate(Globals.Instance.M3DItemManager.PlayerDockHeaderPrefab);
//			//playerHeader.UpdatePlayerInfo(info);
//			//playerHeader.gameObject.transform.parent = Globals.Instance.MGUIManager.MGUICamera.transform;
//			//playerHeader.LayoutPosition(warshipWorldPos + new Vector3(mDockHeaderOffset[index],0,0));
//						
//			// create the player warship
//			//
//			WarshipConfig.WarshipObject config = Globals.Instance.MDataTableManager.GetConfig<WarshipConfig>().GetWarshipObjectByID(info.warshipId);
//			
//			if(config != null){
//				Globals.Instance.MResourceManager.Load(config.WarshipIntactModel, delegate(Object obj, string error) {
//					playerWarship = (GameObject)GameObject.Instantiate(obj);
//					playerWarship.SetActiveRecursively(true);
//					LayerMaskDefine.SetLayerRecursively(playerWarship, LayerMaskDefine.REFLECTION);
//					playerWarship.transform.position = warshipWorldPos;
//				});
//			}
//			
//		}
	}
	
	/// <summary>
	/// Player warship load done delegate
	/// </summary>
	private delegate void PlayerWarshipLoadDoneDele(PlayerWarship warship);
}

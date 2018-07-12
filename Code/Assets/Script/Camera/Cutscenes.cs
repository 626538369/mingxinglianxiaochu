using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cutscenes : MonoBehaviour {
	
	/// <summary>
	/// The m_replace original model
	/// </summary>
	public GameObject[]			m_replaceSrc;
	
	/// <summary>
	/// The m_replace destination model name
	/// </summary>
	public string[]				m_replaceDst;
	
	/// <summary>
	/// The initailize actor monster model first and play at same time
	/// </summary>
	public bool					m_initActorMonster = false;
		
	/// <summary>
	/// Play done delegate.
	/// </summary>
	public delegate void		PlayDoneDelegate(Cutscenes _cutscenes);
	
	/// <summary>
	/// The m_play over delegate implements
	/// </summary>
	private PlayDoneDelegate	m_playOverDelegate = null;
	
	/// <summary>
	/// The m_curr camera animation control.
	/// </summary>
	private RelAnimControl		m_currCameraAnimControl = null;
	
	/// <summary>
	/// The name of the m player.
	/// </summary>
	private EZ3DSimipleText		mPlayerName		= null;
	
	/// <summary>
	/// The m replace actor object.
	/// </summary>
	private GameObject			mReplaceActorObj = null;

	
	/// <summary>
	/// The cut scene move height
	/// </summary>
	private static readonly float CUT_SCENE_MOVE_HEIGHT = 40;
	
	/// <summary>
	/// The cutscene move time length;
	/// </summary>
	private static readonly float CUT_SCENE_MOVE_LENGTH = 2;
	
	/// <summary>
	/// The sm top cutscene position.
	/// </summary>
	private static Vector3[]		smTopCutscenePos = new Vector3[2];
	
	/// <summary>
	/// The sm top cutscene game object
	/// </summary>
	private static GameObject	smTopCutscene = null;
	
	/// <summary>
	/// The sm bottom cutscene position.
	/// </summary>
	private static Vector3[]		smBottomCutscenePos = new Vector3[2];
	
	/// <summary>
	/// The sm bottom cutscene game object
	/// </summary>
	private static GameObject	smBottomCutscene = null;
	
	/// <summary>
	/// The m store main camera.
	/// </summary>
	private GameObject			mStoreMainCamera = null;

	void Start(){
		
		float tEndMoveTime = 0;
		
		// remove the main camera of its child
		GameObject t_tmpCamera = transform.Find("Tmp Camera").gameObject;
		if(t_tmpCamera != null){
			mStoreMainCamera	= GameObject.FindWithTag("MainCamera");
			if(mStoreMainCamera == null){
				mStoreMainCamera = Globals.Instance.MSceneManager.mMainCamera.gameObject;
			}
				
			mStoreMainCamera.transform.parent			= t_tmpCamera.transform.parent;
			mStoreMainCamera.transform.localPosition	= t_tmpCamera.transform.localPosition;	
			mStoreMainCamera.transform.rotation		 	= t_tmpCamera.transform.rotation;	
						
			RelAnimControl t_src = t_tmpCamera.GetComponent<RelAnimControl>();
			RelAnimControl t_dst = mStoreMainCamera.AddComponent<RelAnimControl>();
			
			t_src.CopyTo(t_dst);
			
			DestroyObject(t_tmpCamera);
			
			m_currCameraAnimControl = t_dst;
			m_currCameraAnimControl.SetPlayAnimDoneDelegate(delegate(){
				
				if(m_playOverDelegate != null){
					try{
						m_playOverDelegate(this);
					}catch(System.Exception e){
						Debug.LogError(e.GetType().Name + " " + e.Message);
					}					
				}
				
				// destroy own
				Destroy(this);
				
				// SceneFadeInOut.Instance.FadeInScene(5);
			});
			
			tEndMoveTime = m_currCameraAnimControl.GetPlayLength();
			if(tEndMoveTime > 0){
				StartCoroutine(EndCutsceneMove(tEndMoveTime));	
			}
		}
		
		// replace the model
		int t_num = Mathf.Min(m_replaceDst.Length,m_replaceSrc.Length);
		for(int i = 0;i < t_num;i++){
			if(m_replaceDst[i].ToLower() == "actor"){
				ReplaceActor(m_replaceSrc[i]);
			}else if(m_replaceDst[i].ToLower() == "boss"){
				ReplaceBoss(m_replaceSrc[i]);
			}
		}
		
		// get the cutscene background position
		if(smTopCutscene == null && Globals.Instance.MGUIManager != null){			
			smTopCutscene		= Globals.Instance.MGUIManager.MGUICamera.transform.Find("CutsceneTop").gameObject;
			smBottomCutscene	= Globals.Instance.MGUIManager.MGUICamera.transform.Find("CutsceneBottom").gameObject;
			
			PackedSprite ps = smTopCutscene.GetComponent<PackedSprite>();
			
			smTopCutscenePos[0]		= new Vector3(0,Screen.height / 2,smTopCutscene.transform.localPosition.z);
			smBottomCutscenePos[0]	= new Vector3(0,-Screen.height / 2,smTopCutscene.transform.localPosition.z);
			
			smTopCutscenePos[1]		= smTopCutscenePos[0] + new Vector3(0,CUT_SCENE_MOVE_HEIGHT,0);
			smBottomCutscenePos[1]	= smBottomCutscenePos[0] + new Vector3(0,-CUT_SCENE_MOVE_HEIGHT,0);
		}
		
		if(tEndMoveTime > 0){
			// setup the animation of cutscene background
			smTopCutscene.transform.localPosition		= smTopCutscenePos[1];
			smBottomCutscene.transform.localPosition	= smBottomCutscenePos[1];
			
			iTween.MoveTo(smTopCutscene,iTween.Hash("position",smTopCutscenePos[0],"time",CUT_SCENE_MOVE_LENGTH,"isLocal",true));
			iTween.MoveTo(smBottomCutscene,iTween.Hash("position",smBottomCutscenePos[0],"time",CUT_SCENE_MOVE_LENGTH,"isLocal",true));	
		}
				
		// set the uiCamera cull mask to default/LargeObject in order to 
		// hide the all interface and show the cutscene background
		// defualt layer is cutscene background
		// LargeObject layer is fadeInOut backgroud
		Globals.Instance.MGUIManager.MGUICamera.cullingMask =  (1 << 0) | (1 << 11);
		
		// disable the UIManager for POINTER_PTR evente;
	}
	
	void Update(){
		
		bool tPlayDone = HelpUtil.GetButtonState(false);
		
		// some tap event to stop cutscene
		if(tPlayDone && m_currCameraAnimControl != null){
			m_currCameraAnimControl.Stop(true);
			Destroy(m_currCameraAnimControl);
			m_currCameraAnimControl = null;
		}
	}
		
	/// <summary>
	/// Ends the cutscene move.
	/// </summary>
	/// <returns>
	/// The cutscene move.
	/// </returns>
	/// <param name='waitTime'>
	/// Wait time.
	/// </param>
	private IEnumerator EndCutsceneMove(float waitTime){
		yield return new WaitForSeconds(waitTime);
		
		// SceneFadeInOut.Instance.FadeOutScene(1);
		
		iTween.MoveTo(smTopCutscene,iTween.Hash("position",smTopCutscenePos[1],"time",CUT_SCENE_MOVE_LENGTH,"isLocal",true));
		iTween.MoveTo(smBottomCutscene,iTween.Hash("position",smBottomCutscenePos[1],"time",CUT_SCENE_MOVE_LENGTH,"isLocal",true));
	}
	
	/// <summary>
	/// Replaces the actor model instead of specific model
	/// </summary>
	/// <param name='_originalModel'>
	/// _original model.
	/// </param>
	private void ReplaceActor(GameObject _originalModel){
		if(Globals.Instance.MGameDataManager == null){
			return;
		}
		
		PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;
		GirlData t_wsData = actorData.GetActorFakeWarshipData();
		if(t_wsData != null)
		{
//			ReplaceModel(t_wsData.BasicData.ModelName,_originalModel,delegate(Object obj,string error){
//				mReplaceActorObj = (GameObject)obj;
//				
//				mPlayerName = WarshipL.Create3DNameImpl(mReplaceActorObj,actorData.BasicData.FleetName);
//				
//				// change the layer to enable render in cutscene mode
//				mPlayerName.gameObject.layer = 11;
//				for(int i = 0;i < mPlayerName.transform.childCount;i++){
//					mPlayerName.transform.GetChild(i).gameObject.layer = 11;
//				}				
//			});	
		}		
	}
	
	/// <summary>
	/// Replaces the boss.
	/// </summary>
	/// <param name='_originalModel'>
	/// _original model.
	/// </param>
	private void ReplaceBoss(GameObject _originalModel){
		if(Globals.Instance.MGameDataManager == null){
			return;
		}
		
		CopyData t_cpdata = Globals.Instance.MGameDataManager.MCurrentCopyData;
		List<CopyMonsterData.MonsterData> t_monsterList = t_cpdata.MCopyMonsterData.MMonsterDataList;
		if(t_monsterList.Count > 0){
			CopyMonsterData.MonsterData t_monsterData = t_monsterList[t_monsterList.Count - 1];
			ReplaceModel(t_monsterData.ModelName,_originalModel);
		}		
	}
	
	/// <summary>
	/// Replaces the model subfunction
	/// </summary>
	/// <param name='_prefabName'>
	/// _prefab name.
	/// </param>
	/// <param name='_originalModel'>
	/// _original model.
	/// </param>
	private void ReplaceModel(string _prefabName,GameObject _originalModel,ResourceMgr.ResourceDelegate dele = null){
		if(Globals.Instance.MResourceManager == null){
			return;
		}
		
		Globals.Instance.MResourceManager.Load(_prefabName, delegate(Object obj, string error) {
			if(obj == null){
				Debug.LogError("obj == null ReplaceModel " + error);
				return;
			}
			
			GameObject t_warship = (GameObject)Instantiate(obj);
			
			try{
				RelAnimControl t_src = _originalModel.GetComponent<RelAnimControl>();
				RelAnimControl t_dst = t_warship.AddComponent<RelAnimControl>();
				
				t_src.CopyTo(t_dst);
				t_dst.transform.parent			= _originalModel.transform.parent;
				t_dst.transform.localPosition	= _originalModel.transform.localPosition;
				
				Destroy(_originalModel);
				
				if(dele != null){
					dele(t_warship,error);
				}
				
			}catch(System.Exception ex){
				Debug.LogError("ReplaceModel error: " + ex.Message);
			}	
		});
	}
	
	// calledback when destory
	protected void OnDestroy(){
		
		// restore the main Camera
		if(mStoreMainCamera != null){
			mStoreMainCamera.transform.parent = null;
		}

		// delete all cutscenes object
		int t_childCount = transform.GetChildCount();
		for(int i = 0;i < t_childCount;i++){
			GameObject t_go = transform.GetChild(i).gameObject;
			GameObject.Destroy(t_go);
		}
		
		// destroy camera animation control component
		GameObject.Destroy(m_currCameraAnimControl);
		GameObject.Destroy(gameObject);
		
		// restore to show GUI/LargeObject layer 
		Globals.Instance.MGUIManager.MGUICamera.cullingMask = (1 << 8) | (1 << 11);
		
		// enable the UIManager for POINTER_PTR
		
		// delete the player name
		if(mPlayerName != null){
			Globals.Instance.M3DItemManager.DestroyEZ3DItem(mPlayerName.gameObject);
		}
	}
	
	/// <summary>
	/// Sets the play over delegate.
	/// </summary>
	/// <param name='_dele'>
	/// _dele.
	/// </param>
	public void SetPlayOverDelegate(PlayDoneDelegate _dele){
		m_playOverDelegate		= _dele;
	}
	
}

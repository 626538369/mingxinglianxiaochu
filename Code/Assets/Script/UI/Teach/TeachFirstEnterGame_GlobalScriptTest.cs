using UnityEngine;
using System.Collections;

public class TeachFirstEnterGame_GlobalScriptTest : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		// test for single scene
		if(Globals.Instance.MDataTableManager == null){
				
			// for config reading
			Globals.Instance.MDataTableManager 	= gameObject.GetComponent(typeof(DataTableManager)) as DataTableManager;
			
			// for warship 3D title item
			Globals.Instance.M3DItemManager		= gameObject.GetComponent(typeof(EZ3DItemManager)) as EZ3DItemManager;
			Globals.Instance.MGUIManager		= gameObject.GetComponent(typeof(GUIManager)) as GUIManager;
			
			// control
			Globals.Instance.MFingerEvent 		= gameObject.GetComponent(typeof(FingerEvent)) as FingerEvent;
			
			// battle fog and other effect
			Globals.Instance.MEffectManager		= gameObject.GetComponent(typeof(EffectManager)) as EffectManager;
			
			// create fleet
			Globals.Instance.MPlayerManager		= gameObject.GetComponent(typeof(PlayerManager)) as PlayerManager;
			
			// camera track for bettle entering
			Globals.Instance.MCamTrackController	= gameObject.GetComponent(typeof(CameraTrackController)) as CameraTrackController;
			
			// battle game data
			Globals.Instance.MGameDataManager		= gameObject.GetComponent(typeof(GameDataManager)) as GameDataManager;
			Globals.Instance.MSkillManager			= gameObject.GetComponent(typeof(SkillManager)) as SkillManager;
			
			Globals.Instance.MGUILayoutManager		= gameObject.GetComponent(typeof(GUILayoutManager)) as GUILayoutManager;
			
			Globals.Instance.MAffectorManager		= gameObject.GetComponent<AffectorManager>();
			Globals.Instance.MSoundManager			= gameObject.GetComponent<SoundManager>();
			
			Globals.Instance.MResourceManager		= gameObject.GetComponent<ResourceMgr>();			
			
			// move the UICamera to the root
			transform.Find("UICamera").transform.parent = null;
			
			
		}else{
			
			Destroy(gameObject);
			
			// hide the loading Camera
			//GUILoading loading = Globals.Instance.MGUIManager.GetGUIWindow<GUILoading>();
			//if(loading !=null){
			//	loading.SetVisible(false);
			//}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

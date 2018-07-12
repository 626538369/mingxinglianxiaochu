using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EZ3DItemManager : MonoBehaviour 
{
	public GameObject SimpleTextTP;
	public GameObject MonsterArrowTP;	
	public GameObject WarshipHeaderTP;
	
	public BattleGeneralCmd		BattleGeneralCmdPrefab;
	
	public NewEffectFont		NewEffectFontPrefab;
	
	public SystemNotifyText		SystemNotifyTextPrefab;
	
	public PackedSprite			ReinforcementsLabelPrefab;
	
	public FightCutscene		BattleFightBeginPrefab;
	
	public PackedSprite		UnsealChestPrefab;
	//public PlayerDockHeader		PlayerDockHeaderPrefab;
	
	public delegate void EffectEndDelegate();
	
	// Copy from Unity homepage, Note that the parent transform's world rotation and scale are applied to the local position when calculating the world position. 
	// This means that while 1 unit in Transform.position is always 1 unit, 1 unit in Transform.localPosition will get scaled by the scale of all ancestors.
	public Transform EZ3DItemParent = null;
	[HideInInspector] public Vector3 EZ3DItemParentScale = Vector3.one;
	[HideInInspector] public Vector3 EZ3DItemParentScaleInv = Vector3.one;
	
	/**
	 * tzz added
	 * the offset position (y) for displaying 3D items
	 */
	private static readonly float	Offset = 30.0f;
	
	void Awake()
	{
		_mEZ3DObjList = new Dictionary<int, GameObject>();
		_mTmpRemoveList = new Dictionary<int, GameObject>();
	}
	
	void Start()
	{
		EZ3DItemParent = Globals.Instance.MGUIManager.MGUICamera.transform.Find("EZ3DItemParent");
		//EZ3DItemParent.localScale = new Vector3(Globals.Instance.MGUIManager.widthRatio, Globals.Instance.MGUIManager.heightRatio, 1.0f);
		EZ3DItemParent.localScale = new Vector3(1, 1, 1.0f);
		EZ3DItemParentScale = EZ3DItemParent.localScale;
		
		Transform parent = EZ3DItemParent.parent;
		while (null != parent)
		{
			EZ3DItemParentScale = new Vector3(EZ3DItemParentScale.x * parent.localScale.x, 
				EZ3DItemParentScale.y * parent.localScale.y, EZ3DItemParentScale.z * parent.localScale.z);
			parent = parent.parent;
		}
		
		EZ3DItemParentScaleInv = new Vector3(1.0f / EZ3DItemParentScale.x, 1.0f / EZ3DItemParentScale.y, 1.0f / EZ3DItemParentScale.z);
	}
	
	void OnDestroy()
	{
		_mEZ3DObjList.Clear();
		_mTmpRemoveList.Clear();
		
		EZ3DItemParent = null;
	}
	
	void OnDisable()
	{
		foreach (KeyValuePair<int, GameObject> keyVal in _mEZ3DObjList)
		{
			foreach(GameObject obj in m_systemEffectList){
				if(obj == keyVal.Value){
					m_systemEffectList.Remove(keyVal.Value);
					break;
				}
			}
			
			GameObject.DestroyObject(keyVal.Value);
		}
		
		_mEZ3DObjList.Clear();
		_mTmpRemoveList.Clear();
	}
	
	public GameObject CreateEZ3DItem(UnityEngine.Object prefab, Vector3 worldPos,int Layer = 8)
	{
		
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		go.name = "UI3DItem" + _mEZ3DObjList.Count;
		go.transform.parent = EZ3DItemParent;
		go.transform.localScale = Vector3.one;
		
		Vector3 guiPos = GUIManager.WorldToGUIPoint(worldPos);
		guiPos.x *= Globals.Instance.M3DItemManager.EZ3DItemParentScaleInv.x;
		guiPos.y *= Globals.Instance.M3DItemManager.EZ3DItemParentScaleInv.y;
		go.transform.localPosition = new Vector3(guiPos.x, guiPos.y, GUIManager.GUI_NEAREST_Z);
		
		// Set GUI layer
		LayerMaskDefine.SetLayerRecursively(go, Layer);
		
		_mEZ3DObjList.Add(go.GetInstanceID(), go);
		
		return go;
	}
	
	IEnumerator Create2DPackedEffect(PackedSprite prefab, Vector3 guiPos, float length, EffectEndDelegate endDel = null)
	{
		PackedSprite sprite = Instantiate(prefab) as PackedSprite;
		sprite.gameObject.name = "UI3DItem" + _mEZ3DObjList.Count;
		sprite.transform.parent = EZ3DItemParent;
		sprite.transform.localScale = Vector3.one;
		
		guiPos.x *= Globals.Instance.M3DItemManager.EZ3DItemParentScaleInv.x;
		guiPos.y *= Globals.Instance.M3DItemManager.EZ3DItemParentScaleInv.y;
		sprite.transform.localPosition = new Vector3(guiPos.x, guiPos.y, GUIManager.GUI_NEAREST_Z);
		sprite.PlayAnim(0);
		
		LayerMaskDefine.SetLayerRecursively(sprite.gameObject, LayerMaskDefine.GUI);
		
		yield return new WaitForSeconds(length);
		
		if (null != endDel)
			endDel();
		
		Destroy(sprite.gameObject);
	}
	
	/// <summary>
	/// tzz fucked
	/// Destroies the E z3 D item.
	/// </summary>
	/// <param name='go'>
	/// Go.
	/// </param>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	public void DestroyEZ3DItem(GameObject go, float delay = 0.0f)
	{
		_mEZ3DObjList.Remove(go.GetInstanceID());
		GameObject.DestroyObject(go, delay);
		
		//! tzz added for remove the system effect list
		foreach(GameObject obj in m_systemEffectList){
			if(obj == go){
				m_systemEffectList.Remove(go);
				break;
			}
		}
	}
	
	//-----------------------------------------------------------
	public EZ3DSimipleText Create3DSimpleText(object gameObjectOrPos,string text,float heightOffset, UIEventListener.VoidDelegate del = null)
	{
		GameObject go;
		
		if(gameObjectOrPos is GameObject){
			go = CreateEZ3DItem(SimpleTextTP, ((GameObject)gameObjectOrPos).transform.position);	
		}else{
			go = CreateEZ3DItem(SimpleTextTP, (Vector3)gameObjectOrPos);	
		}		
		
		EZ3DSimipleText item = go.GetComponent<EZ3DSimipleText>();
		item.SetValue(gameObjectOrPos, text, heightOffset);
		item.SetValueChangeDelegate(del);
		
		return item;
	}
	
	/// <summary>
	/// Creates the general avatar.
	/// </summary>
	/// <returns>
	/// The general avatar.
	/// </returns>
	/// <param name='shipData'>
	/// Ship data.
	/// </param>
	public EZWarshipHeader CreateGeneralAvatar(WarshipL shipData)
	{
		GameObject go = CreateEZ3DItem(WarshipHeaderTP, Vector3.zero);
		
		EZWarshipHeader generalAvatar = go.GetComponent<EZWarshipHeader>();
		generalAvatar.SetValue(new object[]{shipData});
		
		return generalAvatar;
	}
		
	public GameObject CreateSystemEffect(GameObject prefab, Vector3 guiPos, string text,int Layer = 8)
	{
		GameObject go = CreateEZ3DItem(prefab, Vector3.zero,Layer);
		
		// tzz modified
		// set difference y position in every 3d item objects
		go.transform.localPosition = new Vector3(guiPos.x, guiPos.y - m_systemEffectList.Count * Offset, guiPos.z);		
		m_systemEffectList.Add(go);
				
//		PackedSprite sprite = go.GetComponent<PackedSprite>() as PackedSprite;
//		sprite.PlayAnim(text);
		
		return go;
	}
	
	public void PlayUnsealChestEffect(Vector3 destGUIPos, EffectEndDelegate endDel)
	{
		StartCoroutine( Create2DPackedEffect(UnsealChestPrefab, destGUIPos, 
			UnsealChestPrefab.GetCurAnim().GetLength() + 0.2f, endDel) );
		
		Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/OpenChest");
	}
	
	public void PlayExplorePlaceEffect(EffectEndDelegate endDel)
	{
		NewEffectFont.Show(NewEffectFont.EffectType.DengJiTiSheng, endDel);
		
		Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/LeverUp");
	}
	
	public void PlayLevelUpEffect(Vector3 destGUIPos, EffectEndDelegate endDel)
	{
		NewEffectFont.Show(NewEffectFont.EffectType.DengJiTiSheng, endDel);
		
		Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/LeverUp");
	}
	
	public void PlayRankUpEffect(Vector3 destGUIPos, EffectEndDelegate endDel)
	{
		NewEffectFont.Show(NewEffectFont.EffectType.JunXianTiSheng,endDel);
		
		Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/LeverUp");
	}
	
	public void PlayTaskEffect(bool receive, EffectEndDelegate endDel)
	{
		if(receive){
			//NewEffectFont.Show(NewEffectFont.EffectType.JieShouRenWu,endDel);
			Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/GetTask");
		}else{
			NewEffectFont.Show(NewEffectFont.EffectType.WanChengRenWu,endDel);
			Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/Success");
		}		
	}
	
	public void PlayBattleEffect(GameData.BattleGameData.BattleResult battleResult, EffectEndDelegate endDel)
	{		
		NewEffectFont.EffectType type;
		if (battleResult.BattleWinResult == GameData.BattleGameData.EBattleWinResult.ACTOR_WIN){
			
			type = NewEffectFont.EffectType.ZhanDouShengLi;
			Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/BattleWin");
		}else if(battleResult.BattleWinResult == GameData.BattleGameData.EBattleWinResult.MONSTER_WIN){
			
			type = NewEffectFont.EffectType.ZhanDouShiBai;
			Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/BattleFail");
		}else{
			type = NewEffectFont.EffectType.ZhanDouPingJu;
			Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/BattleDraw");
		}
		
		NewEffectFont.Show(type,endDel);
	}
	
	public void PlaySneakEffect(GameData.BattleGameData.BattleResult battleResult, EffectEndDelegate endDel)
	{		
		NewEffectFont.EffectType type;
		if (battleResult.SneakAttackType == GameData.BattleGameData.SneakAttackType.ROLE_ATTACK){
			type = NewEffectFont.EffectType.TouXiChengGong;
			Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/BattleWin");
		}else if(battleResult.SneakAttackType == GameData.BattleGameData.SneakAttackType.NPC_ATTACK){			
			type = NewEffectFont.EffectType.BeiTouXi;
			Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/BattleFail");
		}else{
			return;
		}
		
		NewEffectFont.Show(type,endDel);
	}
	
	private Dictionary<int, GameObject> _mEZ3DObjList;
	private Dictionary<int, GameObject> _mTmpRemoveList;
	
	//! tzz added for store list of system effect
	private List<GameObject>	m_systemEffectList = new List<GameObject>();
}

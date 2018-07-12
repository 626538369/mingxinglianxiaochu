using UnityEngine;
using System.Collections;

/// <summary>
/// Attribute icon to manage the attribute icon
/// author: tzz
/// </summary>
/// 
[ExecuteInEditMode]
[System.Serializable]
public class AttributeIcon : MonoBehaviour {
	
	public enum AttributeType{
		
		// card interface
		CardSeaAttack,				// 对海攻击
		CardSubmarineAttack,		// 对潜攻击
		CardSpecialAttack,			// 绝技攻击
		CardArmor,					// 装甲
		
		CardDura,					// 耐久
		CardSunder,					// 破甲 (暴击率)
		CardHitRate,				// 命中率
		CardInterference,			// 干扰 (闪避)
		
		CardAttackRange,			// 攻击范围
		CardTonnage,				// 吨位
		
		// general icon
		CVGeneral,					// 航母将领
		SSGeneral,					// 潜艇将领
		FireGeneral,				// 火炮将领
		AllGeneral,					// 通用将领
		
		// general attribute
		ChivalrousAttr,				// 勇气
		CommandAttr,				// 毅力
		IntelligenceAttr,			// 智慧
		
		// warship type
		CVWarship,					// 航母
		BBWarship,					// 战列舰
		BCWarship,					// 战列巡洋舰
		CAWarship,					// 重巡洋舰
		
		SSWarship,					// 潜艇
		CLWarship,					// 轻巡洋舰
		DDWarship,					// 驱逐舰
		
		// warship attribute
		BigGun,						// 大口径火炮
		SmallGun,					// 小口径火炮
		HeavyHullArmor,				// 重型甲板
		LightHullArmor,				// 轻型甲板
		
		HeavyShipboardArmor,		// 重型舷侧
		LightShipboardArmor,		// 轻型舷侧
		AssitantWeapon,				// 辅助兵装				
		FillSpeed,					// 装填速度
		
		Power,						// 斗志				
		EXP,						// 经验
	};
	
	/// <summary>
	/// The name of the animation attr type.
	/// </summary>
	private static readonly string[] AnimationAttrTypeName = 
	{
		// card interface
		"attr1",
		"attr2",
		"attr3",
		"attr5",
		
		"attr4",
		"attr9",
		"attr7",
		"attr8",
		
		"attr6",
		"attr32",
		
		// general icon
		"attr10",
		"attr11",
		"attr12",
		"attr13",
		
		// general attribute
		"attr14",
		"attr15",
		"attr16",
		
		// warship type
		"attr10",
		"attr1",
		"attr1",
		"attr1",
		
		"attr1",
		"attr1",
		"attr1",
		
		// warship attribute
		"attr24",
		"attr25",
		"attr26",
		"attr27",
		
		"attr28",
		"attr29",
		"attr30",
		"attr31",
		
		// 补充斗志\经验
		"attr33",
		"attr34",
	};
	
	public static readonly int[]	AttributeNameWordConfig =
	{
		// card interface
		11030001,
		11030002,
		11030003,
		11030005,
		
		11030004,
		11030008,
		11030006,
		11030007,
		
		11030009,
		11030010,
		
		// general icon
		11030020,
		11030019,
		11030018,
		11030017,
		
		// general attribute
		11030013,
		11030014,
		11030015,
		
		// warship type
		0,
		0,
		0,
		0,
		
		0,
		0,
		0,
		
		// warship attribute
		11030046,
		11030041,
		11030047,
		11030045,
		
		11030048,
		11030045,
		0,
		11030011,
		
		// 补充斗志\经验
		11030012,
		11030016,
	};
	
	public static readonly int[]	AttributeDescWordConfig =
	{
		// card interface
		11030021,
		11030022,
		11030023,
		11030025,
		
		11030024,
		11030028,
		11030026,
		11030027,
		
		11030029,
		11030030,
		
		// general icon
		11030040,
		11030039,
		11030038,
		11030037,
		
		// general attribute
		11030033,
		11030034,
		11030035,
		
		// warship type
		0,
		0,
		0,
		0,
		
		0,
		0,
		0,
		
		// warship attribute
		11030054,
		11030049,
		11030055,
		11030053,
		
		11030056,
		11030053,
		0,
		11030031,
		
		// 补充斗志\经验
		11030032,
		11030036,
	};
	
	/// <summary>
	/// Gets the attribute name icon without ':'
	/// </summary>
	/// <returns>
	/// The attribute name icon.
	/// </returns>
	/// <param name='type'>
	/// Type.
	/// </param>
	public static string GetAttributeNameIcon(AttributeType type){
		if(AttributeNameWordConfig[(int)type] != 0){
			string name = Globals.Instance.MDataTableManager.GetWordText(AttributeNameWordConfig[(int)type]);
			
			name = name.Replace(":","");
			name = name.Replace("：","");
			
			return name;
		}
		
		return "";
	}
	
	/// <summary>
	/// Gets the attribute desc icon.
	/// </summary>
	/// <returns>
	/// The attribute desc icon.
	/// </returns>
	/// <param name='type'>
	/// Type.
	/// </param>
	public static string GetAttributeDescIcon(AttributeType type){
		if(AttributeDescWordConfig[(int)type] != 0){			
			return Globals.Instance.MDataTableManager.GetWordText(AttributeDescWordConfig[(int)type]);
		}
		
		return "";
	}
	
	/// <summary>
	/// The type of the attribute.
	/// </summary>
	public AttributeType	AttriType = AttributeType.CardSeaAttack;
	private AttributeType	mMirrorAttrType = AttributeType.CardSeaAttack;
	
	/// <summary>
	/// The name of the attribute.
	/// </summary>
	public string			AttributeName;
	
	/// <summary>
	/// The attribute name mirror to detect attribute name is changed in edit mode
	/// </summary>
	private string			AttributeNameMirror;
	
	/// <summary>
	/// The word config.
	/// </summary>
	public int				WordConfigValue = 0;
	
	/// <summary>
	/// The percent format.
	/// </summary>
	public bool				PercentFormat = false;
	
	/// <summary>
	/// The hide attribute number.
	/// </summary>
	public bool				HideAttributeNumber = false;
		
	/// <summary>
	/// The show attribute sign.
	/// </summary>
	public bool				ShowAttributeSign	= false;
	
	/// <summary>
	/// The icon sprite.
	/// </summary>
	private PackedSprite	mIconSprite;
	
	/// <summary>
	/// The attribute text.
	/// </summary>
	private SpriteText		mAttributeText;
	
	/// <summary>
	/// The m attribute value.
	/// </summary>
	private int				mAttributeValue	= 0;
	
	/// <summary>
	/// The m box collider.
	/// </summary>
	private BoxCollider		mBoxCollider;
	
	/// <summary>
	/// The change value icon. by hxl 20130105
	/// </summary>
	public SpriteText 	ChangeValueText;
	private SpriteText mChangeValueText = null;
	
	void Awake () {
		mIconSprite		= GetComponentInChildren<PackedSprite>();
		mAttributeText	= GetComponentInChildren<SpriteText>();
		
		mBoxCollider	= gameObject.GetComponent<BoxCollider>();
		if(mBoxCollider == null){
			mBoxCollider	= gameObject.AddComponent<BoxCollider>();	
		}		
		
		if(Application.isPlaying && WordConfigValue != 0 && Globals.Instance.MDataTableManager != null){
			AttributeName = Globals.Instance.MDataTableManager.GetWordText(WordConfigValue);
		}
		
		AttributeNameMirror = AttributeName;
		mMirrorAttrType		= AttriType;		
	}
	
	void Start(){
		// refresh the attribute name and type icon
		// after PackedSprite is started
		UpdateAttributeName();
		UpdateAttributeTypeIcon();
	}
	
	void Update(){
		
		if(!Application.isPlaying){
			// editor
			if(AttributeNameMirror != AttributeName){
				AttributeNameMirror = AttributeName;
				UpdateAttributeName();
			}
			
			if(mMirrorAttrType != AttriType){
				mMirrorAttrType		= AttriType;
				UpdateAttributeTypeIcon();
			}
		}
		
		if(!IsHidden() && HelpUtil.GetButtonState(false)){
			// show the attribute tip
			//
			Vector3 tTouchPos = HelpUtil.GetButtonPosition();
			
			Ray tRay = Globals.Instance.MGUIManager.MGUICamera.ScreenPointToRay(tTouchPos);
			RaycastHit tHit;
			if(mBoxCollider.Raycast(tRay,out tHit,1000)){
				Globals.Instance.MGUIManager.ShowAttributeTips(this);
			}
		}
	}
	
	/// <summary>
	/// Sets the size.
	/// </summary>
	/// <value>
	/// The size.
	/// </value>
	public Vector2 Size{
		get{return new Vector2(mIconSprite.width + mAttributeText.PixelSize.x,mIconSprite.height);}
	}
	
	/// <summary>
	/// Updates the name of the attribute when attributeName is change when edit mode
	/// </summary>
	public void UpdateAttributeName(){
		SetAttribute(mAttributeValue);
	}
	
	/// <summary>
	/// Gets the attribute icon.
	/// </summary>
	/// <value>
	/// The attribute icon.
	/// </value>
	public PackedSprite AttributeIconSprite{
		get{return mIconSprite;}
	}
	
	/// <summary>
	/// Updates the attribute type icon.
	/// </summary>
	public void UpdateAttributeTypeIcon(){
		mIconSprite.PlayAnim(AnimationAttrTypeName[(int)AttriType]);
	}
	
	/// <summary>
	/// Hide the specified iHide.
	/// </summary>
	/// <param name='iHide'>
	/// I hide.
	/// </param>
	public void Hide(bool iHide){
		mIconSprite.Hide(iHide);
		mAttributeText.Hide (iHide);
	}
	
	/// <summary>
	/// Sets the name of the attribute.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	public void SetAttributeName(string name){
		AttributeName = name;
		SetAttribute(mAttributeValue);
	}
	
	/// <summary>
	/// Sets the name of the arrtibute by wordconfig
	/// </summary>
	/// <param name='wordConfig'>
	/// Word config.
	/// </param>
	public void SetArrtibuteName(int wordConfig){
		AttributeName = Globals.Instance.MDataTableManager.GetWordText(wordConfig);
		SetAttribute(mAttributeValue);
	}
	
	public void SetAttributeName(int wordconfig){
		AttributeName = Globals.Instance.MDataTableManager.GetWordText(wordconfig);
		SetAttribute(mAttributeValue);
	}
	
	/// <summary>
	/// Determines whether this instance is hidden.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is hidden; otherwise, <c>false</c>.
	/// </returns>
	public bool IsHidden(){
		if(gameObject.active){
			return mIconSprite.IsHidden();	
		}
		
		return false;		
	}
			
	/// <summary>
	/// Sets the attribute.
	/// </summary>
	/// <param name='iType'>
	/// I type.
	/// </param>
	/// <param name='iAttrValue'>
	/// I attr value.
	/// </param>
	public void SetAttribute(int iAttrValue){
		
		mAttributeValue = iAttrValue;
		
		string tFinal = AttributeName;
		
		if(!HideAttributeNumber){
			
			if(ShowAttributeSign){
				tFinal += iAttrValue >= 0 ? "+" : "-";
			}
			
			bool percent = PercentFormat 
							|| AttriType == AttributeType.CardHitRate 
							|| AttriType == AttributeType.CardInterference
							|| AttriType == AttributeType.CardSunder;
			
			tFinal += (percent ? ((iAttrValue/100.0f).ToString("F0") + "%") : iAttrValue.ToString());
		}
		
		mAttributeText.Text = tFinal;
		
		mBoxCollider.size	= new Vector3(Size.x * 1.2f,Size.y,1);
		if(AttributeName.Length == 0){
			mBoxCollider.center = mIconSprite.transform.localPosition;
		}		
	}
	
	/// <summary>
	/// Sets the change attrbute.
	/// </summary>
	/// <param name='value'>
	/// Value. Show Value
	/// </param>
	/// <param name='pos'>
	/// Position.
	/// </param>
	/// <param name='space'>
	/// Space.
	/// </param>
	public void SetChangeAttrbute(int value = 0, float pos = 0, float space = 10)
	{
		if(value == 0)
		{
			if(mChangeValueText != null)
			{
				Destroy(mChangeValueText.gameObject);
				mChangeValueText = null;
			}
		}
		else
		{
			if(ChangeValueText != null && mChangeValueText == null)
			{
				mChangeValueText = Instantiate(ChangeValueText) as SpriteText;
				if(mChangeValueText == null) return;
				
				Transform tPackedTran = mChangeValueText.transform.Find("Icon");
				PackedSprite tPackedSprite = tPackedTran.GetComponent<PackedSprite>();
				
				string color = GUIFontColor.White255255255;
				if(value > 0)
				{
					color = GUIFontColor.LimeGreen089210000;
					tPackedSprite.PlayAnim(0);
				}
				else if(value < 0)
				{
					color = GUIFontColor.DarkRed210000005;
					tPackedSprite.PlayAnim(1);
					value = -value;
				}
				
				mChangeValueText.SetCharacterSize(mAttributeText.characterSize);
				mChangeValueText.Text = color+value;
				
				mChangeValueText.transform.parent = transform;
				
				mChangeValueText.transform.localScale = Vector3.one;
				if(pos == 0)
				{
					mChangeValueText.transform.localPosition = new Vector3(mAttributeText.transform.localPosition.x + mAttributeText.PixelSize.x + space, mAttributeText.transform.localPosition.y, mAttributeText.transform.localPosition.z);
					tPackedSprite.transform.localPosition = new Vector3(tPackedSprite.transform.localPosition.x + mChangeValueText.PixelSize.x + space, tPackedSprite.transform.localPosition.y, tPackedSprite.transform.localPosition.z);
				}
				else
				{
					mChangeValueText.transform.localPosition = new Vector3(mAttributeText.transform.localPosition.x + pos, mAttributeText.transform.localPosition.y, mAttributeText.transform.localPosition.z);
					tPackedSprite.transform.localPosition = new Vector3(tPackedSprite.transform.localPosition.x + space, tPackedSprite.transform.localPosition.y, tPackedSprite.transform.localPosition.z);
				}
			}
		}
	}
}



	
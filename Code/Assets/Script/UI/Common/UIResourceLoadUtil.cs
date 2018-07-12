using UnityEngine;
using System.Collections;
/// <summary>
/// load ui resourece
/// </summary>
public class UIResourceLoadUtil
{
	
	//load object from path
	public static Object Load(string path)
	{
		 return Resources.Load(path);
	}
	
	//load the uicamera
	public static Object LoadUICameraPreb()
	{
		string path = "TempArtist/UI/Prefab/Control/UICamera";
		return Load(path);
	}
	
	public static Object LoadXML(string xmlName)
	{
		string path = "Config/"+xmlName;
		return Load(path);
	}
	
	public static Object LoadIcon(string index)
	{
		string path = "TempArtist/UI/Prefab/Icon/"+index;
		return Load(path);
	}
	
	public static Object LoadControlImage(int index)
	{
		string path = "TempArtist/UI/Prefab/Control/ControlImage"+index;
		return Load(path);
	}
	
	public static Object LoadControlProgressBar(int index)
	{
		string path = "TempArtist/UI/Prefab/Control/ControlProgressBar"+index;
		return Load(path);
	}
	
	public static Object LoadControlRadio(int index)
	{
		string path = "TempArtist/UI/Prefab/Control/ControlRadio"+index;
		return Load(path);
	}
	
	public static Object LoadControlButton(int index)
	{
		string path = "TempArtist/UI/Prefab/Control/ControlButton"+index;
		return Load(path);
	}
	
	//load the icon 
	public static Object LoadIconPrefab()
	{
		string path = "TempArtist/UI/Prefab/Control/IconSimpleSprite";
		return Load(path);
	}
	//load the icon by index
	public static Object LoadIconByIndex(int index)
	{
		string path = "";
		switch(index)
		{
		case 1:
			path = "TempArtist/UI/Texture/Icon/Icon1";
			break;
		case 2:
			path = "TempArtist/UI/Texture/Icon/triangle1";
			break;
		}
		return Load(path);
	}
	
	//load main menu down preb
	public static Object LoadMainMenuDown(string prebName)
	{
		string path = "TempArtist/UI/Prefab/PanelMain/MainDownButton/"+prebName;
		return Load(path);
	}
	
	//load the preb of tips
	public static Object LoadControlTipsBG()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlTipsBG";
		return Load(path);
	}
	
	
	
	public static Object LoadControlButton1()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlButton1";
		return Load(path);
	}
	
	public static Object LoadControlButton2()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlButton2";
		return Load(path);
	}
	
	public static Object LoadControlButton3()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlButton3";
		return Load(path);
	}
	
	public static Object LoadControlImage3()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlImage3";
		return Load(path);
	}
	
	public static Object LoadControlImage4()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlImage4";
		return Load(path);
	}
	
	public static Object LoadControlRadio2()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlRadio2";
		return Load(path);
	}
	
	public static Object LoadControlRadio3()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlRadio3";
		return Load(path);
	}
	
	public static Object LoadControlToggle1()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlToggle1";
		return Load(path);
	}
	
	public static Object LoadControlDialog5()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlDialog5";
		return Load(path);
	}
	
	public static Object LoadControlDialog4()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlDialog4";
		return Load(path);
	}
	
	public static Object LoadControlDialog(int index)
	{
		string path = "TempArtist/UI/Prefab/Control/ControlDialog"+index;
		return Load(path);
	}
	
	//load the button 1
	public static Object LoadButton1()
	{
		string path = "TempArtist/UI/Prefab/Control/Button1";
		return Load(path);
	}
	
	public static Object LoadButton4()
	{
		string path = "TempArtist/UI/Prefab/Control/Button4";
		return Load(path);
	}
	
	public static Object LoadButton5()     
	{
		string path = "TempArtist/UI/Prefab/Control/Button5";
		return Load(path);
	}
	
	//load the button 6
	public static Object LoadButton6()
	{
		string path = "TempArtist/UI/Prefab/Control/Button6";
		return Load(path);
	}
	
	//load the button 7
	public static Object LoadButton7()
	{
		string path = "TempArtist/UI/Prefab/Control/Button7";
		return Load(path);
	}
	
	//load the button 8
	public static Object LoadButton8()
	{
		string path = "TempArtist/UI/Prefab/Control/Button8";
		return Load(path);
	}
	
	public static Object LoadButton9()
	{
		string path = "TempArtist/UI/Prefab/Control/Button9";
		return Load(path);
	}
	
	public static Object LoadButton10()
	{
		string path = "TempArtist/UI/Prefab/Control/Button10";
		return Load(path);
	}
	
	public static Object LoadButton11()
	{
		string path = "TempArtist/UI/Prefab/Control/Button11";
		return Load(path);
	}
	
	public static Object LoadControlProgressBar1()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlProgressBar1";
		return Load(path);
	}
	
	public static Object LoadControlProgressBar2()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlProgressBar2";
		return Load(path);
	}
	//load the object of panel login
	
	
	public static Object LoadPanelLogin()
	{
		string path = "TempArtist/UI/Prefab/PanelLogin/PanelLogin";
		return Load(path);
	}

	//load the textrue of role select view
	public static Object LoadTextureRoleMan(int index)
	{
		string path = "TempArtist/UI/Texture/PanelLogin/login_RoleMan"+index;
		//Debug.Log(path);
		return Load(path);
	}
	
	//load the textrue of role select view
	public static Object LoadTextureRoleWoman(int index)
	{
		string path = "TempArtist/UI/Texture/PanelLogin/login_RoleWoman"+index;
		//Debug.Log(path);
		return Load(path);
	}
	
	//load the preb of single role
	public static Object LoadSingleRolePreb()
	{
		string path = "TempArtist/UI/Prefab/PanelLogin/SingleRole";
		return Load(path);
	}
	
	//load the simpletext preb
	public static Object LoadSimpleTextPreb()
	{
		string path = "TempArtist/UI/Prefab/Common/UISimpleText";
		return Load(path);
	}
	
	//load the preb of loading panel
	public static Object LoadPanelLoading()
	{
		string path = "TempArtist/UI/Prefab/PanelLoading/PanelLoading";
		return Load(path);
	}
		
	//load the preb of text
	public static Object LoadSpriteText()
	{
		string path = "TempArtist/UI/Prefab/Common/UISimpleText";
		return Load(path);	
	}
	//load the preb of main menu top
	public static Object LoadMenuTop()
	{
		string path = "TempArtist/UI/Prefab/PanelMain/MainMenuTop";
		return Load(path);
	}
	//load the preb of mian menu bottom
	public static Object LoadMenuBottom()
	{
		string path = "TempArtist/UI/Prefab/PanelMain/MainMenuDown";
		return Load(path);
	}
	//load the preb button of port go
	public static Object LoadButtonPortGO()
	{
		string path = "TempArtist/UI/Prefab/PanelMain/ButtonPortGO";
		return Load(path);
	}
	
	//load the preb button of back port
	public static Object LoadButtonBackPort()
	{
		string path = "TempArtist/UI/Prefab/Copy/BackPortBtn";
		return Load(path);
	}
	
	//load the preb button of fast battle
	public static Object LoadButtonFastBattle()
	{
		string path = "TempArtist/UI/Prefab/Battle/QuickFightBtn";
		return Load(path);
	}
	
	//load the preb button of menu down
	public static Object LoadMainDownButtonShip()
	{
		string path = "TempArtist/UI/Prefab/PanelMain/MainDownButton/MainDownButtonShip1";
		return Load(path);
	}
	
	public static Object LoadMainDownButton(int index)
	{
		string path = "";
		switch(index)
		{
		case 0:
			path = "TempArtist/UI/Prefab/PanelMain/MainDownButton/MainDownButtonShip1";
			break;
		case 1:
			path = "TempArtist/UI/Prefab/PanelMain/MainDownButton/MainDownButtonMap1";
			break;
		case 2:
			path = "TempArtist/UI/Prefab/PanelMain/MainDownButton/MainDownButtonGeneral1";
			break;
		case 3:
			path = "TempArtist/UI/Prefab/PanelMain/MainDownButton/MainDownButtonFriends1";
			break;
		case 4:
			path = "TempArtist/UI/Prefab/PanelMain/MainDownButton/MainDownButtonDiary1";
			break;
		case 5:
			path = "TempArtist/UI/Prefab/PanelMain/MainDownButton/MainDownButtonBackpack1";
			break;		
		}
		return Load(path);
	}
	
	
	
	//load the preb list button of menu bottom
//	public static Object LoadMenuBottomListButton(EnumListButton button)
//	{
//		string buttonName = "";
//		switch (button)
//		{
//		case EnumListButton.ListButtonFleet:
//			buttonName = "MainListButtonFleet";
//			break;
//		case EnumListButton.ListButtonFormation:
//			buttonName = "MainListButtonFormation";
//			break;
//		case EnumListButton.ListButtonGeneral:
//			buttonName = "MainListButtonGeneral";
//			break;
//		case EnumListButton.ListButtonLog:
//			buttonName = "MainListButtonLog";
//			break;
//		case EnumListButton.ListButtonMap:
//			buttonName = "MainListButtonMap";
//			break;
//		case EnumListButton.ListButtonStorage:
//			buttonName = "MainListButtonStorage";
//			break;
//		}
//		string path = "TempArtist/UI/Prefab/PanelMain/MainListButton/"+buttonName;
//		return Load(path);
//	}
	//laod the preb main menu left
	public static Object LoadMainMenuLeft()
	{
		string path = "TempArtist/UI/Prefab/Common/MenuLeft/MainMenuLeft";
		return Load(path);
	}
	//
	public static Object LoadMainMenuLeftButton1()
	{
		string path = "TempArtist/UI/Prefab/Common/MenuLeft/MainMenuLeftListButton1";
		return Load(path);
	}
	public static Object LoadMainMenuLeftButton2()
	{
		string path = "TempArtist/UI/Prefab/Common/MenuLeft/MainMenuLeftListButton2";
		return Load(path);
	}
	public static Object LoadMainMenuLeftButton3()
	{
		string path = "TempArtist/UI/Prefab/Common/MenuLeft/MainMenuLeftListButton3";
		return Load(path);
	}
	//load the preb of main menu left down
	public static Object LoadMainMenuLeftDown()
	{
		string path = "TempArtist/UI/Prefab/PanelMain/MainMenuLeftDown";
		return Load(path);
	}
	//load the preb of main menu right top
	public static Object LoadMainMenuRightTop()
	{
		string path = "TempArtist/UI/Prefab/PanelMain/MainMenuRightTop";
		return Load(path);
	}
	
	//load the preb of BGMaskLayer
	public static Object LoadBGMaskLayer()
	{
		string path = "TempArtist/UI/Prefab/Common/BGMaskLayer";
		return Load(path);
	}

	//load the preb of dialog rank
	public static Object LoadMainDialogRank()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlDialog1";
		return Load(path);
	} 
	
	//load the preb of dialog port 
	public static Object LoadMainDialogPort()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlDialog2";
		return Load(path);
	}
	
	//load the control of dialog 3
	public static Object LoadControlDialog3()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlDialog3";
		return Load(path);
	}
	
	//load the preb of dialog general main
	public static Object LoadMainDialogGeneralCommon()
	{
		string path = "TempArtist/UI/Prefab/PanelMain/DialogGeneralCommon";
		return Load(path);
	}
	
	//load the preb of uiscrollist
	public static Object LoadGeneralScrollList()
	{
		string path = "TempArtist/UI/Prefab/PanelMain/GeneralScrollList";
		return Load(path);
	}
	
	public static Object LoadDialogGeneralTriangleMode(int index)
	{
		string path = "";
		switch(index)
		{
		case 1:
			path = "TempArtist/UI/Prefab/PanelMain/GeneralDialogTriangleMode/ButtonTriangle1";
			break;
		case 2:
			path = "TempArtist/UI/Prefab/PanelMain/GeneralDialogTriangleMode/ButtonTriangle2";
			break;
		case 3:
			path = "TempArtist/UI/Prefab/PanelMain/GeneralDialogTriangleMode/ButtonTriangle3";
			break;
		case 4:
			path = "TempArtist/UI/Prefab/PanelMain/GeneralDialogTriangleMode/ButtonTriangle4";
			break;
		case 5:
			path = "TempArtist/UI/Prefab/PanelMain/GeneralDialogTriangleMode/ButtonTriangle5";
			break;
			
		}
		return Load(path);
	}
	
	public static Object LoadControlImage1()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlImage1";
		return Load(path);
	}
	
	// load the control of general panel drug 
	public static Object LoadControlProgressDrug(int index)
	{
		string path = "TempArtist/UI/Prefab/Control/ControlProgressDrug"+index;
		return Load(path);
	}
	
	//load the preb of battle fleet all buffer
	public static Object LoadFleetAllBuffer()
	{
		string path = "TempArtist/UI/Prefab/PanelBattle/FleetAllBuffer";
		return Load(path);
	}
	
	//load the preb of battle message
	public static Object LoadBattleShipMessageImage()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlImage2";
		return Load(path);
	}
	
	//load the preb of tips 
	public static Object LoadTipsPreb()
	{
		string path = "TempArtist/UI/Prefab/Common/EZTips";
		return Load(path);
	}
	//load the preb of  copy select
	public static Object LoadCopySelect()
	{
		string path = "TempArtist/UI/Prefab/CopySelect/CopySelect";
		return Load(path);
	}
	
	//load the preb of single copy
	public static Object LoadSingleCopyPreb()
	{
		string path = "TempArtist/UI/Prefab/CopySelect/SingleCopy"; 
		return Load(path);
	}
	
	// load the preb of LoadDialogCopyWipe
	public static Object LoadDialogCopyWipe()
	{
		string path = "TempArtist/UI/Prefab/CopySelect/DialogCopyWipe"; 
		return Load(path);
	}
	
	public static Object LoadBattleDialogCopyValuation()
	{
		string path = "TempArtist/UI/Prefab/Control/ControlDialog2";
		return Load(path);
	}
	//load the preb of blood strip
	public static Object LoadBattleBloodStrip()
	{
		string path = "TempArtist/UI/Prefab/PanelBattle/BattleBloodStrip";
		return Load(path);
	}
	
	//load the preb of battle effect font
	public static Object LoadEffectFont()
	{
		string path = "TempArtist/UI/Prefab/PanelBattle/EffectFont";
		return Load(path);
	}
	
	public static Object LoadNumber()
	{
		string path = "TempArtist/UI/Prefab/PanelBattle/Number";
		return Load(path);
	}
	
	//load the textrue of copy select view
	public static Object LoadTextureOfCopySelect(int index)
	{
		//string path = "UI/Texture/Copy/copy"+index;
		//   test
		string path = "TempArtist/UI/Texture/CopySelect/copy_Select-Copy_map"+index;
		return Load(path);
	}
	
	public static Object LoadTextureLockedOfCopySelect()
	{
		string path = "TempArtist/UI/Texture/CopySelect/CopyMapLocked";
		return Load(path);
	}
	
	//load the preb of copy select view touchpad
	public static Object LoadTouchPadPreb()
	{
		string path = "TempArtist/UI/Prefab/Common/UITouchPad";
		return Load(path);
	}
	
	public static Object LoadUISlider1()
	{
		string path = "TempArtist/UI/Prefab/Control/UISlider1";
		return Load(path);
	}
	
	public static Object LoadUISlider2()
	{
		string path = "TempArtist/UI/Prefab/Control/UISlider2";
		return Load(path);
	}
	
	public static Object LoadTextField1()
	{
		string path = "TempArtist/UI/Prefab/Control/TextField1";
		return Load(path);
	}
	
}

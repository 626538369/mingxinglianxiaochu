using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace GfanTools
{
	public class Utils
	{
		public static bool IsDirectory(string path)
		{
			FileInfo fi = new FileInfo(path);
			
			if (
				(FileAttributes)(-1) != fi.Attributes
				&& 0 != (fi.Attributes & FileAttributes.Directory)
				)
			{
				return true;
			}
			
			return false;
		}
		
		public static bool IsFile(string path)
		{
			FileInfo fi = new FileInfo(path);
			
			if (
				(FileAttributes)(-1) != fi.Attributes
				&& (0 != (fi.Attributes & FileAttributes.Archive)
				|| 0 != (fi.Attributes & FileAttributes.Hidden)
				|| 0 != (fi.Attributes & FileAttributes.System)
				|| 0 != (fi.Attributes & FileAttributes.ReadOnly))
				)
			{
				return true;
			}
			
			return false;
		}
		
		public static bool IsUnityLevel(string path)
		{
			return path.EndsWith(".unity");
		}
	}
	
	
	public class ToolsMenu
	{
		// [UnityEditor.MenuItem("GfanTools/AssetBundle/Build Selection (Only self)")]
		// public static void DisplayBundleWizard()
		// {
		// }
		// 
		// [UnityEditor.MenuItem("GfanTools/AssetBundle/Build Selection (Track dependency)")]
		// public static void DisplayBundleWizard()
		// {
		// }
		// 
		// [UnityEditor.MenuItem("GfanTools/AssetBundle/Build Selecting Level ()")]
		// public static void DisplayBundleWizard()
		// {
		// }
		// 
		// [UnityEditor.MenuItem("GfanTools/AssetBundle/Build Level ()")]
		// public static void DisplayBundleWizard()
		// {
		// }
		
		[UnityEditor.MenuItem("GfanTools/BuildAssetBundle")]
		public static void DisplayBundleWizard()
		{
			AssetBundleWizard wizard = (AssetBundleWizard)ScriptableWizard.DisplayWizard(
				"AssetBundle", 
				typeof(AssetBundleWizard), 
				"Ok", 
				"Cancel");
			
			wizard.helpString = "NOTE: Press Play to ensure any screen-relative sizing or placement settings are properly re-calculated.";
			wizard.LoadSettings();
		}
		
		//Shortcut keys: % - Ctrl    & - Alt  # - Shift
		[UnityEditor.MenuItem("GfanTools/Open or Close the shadow property %&Q")]
		public static void DisplayOptimizeWizard()
		{
			OptimizeWizard wizard = (OptimizeWizard)ScriptableWizard.DisplayWizard("OptimizeSetting", typeof(OptimizeWizard), "Ok");
			wizard.helpString = "NOTE: Optimize some things.";
			wizard.LoadSettings();
		}
		
		[UnityEditor.MenuItem("GfanTools/Itween Animation Editor %&E")]
		public static void DisplayItweenAnimWizard()
		{
			// ItweenAnimWizard wizard = (ItweenAnimWizard)ScriptableWizard.DisplayWizard("Itween Animation Editor", typeof(ItweenAnimWizard), "Ok", "AddPoint");
			// wizard.helpString = "NOTE: Press AddPoint will add the current point info at the end of list. you can manual modify the time options.";
			// wizard.LoadSettings();
			ItweenAnimWizard wizard = EditorWindow.GetWindow(typeof(ItweenAnimWizard)) as ItweenAnimWizard;
			wizard.LoadSettings();
		}
		
		// [ContextMenu("GfanTools/Optimize/ContolSetting")]
		// public static void ContolSetting()
		// {
		// 	Debug.Log("LiHaojie Test Plugin");
		// }
	}
}

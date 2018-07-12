using System;
using System.Collections.Generic;
using UnityEngine;
using Object=UnityEngine.Object;

// When analyzing the available assets UpdateCharacterElementDatabase creates
// a CharacterElement for each possible element. For instance, one mesh with
// three possible textures results in three CharacterElements. All 
// CharacterElements are saved as part the CharacterGenerator ScriptableObject,
// and can be used on runtime to download and load the assets required for the
// element they represent.
[Serializable]
public class CharacterElement
{
    public string name;
    public string bundleName;
	public static int AssetBundleVersion = 1;
    // The WWWs for retrieving the appropriate assetbundle are stored 
    // statically, so CharacterElements that share an assetbundle can
    // use the same WWW.
    // path to assetbundle -> WWW for retieving required assets
    private  WWW wwwAsset;

    // The required assets are loaded asynchronously to avoid delays
    // when first using them. A LoadAsync results in an AssetBundleRequest
    // which are stored here so we can check their progress and use the
    // assets they contain once they are loaded.
    public AssetBundleRequest gameObjectRequest;
    AssetBundleRequest boneNameRequest;
	
	
	static Dictionary<string,AssetBundleRequest>  materialRequestList = new Dictionary<string, AssetBundleRequest>();
	
    public CharacterElement(string name, string bundleName)
    {
        this.name = name;
        this.bundleName = bundleName;
    }

    // Returns the WWW for retieving the assetbundle required for this 
    // CharacterElement, and creates a WWW only if one doesnt exist already. 
    public WWW CreateWWWBundle(string assetPath,int assetBundleVersion)
    {
       	if (wwwAsset == null)
		{
			wwwAsset = WWW.LoadFromCacheOrDownload(assetPath + bundleName + ".assetbundle",assetBundleVersion);
		}      
		return wwwAsset;
    }
	
	public WWW GetWWWBundle()
    {
          return wwwAsset;
    }
	


}
using UnityEngine;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
	public enum LoadingState
	{
		INITIAL,
		
		CONNECTING,
		CONNECTED,
		
		REQUEST_CHANGE,
		RECEIVE_CHANGE,
		
		LOAD_SCENE,
		LOADING_SCENE,
		LOADED
	}
	
	public LoadingState MLoadingState
	{
		get { return _loadingState; }
		set { _loadingState = value; }
	}
	
	protected void Awake()
	{
		_isVisible = true;
		_mTime = 0.0f;
		_loadingState = LoadingState.INITIAL;
	}
	
	protected void Start()
	{
//		Globals.Instance.MSystemUIMain.PanelChange(PanelName.PanelLoading);
//		_uIPanelLoading = Globals.Instance.MSystemUIMain._uIPanelLoading;
	}
	
	public void Release()
	{
		_isVisible = false;
	}
	
	public void Update()
	{
		if (!_isVisible)
			return;
		
		if (_isPaused)
			return;
		
		_mTime += Time.deltaTime;
		SetProgress(_mTime / (_maxTime * 0.9999f));
		
		if (_mTime >= _maxTime)
			_loadingState = LoadingState.LOADED;
		
		switch (_loadingState)
		{
		case LoadingState.CONNECTING:
		{
			// Do somethings
			break;
		}
		case LoadingState.LOAD_SCENE:
		{
			// Release last 3d scene information, maybe is asynchronism
			// In u3d, maybe do not need
			
			break;
		}
		case LoadingState.LOADING_SCENE:
		{
			// Call load 3d scene, maybe is asynchronism
			
			break;
		}
		case LoadingState.LOADED:
		{
			// Switch into the new GameState
			SetProgress(1.0f);
			_isVisible = false;
			
			break;
		}
		}
	}
	
	public void SetVisible(bool visible)
	{
		_isVisible = visible;
		
		if (visible)
		{
		}
	}
	
	public void SetProgress(float progress)
	{
//		progress = Mathf.Clamp(progress, 0.0f, 1.0f);
//		_progress = progress;
//		
//		if (_uIPanelLoading == null)
//		{
//			return;
//		}
//		
//		_uIPanelLoading.progressGo(progress);
	}
	
	public void SetProgressText(string text)
	{
	//	_uIPanelLoading.SetLoadingTypeText(text);
	}
	
	public void SetProgressImage(string imageName)
	{}
	
	private bool _isVisible = false;
	private bool _isPaused = false;
	private float _progress = 0.0f;
	private LoadingState _loadingState = LoadingState.INITIAL;
	
//	private UIPanelLoading _uIPanelLoading;
	
	private float _mTime = 0.0f;
	private float _maxTime = 2.0f;
}

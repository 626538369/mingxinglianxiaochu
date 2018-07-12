using UnityEngine;
using System.Collections;

public enum ZoomMethod
{
    // move the camera position forward/backward
    Position,

    // change the field of view of the camera, or projection size for orthographic cameras
    FOV,
}

public class CameraSmoothZoom : CameraBehaviour 
{
	#region Class Property
	public ZoomMethod ZoomMethod
	{
		get { return _zoomMethod; }
		set { _zoomMethod = value; }
	}
	
	public Vector3 CameraInitPosition
	{
		get { return _cameraInitPosition; }
		set { _cameraInitPosition = value; }
	}
	
	public float CameraInitFOV
	{
		get { return _cameraInitFOV; }
		set { _cameraInitFOV = value; }
	}
	
	public float CameraInitOrthoSize
	{
		get { return _cameraInitOrthoSize; }
		set { _cameraInitOrthoSize = value; }
	}
	
	public float ZoomSpeed
	{
		get { return _zoomSpeed; }
		set { _zoomSpeed = value; }
	}
	
	public float ZoomDelta
	{
		get { return _zoomDelta; }
		set { _zoomDelta = value; }
	}
	
	public float MinZoomAmount
	{
		get { return _minZoomAmount; }
		set { _minZoomAmount = value; }
	}
	
	public float MaxZoomAmount
	{
		get { return _maxZoomAmount; }
		set { _maxZoomAmount = value; }
	}
	#endregion
	
	
	// Update is called once per frame
	public override void LateUpdate () 
	{
		if (isUseCoroutine)
			return;
		
		if (!enabled)
			return;
		
		_zoomAmount += _zoomSpeed * _zoomDelta;
		_zoomAmount = Mathf.Clamp( _zoomAmount, _minZoomAmount, _maxZoomAmount );
		
        switch( _zoomMethod )
        {
            case ZoomMethod.Position:
			{
				Globals.Instance.MSceneManager.mMainCamera.transform.position = _cameraInitPosition + _zoomAmount * Globals.Instance.MSceneManager.mMainCamera.transform.forward;
	            break;
			}
            case ZoomMethod.FOV:
                if( Globals.Instance.MSceneManager.mMainCamera.orthographic )
                    Globals.Instance.MSceneManager.mMainCamera.orthographicSize = Mathf.Max( _cameraInitOrthoSize - _zoomAmount, 0.1f );
                else
                    Globals.Instance.MSceneManager.mMainCamera.fov = Mathf.Max( _cameraInitFOV - _zoomAmount, 0.1f );
                break;
        }
	}
	
	public void Reset()
	{
		_cameraInitPosition = Globals.Instance.MSceneManager.mMainCamera.transform.position;
		_cameraInitFOV = Globals.Instance.MSceneManager.mMainCamera.fov;
		_cameraInitOrthoSize = Globals.Instance.MSceneManager.mMainCamera.orthographicSize;
	}
	
	private Vector3 _cameraInitPosition;
	private float _cameraInitFOV;
	private float _cameraInitOrthoSize;
	
    private ZoomMethod _zoomMethod = ZoomMethod.Position;
	private float _zoomDelta = 1.0f;
    private float _zoomSpeed = 0.1f;
    private float _zoomAmount = 0;
    private float _minZoomAmount = 0.0f;
    private float _maxZoomAmount = 30;
}

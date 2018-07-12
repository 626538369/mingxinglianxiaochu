using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTrackPoint
{
	public Vector3 Position;
	public Vector3 EulerAngles;
	public float SpeedTimes;
	public float DelayTimes;
	
	public float Speed;
}

public class CameraTrackController : MonoBehaviour 
{
	public void Awake()
	{
		_mCamTrackList = new Dictionary<string, CameraTrack>();
		MonoBehaviour.DontDestroyOnLoad(this);
	}
	
	void Start()
	{
	}
	
	void OnDisable()
	{
		_mCamTrackList.Clear();
	}
	
	void OnDestroy()
	{
		_mCamTrackList.Clear();
	}
	
	/// <summary>
	/// Starts the track.
	/// </summary>
	/// <returns>
	/// The track.
	/// </returns>
	/// <param name='trackName'>
	/// Track name prefab name in Resource folder
	/// </param>
	/// <param name='relativeTf'>
	/// Relative tf.
	/// </param>
	/// <param name='trackRelative'>
	/// Track relative.
	/// </param>
	public CameraTrack StartTrack(string trackName, Transform relativeTf, bool trackRelative = false){
		
		CameraTrack track = null;
		
		if (!_mCamTrackList.TryGetValue(trackName, out track)){
			
			Object obj = Resources.Load(trackName);
			GameObject go = GameObject.Instantiate(obj) as GameObject;
			
			go.name = trackName;
			track = go.GetComponent<CameraTrack>() as CameraTrack;
			
			_mCamTrackList.Add(trackName, track);
		}
		
		if(trackRelative){
			track.Restart(relativeTf.position);
		}else{
			track.Restart();
		}
				
		return track;
	}
	
	/// <summary>
	/// Starts the nivose track.
	/// </summary>
	/// <returns>
	/// The nivose track.
	/// </returns>
	/// <param name='trackName'>
	/// Track name.
	/// </param>
	/// <param name='dataSlot'>
	/// Data slot.
	/// </param>
	public CameraTrack StartNivoseTrack(string trackName,SkillDataSlot dataSlot)
	{
		Vector3 tStartPos	= Globals.Instance.MPlayerManager.GetWarship(dataSlot._attackerID).U3DGameObject.transform.position;
		Vector3 tTargetPos	= Vector3.zero;
		
		foreach(SkillDataSlot.AttackTargetData data in dataSlot._attackTargetDataList.Values){
			
			if(data._isPrimaryTarget){
				
				WarshipL target = Globals.Instance.MPlayerManager.GetWarship(data._targetID);
				tTargetPos = target.U3DGameObject.transform.position;
				
				break;
			}
		}
		
		CameraTrack track = null;
		
		if (!_mCamTrackList.TryGetValue(trackName, out track)){
			
			Object obj = Resources.Load(trackName);
			GameObject go = GameObject.Instantiate(obj) as GameObject;
			go.name = trackName;
			track = go.GetComponent<CameraTrack>() as CameraTrack;
			_mCamTrackList.Add(trackName, track);
		}
		
		track.RestartNivoseTrack(true,tStartPos,tTargetPos);
		
		return track;
	}
	
	
		
	public void ResumeTrack(string trackName, int index)
	{
		CameraTrack track = null;
		if (_mCamTrackList.TryGetValue(trackName, out track))
		{
			if(index != -1){
				track.Index = index;
			}			
			track.Resume();
		}
	}
	
	//! return end stop position
	public KeyFrameInfo StopTrack(string trackName)
	{
		KeyFrameInfo t_endPos = null;
		
		CameraTrack track = null;
		if (_mCamTrackList.TryGetValue(trackName, out track))
		{
			t_endPos = track.Stop();
			
			_mCamTrackList.Remove(trackName);
			GameObject.Destroy(track.gameObject);
		}
		
		return t_endPos;
	}
	
	public void StopAllTracks()
	{
		foreach (CameraTrack track in _mCamTrackList.Values)
		{
			track.Stop();
		}
	}
	
	//! tzz added for pause the track by name 
	public void PauseTrack(string trackName){
		CameraTrack track = null;
		if (_mCamTrackList.TryGetValue(trackName, out track)){
			track.Stop();
		}
	}
	
	
	private void StartMoveToPoints()
	{
		StartCoroutine( DoMoveToPoint() );
	}
		
	private IEnumerator DoMoveToPoint()
	{
		CameraTrackPoint fromPoint = null;
		CameraTrackPoint toPoint = null;
		CameraTrackPoint[] pointArray = null;
		
		if (_mTrackModule == 0)
		{
			pointArray = _mCameraTrackPointList.ToArray();
		}
		else if (_mTrackModule == 1)
		{
			pointArray = _mCameraTrackPointList1.ToArray();
		}
		else if (_mTrackModule == 2)
		{
			pointArray = _mCameraTrackPointList2.ToArray();
		}
		
		for (int index = 0; index < pointArray.Length; ++index)
		{
			if (0 == index)
			{
				fromPoint = null;
				toPoint = pointArray[index];
			}
			else
			{
				fromPoint = toPoint;
				toPoint = pointArray[index];
				
				float length = (toPoint.Position - fromPoint.Position).magnitude;
				fromPoint.Speed = length / fromPoint.SpeedTimes;
			}
			
			while (true)
			{
				if (fromPoint == null)
				{
					Globals.Instance.MSceneManager.mMainCamera.transform.rotation = Quaternion.Euler(toPoint.EulerAngles);
					Globals.Instance.MSceneManager.mMainCamera.transform.position = toPoint.Position;
					
					yield return new WaitForSeconds(toPoint.DelayTimes);
					break;
				}
				
				if (fromPoint != null && toPoint != null)
				{
					float moveDamping = 0.5f;
					float rotateDamping = 1.0f / fromPoint.SpeedTimes;
					
					float speed = fromPoint.Speed;
					float distance = (Globals.Instance.MSceneManager.mMainCamera.transform.position- toPoint.Position).magnitude;
					float squareMovement = speed * moveDamping * Time.deltaTime;
					// squareMovement *= squareMovement;
					
					if (distance <= squareMovement)
					{
						Globals.Instance.MSceneManager.mMainCamera.transform.rotation = Quaternion.Euler(toPoint.EulerAngles);
						Globals.Instance.MSceneManager.mMainCamera.transform.position = toPoint.Position;
						
						yield return new WaitForSeconds(toPoint.DelayTimes);
						break;
					}
					
					while (distance > squareMovement)
					{
						Quaternion currentRotation = Globals.Instance.MSceneManager.mMainCamera.transform.rotation;
						Vector3 destEulerAngles = toPoint.EulerAngles;
						Quaternion destRotation = Quaternion.Euler(destEulerAngles);
						
						Quaternion wantedRotation = Quaternion.Slerp(currentRotation, destRotation, rotateDamping * Time.deltaTime);
						
						Vector3 currentPos = Globals.Instance.MSceneManager.mMainCamera.transform.position;
						Vector3 destDirection = toPoint.Position - currentPos;
						destDirection.Normalize();
						
						Vector3 wantedPosition = currentPos;
						wantedPosition += destDirection * speed * moveDamping * Time.deltaTime;
						
						Globals.Instance.MSceneManager.mMainCamera.transform.rotation = wantedRotation;
						Globals.Instance.MSceneManager.mMainCamera.transform.position = wantedPosition;
						
						distance = (Globals.Instance.MSceneManager.mMainCamera.transform.position- toPoint.Position).magnitude;
						squareMovement = speed * moveDamping * Time.deltaTime;
						// squareMovement *= squareMovement;
						
						yield return null;
					}
					
				}
				
			} // End while(true)
			
		} // for (int index = 0; index < pointArray.Length; ++index)
	}
	
	public void TestData()
	{
		{
			CameraTrackPoint point = new CameraTrackPoint();
			point.Position = new Vector3(-200f,185f, -186f);
			point.EulerAngles = new Vector3(45f, 0f, 0);
			point.SpeedTimes = 2;
			point.DelayTimes = 2;
			_mCameraTrackPointList.Add(point);
			
			point = new CameraTrackPoint();
			point.Position = new Vector3(587f, 70f, -105f);
			point.EulerAngles = new Vector3(15f,15f, 0f);
			point.SpeedTimes = 2;
			point.DelayTimes = 0;
			_mCameraTrackPointList.Add(point);
			
			point = new CameraTrackPoint();
			point.Position = new Vector3(690f, 122f, -94f);
			point.EulerAngles = new Vector3(30f, 0, 0);
			point.SpeedTimes = 3;
			point.DelayTimes = 0;
			_mCameraTrackPointList.Add(point);
		}

		{
			CameraTrackPoint point = new CameraTrackPoint();
			point.Position = new Vector3(-200f,185f, -186f);
			point.EulerAngles = new Vector3(45f, 0f, 0);
			point.SpeedTimes = 2;
			point.DelayTimes = 2;
			_mCameraTrackPointList1.Add(point);
			
			point = new CameraTrackPoint();
			point.Position = new Vector3(440f, 140f, -241f);
			point.EulerAngles = new Vector3(13f,22f, 0f);
			point.SpeedTimes = 2;
			point.DelayTimes = 0;
			_mCameraTrackPointList1.Add(point);
			
			point = new CameraTrackPoint();
			point.Position = new Vector3(690f, 240f, -240f);
			point.EulerAngles = new Vector3(35f, 0, 0);
			point.SpeedTimes = 4;
			point.DelayTimes = 0;
			_mCameraTrackPointList1.Add(point);
		}
		
		{
			CameraTrackPoint point = new CameraTrackPoint();
			point.Position = new Vector3(-200f,185f, -186f);
			point.EulerAngles = new Vector3(45f, 0f, 0);
			point.SpeedTimes = 2;
			point.DelayTimes = 2;
			_mCameraTrackPointList2.Add(point);
			
			point = new CameraTrackPoint();
			point.Position = new Vector3(440f, 140f, -241f);
			point.EulerAngles = new Vector3(13f,22f, 0f);
			point.SpeedTimes = 2;
			point.DelayTimes = 0;
			_mCameraTrackPointList2.Add(point);
			
			point = new CameraTrackPoint();
			point.Position = new Vector3(690f, 240f, -240f);
			point.EulerAngles = new Vector3(35f, 0, 0);
			point.SpeedTimes = 4;
			point.DelayTimes = 0;
			_mCameraTrackPointList2.Add(point);
		}
	}
	
	public List<CameraTrackPoint> _mCameraTrackPointList = new List<CameraTrackPoint>();
	public List<CameraTrackPoint> _mCameraTrackPointList1 = new List<CameraTrackPoint>();
	public List<CameraTrackPoint> _mCameraTrackPointList2 = new List<CameraTrackPoint>();
	
	public CameraTrackPoint[] pointArray = new CameraTrackPoint[10];
	
	private int _mTrackModule;
	
	private Dictionary<string, CameraTrack> _mCamTrackList;
}

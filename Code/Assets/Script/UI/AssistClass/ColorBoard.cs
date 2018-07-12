using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBoardEventArgs : EventArgs
{
    public Color submitColor;
}

public class ColorBoard : MonoBehaviour {

    /// <summary>
    /// �����������Լ�
    /// </summary>

    public event EventHandler<ColorBoardEventArgs> CommitColor;
	public event EventHandler CancelColor;
	
    private Camera uiCamera;
    UISlider toneSlider;
    UISlider redSlider;
    UISlider greenSlider;
    UISlider blueSlider;
    UISprite colorView;
    UIInput RedInput;
    UIInput GreenInput;
    UIInput BlueInput;
    Transform colorPoint;
    UISprite resultColor;
    UIButton OKBtn;
    /// <summary> ����ʱ����Ŀؼ� </summary>
    List<Transform> mutexObjs = new List<Transform>();
    //Vector3 colorViewTopRight;
    //Vector3 colorViewBottomRight;
    //Vector3 colorViewTopLeft;
    //Vector3 colorViewBottomLeft;
    float factor = 1f / 6f;  //0.1666667f
    Color curTone = Color.red;

    /// <summary>
    /// ��ColoView��Χ�ڰ��������
    /// </summary>
    bool isPressOnColorViewArea;

    Plane colorViewPlane;
	
	Color mInitColor;

    Vector3 colorPointOriginalPos;

	// Use this for initialization
    void Start()
    {
		uiCamera = Globals.Instance.MGUIManager.MGUICamera;
        toneSlider = transform.Find("tone Slider").GetComponent<UISlider>();
        redSlider = transform.Find("1R group/R Slider").GetComponent<UISlider>();
        greenSlider = transform.Find("2G group/G Slider").GetComponent<UISlider>();
        blueSlider = transform.Find("3B group/B Slider").GetComponent<UISlider>();
        colorView = transform.Find("color view group/color view").GetComponent<UISprite>();
        resultColor = transform.Find("resultColor").GetComponent<UISprite>();
        colorPoint = transform.Find("color view group/color point");
        OKBtn = transform.Find("OKBtn").GetComponent<UIButton>();
        RedInput = transform.Find("1R group/R Input").GetComponent<UIInput>();
        GreenInput = transform.Find("2G group/G Input").GetComponent<UIInput>();
        BlueInput = transform.Find("3B group/B Input").GetComponent<UIInput>();
        colorViewPlane.SetNormalAndPosition(-colorView.transform.forward, colorView.GetComponent<Collider>().bounds.center);
        colorPointOriginalPos = colorPoint.position;
		EventDelegate.Add(toneSlider.onChange, ToneSliderValueChange);
   		EventDelegate.Add(redSlider.onChange, RGBSliderValueChanged);
        EventDelegate.Add(greenSlider.onChange, RGBSliderValueChanged);
 		EventDelegate.Add(blueSlider.onChange, RGBSliderValueChanged);
		
		EventDelegate.Add(RedInput.onChange, UpdateRedSliderValue);
		EventDelegate.Add(GreenInput.onChange, UpdateGreenSliderValue);
		EventDelegate.Add(BlueInput.onChange, UpdateBlueSliderValue);

        mutexObjs.Add(colorView.transform);
        mutexObjs.Add(toneSlider.transform);
        mutexObjs.Add(redSlider.transform);
        mutexObjs.Add(greenSlider.transform);
        mutexObjs.Add(blueSlider.transform);
        //CheckTouch();
		
		redSlider.sliderValue = mInitColor.r;
		greenSlider.sliderValue = mInitColor.g;
		blueSlider.sliderValue = mInitColor.b;
		resultColor.color = mInitColor;
    }

    void ToneSliderValueChange()
    {
        int _multiple = (int)(toneSlider.sliderValue / factor);
        float _curToneGreenValue = 0f;
        float _curToneRedValue = 0f;
        float _curToneBlueValue = 0f;
        //Debug.Log(_multiple);
        switch (_multiple)
        {
            case 0:
                _curToneGreenValue = toneSlider.sliderValue / factor;
                curTone = new Color(1f, _curToneGreenValue, 0f);
                break;
            case 1:
                _curToneRedValue = (toneSlider.sliderValue - factor) / factor;
                curTone = new Color(1 - _curToneRedValue, 1f, 0f);
                break;
            case 2:
                _curToneBlueValue = (toneSlider.sliderValue - 2 * factor) / factor;
                curTone = new Color(0f, 1f, _curToneBlueValue);
                break;
            case 3:
                _curToneGreenValue = (toneSlider.sliderValue - 3 * factor) / factor;
                curTone = new Color(0f, 1 - _curToneGreenValue, 1f);
                break;
            case 4:
                _curToneRedValue = (toneSlider.sliderValue - 4 * factor) / factor;
                curTone = new Color(_curToneRedValue, 0f, 1f);
                break;
            case 5:
                _curToneBlueValue = (toneSlider.sliderValue - 5 * factor) / factor;
                curTone = new Color(1f, 0f, 1 - _curToneBlueValue);
                break;
        }
        //�ı���ɫ�ӿ�����ʾ����ɫ
        colorView.color = curTone;
        if (isRedSliderClicked || isGreenSliderClicked || isBlueSliderClicked)
            return;
        ChangeSliderValue();
    }

    /*/// <summary>
    /// ���»���ؼ��ĸ��ǵ�ǰ�����ģ�t��nullʱ��ʾû��ѡ���κοؼ������л���ؼ�collider��enable
    /// </summary>
    /// <param name="t"></param>
    void UpdateEvent(Transform t)
    {
        if (t)
        {
            foreach (Transform child in mutexObjs)
            {
                if (t.Equals(child))
                {
                    child.collider.enabled = true;
                }
                else
                {
                    child.collider.enabled = false;
                }
            }
        }
        else
        {
            foreach (Transform child in mutexObjs)
            {
                child.collider.enabled = true;
            }
        }
    }
    */

    //bool CheckTouch()
    //{
    //    if (Application.platform == RuntimePlatform.Android)
    //    {
    //        if (Input.touchCount > 1)
    //        {
    //            EnableMutexObjsCollider(false);
    //            return false;
    //        }
    //        else
    //        {
    //            EnableMutexObjsCollider(true);
    //            return true;
    //        }
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}

    void EnableMutexObjsCollider(bool isEnable)
    {
        foreach (Transform t in mutexObjs)
        {
            t.GetComponent<Collider>().enabled = false;
        }
    }

	// Update is called once per frame
    void Update()
    {
        //CheckTouch();
        if (Input.GetMouseButtonDown(0))
        {
            //��ֹƽ���㴥��ͬʱ�������UI�ؼ�
            //Transform _result = CheckBounds();
            CheckBounds();
            //UpdateEvent(_result);
        }
        if (Input.GetMouseButton(0))
        {
            if (isPressOnColorViewArea)
            {
                LocateColorPoint();
                ChangeSliderValue();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //UpdateEvent(null);
            isRedSliderClicked = false;
            isGreenSliderClicked = false;
            isBlueSliderClicked = false;
            isToneSliderClicked = false;
            isPressOnColorViewArea = false;
        }
    }

    /// <summary>
    /// ����ѡ����ɫԲȦ��λ��
    /// </summary>
    void LocateColorPoint()
    {
        Ray _ray = uiCamera.ScreenPointToRay(Input.mousePosition);
        float _dis = -1f;
        bool _isIntersect = colorView.GetComponent<Collider>().bounds.IntersectRay(_ray, out _dis);
        Vector3 _colorPointSize = colorPoint.GetComponent<Collider>().bounds.size;
        Bounds _colorViewBounds = colorView.GetComponent<Collider>().bounds;
        _colorPointSize.z = 0f;
        if (_isIntersect)
        {
            Vector3 _colorPointPos = _ray.origin + _ray.direction * _dis - _colorPointSize / 2f;
            if (_colorPointPos.x >= _colorViewBounds.min.x && _colorPointPos.x + _colorPointSize.x <= _colorViewBounds.max.x &&
                _colorPointPos.y >= _colorViewBounds.min.y && _colorPointPos.y + _colorPointSize.y <= _colorViewBounds.max.y)
            {
                _colorPointPos.z = colorPoint.position.z;
                colorPoint.position = _colorPointPos;
            }
        }
        else
        {
            _dis = -1f;
            colorViewPlane.Raycast(_ray, out _dis);
            Vector3 _result = colorPoint.position;
            Vector3 _colorPointPos = _ray.origin + _ray.direction * _dis - _colorPointSize / 2f;
            if (_colorPointPos.x <= _colorViewBounds.min.x)
            {
                _result.x = _colorViewBounds.min.x;
            }
            else if (_colorPointPos.x + _colorPointSize.x >= _colorViewBounds.max.x)
            {
                _result.x = _colorViewBounds.max.x - _colorPointSize.x;
            }
            else if (_colorPointPos.x >= _colorViewBounds.min.x && _colorPointPos.x + _colorPointSize.x <= _colorViewBounds.max.x)
            {
                _result.x = _colorPointPos.x;
            }

            if (_colorPointPos.y <= _colorViewBounds.min.y)
            {
                _result.y = _colorViewBounds.min.y;
            }
            else if (_colorPointPos.y + _colorPointSize.y >= _colorViewBounds.max.y)
            {
                _result.y = _colorViewBounds.max.y - _colorPointSize.y;
            }
            else if (_colorPointPos.y >= _colorViewBounds.min.y && _colorPointPos.y + _colorPointSize.y <= _colorViewBounds.max.y)
            {
                _result.y = _colorPointPos.y;
            }
            _result.z = colorPoint.position.z;
            colorPoint.position = _result;
        }
    }

    bool isRedSliderClicked;
    bool isGreenSliderClicked;
    bool isBlueSliderClicked;
    bool isToneSliderClicked;

    ///// <summary>
    ///// 
    ///// </summary>
    //Transform CheckBounds()
    //{
    //    Transform _result = null;
    //    Ray _ray = uiCamera.ScreenPointToRay(Input.mousePosition);
    //    if (colorView.collider.enabled && colorView.collider.bounds.IntersectRay(_ray))
    //    {
    //        isPressOnColorViewArea = true;
    //        return colorView.transform;
    //    }
    //    else
    //    {
    //        isPressOnColorViewArea = false;
    //    }
    //    if (redSlider.collider.bounds.IntersectRay(_ray) || redSlider.GetComponentsInChildren<BoxCollider>()[1].bounds.IntersectRay(_ray))
    //    {
    //        isRedSliderClicked = true;
    //        return redSlider.transform;
    //    }
    //    else
    //    {
    //        isRedSliderClicked = false;
    //    }
    //    if (greenSlider.collider.bounds.IntersectRay(_ray) || greenSlider.GetComponentsInChildren<BoxCollider>()[1].bounds.IntersectRay(_ray))
    //    {
    //        isGreenSliderClicked = true;
    //        return greenSlider.transform;
    //    }
    //    else
    //    {
    //        isGreenSliderClicked = false;
    //    }
    //    if (blueSlider.collider.bounds.IntersectRay(_ray) || blueSlider.GetComponentsInChildren<BoxCollider>()[1].bounds.IntersectRay(_ray))
    //    {
    //        isBlueSliderClicked = true;
    //        return blueSlider.transform;
    //    }
    //    else
    //    {
    //        isBlueSliderClicked = false;
    //    }
    //    if (toneSlider.collider.bounds.IntersectRay(_ray) || toneSlider.GetComponentsInChildren<BoxCollider>()[1].bounds.IntersectRay(_ray))
    //    {
    //        isToneSliderClicked = true;
    //        return toneSlider.transform;
    //    }
    //    else
    //    {
    //        isToneSliderClicked = false;
    //    }
    //    return _result;
    //}

    /// <summary>
    /// 
    /// </summary>
    void CheckBounds()
    {
        Ray _ray = uiCamera.ScreenPointToRay(Input.mousePosition);
        if (colorView.GetComponent<Collider>().enabled && colorView.GetComponent<Collider>().bounds.IntersectRay(_ray))
        {
            isPressOnColorViewArea = true;
        }
        else
        {
            isPressOnColorViewArea = false;
        }
        if (redSlider.GetComponent<Collider>().bounds.IntersectRay(_ray) || redSlider.GetComponentsInChildren<BoxCollider>()[1].bounds.IntersectRay(_ray))
        {
            isRedSliderClicked = true;
        }
        else
        {
            isRedSliderClicked = false;
        }
        if (greenSlider.GetComponent<Collider>().bounds.IntersectRay(_ray) || greenSlider.GetComponentsInChildren<BoxCollider>()[1].bounds.IntersectRay(_ray))
        {
            isGreenSliderClicked = true;
        }
        else
        {
            isGreenSliderClicked = false;
        }
        if (blueSlider.GetComponent<Collider>().bounds.IntersectRay(_ray) || blueSlider.GetComponentsInChildren<BoxCollider>()[1].bounds.IntersectRay(_ray))
        {
            isBlueSliderClicked = true;
        }
        else
        {
            isBlueSliderClicked = false;
        }
        if (toneSlider.GetComponent<Collider>().bounds.IntersectRay(_ray) || toneSlider.GetComponentsInChildren<BoxCollider>()[1].bounds.IntersectRay(_ray))
        {
            isToneSliderClicked = true;
        }
        else
        {
            isToneSliderClicked = false;
        }
    }

    /// <summary>
    /// �ı�RGBSlider��Valueֵ
    /// </summary>
    void ChangeSliderValue()
    {
        Vector3 _colorPointSize = colorPoint.GetComponent<Collider>().bounds.size;
        float _ratioY = (colorPoint.position.y - colorView.GetComponent<Collider>().bounds.min.y) / (colorView.GetComponent<Collider>().bounds.max.y - _colorPointSize.y - colorView.GetComponent<Collider>().bounds.min.y);
        float _ratioX = (colorView.GetComponent<Collider>().bounds.max.x - _colorPointSize.x - colorPoint.position.x) / (colorView.GetComponent<Collider>().bounds.max.x - _colorPointSize.x - colorView.GetComponent<Collider>().bounds.min.x);

        redSlider.sliderValue = curTone.r - curTone.r * (1f - _ratioY) + (1f - curTone.r) * _ratioX * _ratioY;
        greenSlider.sliderValue = curTone.g - curTone.g * (1f - _ratioY) + (1f - curTone.g) * _ratioX * _ratioY;
        blueSlider.sliderValue = curTone.b - curTone.b * (1f - _ratioY) + (1f - curTone.b) * _ratioX * _ratioY;
        resultColor.color = new Color(redSlider.sliderValue, greenSlider.sliderValue, blueSlider.sliderValue);
        UpdateInputText();
		UpdateColorEvent();
    }

    string msg;
    //void OnGUI()
    //{
    //    GUI.Label(new Rect(5f, 50f, Screen.width, Screen.height), msg);
    //}

    void RGBSliderValueChanged( )
    {
        if (isToneSliderClicked || isPressOnColorViewArea) return;

        Vector3 _hsv = Rgb2Hsv(redSlider.sliderValue, greenSlider.sliderValue, blueSlider.sliderValue);
        //Debug.Log("HSV : " + _hsv);
        if ((-1f * Vector3.one).Equals(_hsv))
        {
            bool b1 = redSlider.sliderValue == 1f && greenSlider.sliderValue == 1f && blueSlider.sliderValue == 1f;
            bool b2 = redSlider.sliderValue == 0f && greenSlider.sliderValue == 0f && blueSlider.sliderValue == 0f;
            if (b1 || b2)
            {
                if (b1)
                {
                    colorPoint.position = colorPointOriginalPos;
                }
                else if (b2)
                {
                    Vector3 _curLocPos = colorPoint.localPosition;
                    _curLocPos.y = 0f;
                    colorPoint.localPosition = _curLocPos;
                }
                toneSlider.sliderValue = 0f;
                colorView.color = Color.red;
                curTone = Color.red;
            }
        }
        else
        {
            //try
            //{
                Vector3 _colorPointSize = colorPoint.GetComponent<Collider>().bounds.size;
                //Debug.Log(value + "  " + _colorPointSize.x + ", " + _colorPointSize.y + ", " + _colorPointSize.z);
                Bounds _colorViewBounds = colorView.GetComponent<Collider>().bounds;
                //Debug.Log(_hsv.x / 360f);
				EventDelegate.Remove(toneSlider.onChange,ToneSliderValueChange);
                
                toneSlider.sliderValue = _hsv.x / 360f;
				EventDelegate.Add(toneSlider.onChange,ToneSliderValueChange);
              

                Vector3 _colorPointPos = colorPoint.position;
                _colorPointPos.x = _colorViewBounds.min.x + _hsv.y * (_colorViewBounds.max.x - _colorViewBounds.min.x - _colorPointSize.x);
                _colorPointPos.y = _colorViewBounds.min.y + _hsv.z * (_colorViewBounds.max.y - _colorViewBounds.min.y - _colorPointSize.y);
                colorPoint.position = _colorPointPos;
            //}
            //catch (Exception e)
            //{
            //    msg = (_hsv.x / 360f) + "   " + colorPoint.collider.bounds.size.x + ", " + colorPoint.collider.bounds.size.y + ", " + colorPoint.collider.bounds.size.z
            //            + "   " + e.Message + "   Data : " + e.Data + "   BaseException : " + e.GetBaseException() + "   Source : " + e.Source + "   InnerExce : " + e.InnerException
            //            + "   helpLink : " + e.HelpLink + "   targetSite : " + e.TargetSite + "   StackTrace : " + e.StackTrace;
            //}
            //colorView.color = new Color(redSlider.sliderValue, greenSlider.sliderValue, blueSlider.sliderValue);
        }

        resultColor.color = new Color(redSlider.sliderValue, greenSlider.sliderValue, blueSlider.sliderValue);
        UpdateInputText();
		UpdateColorEvent();
        //toneSlider.sliderValue = 
    }

    Vector3 Rgb2Hsv(float R, float G, float B)
    {
        // r,g,b values are from 0 to 1
        // h = [0,360], s = [0,1], v = [0,1]
        // if s == 0, then h = -1 (undefined)

        Vector3 _HSV = new Vector3();

        float min, max, delta;
        min = max = delta = 0f;
        float[] _values = new float[3] { R, G, B };
        min = Mathf.Min(_values);

        max = Mathf.Max(_values);

        _HSV.z = max; // v 

        delta = max - min; 

        if( max != 0f )
            _HSV.y = delta / max; // s 
        else
        {
            // r = g = b = 0 // s = 0, v is undefined
            return -1f * Vector3.one;
        }

        if (delta != 0f)
        {
            if (R == max)
                _HSV.x = (G - B) / delta;       // between yellow & magenta
            else if (G == max)
                _HSV.x = 2 + (B - R) / delta;   // between cyan & yellow
            else
                _HSV.x = 4 + (R - G) / delta;   // between magenta & cyan

            _HSV.x *= 60;                       // degrees 

            if (_HSV.x < 0)
                _HSV.x += 360;
            return _HSV;
        }
        else
        {
            return -1f * Vector3.one;
        }
    }

    void OKBtn_Click()
    {
        if (CommitColor != null)
        {
            ColorBoardEventArgs _eventArgs = new ColorBoardEventArgs();
            _eventArgs.submitColor = resultColor.color;
            CommitColor(this, _eventArgs);
        }
		setVisible(false);
    }
	
	public void setColorBoardInitColor(Color color)
	{
		mInitColor = color;
	}
	
	public void setVisible(bool visible)
	{
		NGUITools.SetActive(this.gameObject,visible);
		if (!visible)
		{
			if (CommitColor != null)
			{
				CommitColor = null;
			}
			if (CancelColor != null)
			{
				CancelColor = null;
			}
		}
		else{
			
		}
	}
	
	void CancelBtn_Click()
	{
		if (CancelColor != null)
        {
			EventArgs _eventArgs = new EventArgs();
            CancelColor(this,_eventArgs);
        }
		setVisible(false);
	}
	
	void UpdateColorEvent()
	{
	    if (CommitColor != null)
        {
            ColorBoardEventArgs _eventArgs = new ColorBoardEventArgs();
            _eventArgs.submitColor = resultColor.color;
            CommitColor(this, _eventArgs);
        }
	}
	

    void UpdateRedSliderValue()
    {
		string content = RedInput.label.text;
        int _value = -1;
        if (isNumeric(content, out _value))
        {
            redSlider.sliderValue = ((float)_value) / 255f;
        }
        else
        {
            redSlider.sliderValue = 0f;
            RedInput.text = "0";
        }
    }

    void UpdateGreenSliderValue()
    {
		string content = GreenInput.label.text;
        int _value = -1;
        if (isNumeric(content, out _value))
        {
            greenSlider.sliderValue = ((float)_value) / 255f;
        }
        else
        {
            greenSlider.sliderValue = 0f;
            GreenInput.text = "0";
        }
    }

    void UpdateBlueSliderValue()
    {
		string content = BlueInput.label.text;
        int _value = -1;
        if (isNumeric(content, out _value))
        {
            blueSlider.sliderValue = ((float)_value) / 255f;
        }
        else
        {
            blueSlider.sliderValue = 0f;
            BlueInput.text = "0";
        }
    }

    bool isNumeric(string str, out int result)
    {
        try
        {
            result = Convert.ToInt32(str);
            return true;
        }
        catch
        {
            result = -1;
            return false;
        }
    }

    void UpdateInputText()
    {
        RedInput.text = Mathf.RoundToInt(redSlider.sliderValue * 255).ToString();
        GreenInput.text = Mathf.RoundToInt(greenSlider.sliderValue * 255).ToString();
        BlueInput.text = Mathf.RoundToInt(blueSlider.sliderValue * 255).ToString();
    }
}

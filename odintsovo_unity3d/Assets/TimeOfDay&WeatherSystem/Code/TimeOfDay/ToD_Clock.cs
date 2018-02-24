using UnityEngine;
using System.Collections;

public class ToD_Clock : MonoBehaviour 
{
	public enum Axis
	{
		x,
		y,
		z
	};

	public ToD_Base clToDBase;
    public Transform tHourHand;
    public Transform tMinuteHand;
	public Axis axis;
	public GameObject pauseObj;
	bool _isPlay = true;


    // The number of degrees per hour
    private float fHoursToDegrees = 360.0f / 12.0f;
    // The number of degrees per minute
    private float fMinutesToDegrees = 360.0f / 60.0f;


	void Update() 
    {
        float fCurrentHour = 24 * clToDBase.Get_fCurrentTimeOfDay;
        float fCurrentMinute = 60 * (fCurrentHour - Mathf.Floor(fCurrentHour));

		if (axis == Axis.x)
		{
			tHourHand.localRotation = Quaternion.Euler(fCurrentHour * fHoursToDegrees, 0, 0);
			tMinuteHand.localRotation = Quaternion.Euler(fCurrentMinute * fMinutesToDegrees, 0, 0);
		}

		if (axis == Axis.y)
		{
			tHourHand.localRotation = Quaternion.Euler(0, fCurrentHour * fHoursToDegrees, 0);
			tMinuteHand.localRotation = Quaternion.Euler(0, fCurrentMinute * fMinutesToDegrees, 0);
		}

		if (axis == Axis.z)
		{
			tHourHand.localRotation = Quaternion.Euler(0, 0, -fCurrentHour * fHoursToDegrees);
			tMinuteHand.localRotation = Quaternion.Euler(0, 0, -fCurrentMinute * fMinutesToDegrees);
		}        
	}

	public void Pause()
	{
		_isPlay = !_isPlay;
		pauseObj.SetActive(!_isPlay);
		clToDBase.enabled = _isPlay;
	}
}


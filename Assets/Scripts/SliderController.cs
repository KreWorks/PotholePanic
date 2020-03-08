using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
	public Transform cam;

	public Slider slider;

	public void SetSolveTime(float solvetime)
	{
		slider.maxValue = solvetime;
		slider.value = 0;
	}

	public void SetProgressTime(float time)
	{
		slider.value = time;
	}

	// Avoid wierd behavior because our camera moves that is why the LATE update
	void LateUpdate()
	{
		//To look always at the camera
		transform.LookAt(transform.position + cam.forward);

	}
}

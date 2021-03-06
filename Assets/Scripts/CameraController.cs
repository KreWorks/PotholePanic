﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CameraMove { Horizontal, Vertical}

public class CameraController : MonoBehaviour
{
	public Camera mainCamera;
	public float moveSpeed = 10f;
	public float zoomSpeed = 5f;
	public float minZoomSize = 2f;
	public float maxZoomSize = 8f;

	public float screenBoundary;

	public Vector2 cameraMoveLimitMin = new Vector2(-10.0f, -15.0f);
	public Vector2 cameraMoveLimitMax = new Vector2(10.0f, 5.0f);

	float screenWidth;
	float screenHeight;

	void Start()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}

	void Update()
	{
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
		{
			MoveCameraInput();
		}
		
		if(Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
		}

		if(Input.mousePosition.x > screenWidth - screenBoundary || Input.mousePosition.x < 0 + screenBoundary || Input.mousePosition.y > screenHeight - screenBoundary || Input.mousePosition.y < 0 + screenBoundary)
		{
			PanCamera();
		}

	}

	void PanCamera()
	{
		float moveSpeedLocal = 0 *Time.deltaTime * moveSpeed / 3.0f;
		if (Input.mousePosition.x > screenWidth - screenBoundary)
		{
			//Move camera
			MoveCamera(GetDirection(CameraMove.Horizontal) * moveSpeedLocal);
		}

		if (Input.mousePosition.x < 0 + screenBoundary)
		{
			//Move camera
			MoveCamera(-1 * GetDirection(CameraMove.Horizontal) * moveSpeedLocal);
		}

		if (Input.mousePosition.y > screenHeight - screenBoundary)
		{
			MoveCamera(GetDirection(CameraMove.Vertical) * moveSpeedLocal);
		}

		if (Input.mousePosition.y < 0 + screenBoundary)
		{
			MoveCamera(-1 * GetDirection(CameraMove.Vertical) * moveSpeedLocal);
		}
	}

	void MoveCamera(Vector3 translateVector)
	{
		Vector3 position = this.transform.position + translateVector;
		if (position.x > cameraMoveLimitMin.x && position.x < cameraMoveLimitMax.x && position.z > cameraMoveLimitMin.y && position.z < cameraMoveLimitMax.y)
		{
			this.transform.position = this.transform.position + translateVector;
		}
	}

	void MoveCameraInput()
	{
		Vector3 move =  new Vector3(0, 0, 0);
		if (Input.GetAxis("Horizontal") != 0)
		{
			move += GetDirection(CameraMove.Horizontal, Input.GetAxisRaw("Horizontal")) * moveSpeed * Time.deltaTime;
		}

		if (Input.GetAxis("Vertical") != 0)
		{
			move += GetDirection(CameraMove.Vertical, Input.GetAxisRaw("Vertical")) * moveSpeed * Time.deltaTime;
		}

		MoveCamera(move);
	}

	void ZoomCamera(float zoomSize)
	{
		if (zoomSize > 0)
		{
			if (mainCamera.orthographicSize > minZoomSize)
			{
				mainCamera.orthographicSize -= zoomSpeed * Time.deltaTime;
			}
		}

		if (zoomSize < 0)
		{
			if (mainCamera.orthographicSize < maxZoomSize)
			{
				mainCamera.orthographicSize += zoomSpeed * Time.deltaTime;
			}
		}
	}
	//NOT WORKING WELL
	public void MoveCameraToSpecificPosition(Vector3 lookAt)
	{
		Vector3 translate = lookAt - new Vector3(0, 0, 0);
		
		this.transform.position = new Vector3(0, 10, -7.5f) + translate;
		mainCamera.orthographicSize = 3.0f;
	}

	Vector3 GetDirection(CameraMove type, float strength = 1.0f)
	{
		Vector3 direction = new Vector3(0, 0, 0);

		if (type == CameraMove.Horizontal)
		{
			direction = new Vector3(strength, 0, 0);
			direction = this.transform.rotation * direction;
		}
		if (type == CameraMove.Vertical)
		{
			direction = new Vector3(0, 0, strength);
			direction = (Quaternion.Euler(0, this.transform.eulerAngles.y, 0)) * direction;
		}

		return direction;
	}
}

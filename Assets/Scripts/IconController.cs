using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconController : MonoBehaviour
{
    // Avoid wierd behavior because our camera moves
    void LateUpdate()
    {
		transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
    }
}

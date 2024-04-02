using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class RotateHelper : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 1.0f;

    void Update()
    {
        Quaternion rotation = Quaternion.AngleAxis(Time.deltaTime * _rotationSpeed * 180.0f, Vector3.up);
        this.transform.rotation = rotation * transform.rotation;
    }
}

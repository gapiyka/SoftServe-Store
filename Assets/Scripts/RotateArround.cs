using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArround : MonoBehaviour
{
    [SerializeField] private Transform lookAtPos;

    void Update()
    {
        transform.LookAt(lookAtPos);
        transform.Translate(Vector3.right * Time.deltaTime);
    }
}

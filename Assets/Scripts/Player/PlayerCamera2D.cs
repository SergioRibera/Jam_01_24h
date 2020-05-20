using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera2D : MonoBehaviour
{
    [SerializeField]
    float smooth = 5f;
    public Vector3 offset;

    Transform player;

    public void Init(Transform _p)
    {
        player = _p;
    }

    internal void MoveCam()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth * Time.deltaTime);
    }
}
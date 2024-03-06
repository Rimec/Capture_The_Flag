using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Camera))]
public class MultipleTargetsCamera : MonoBehaviour
{
    public List<Transform> targets;

    private Camera cam;

    [Header("Movement")]
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 velocity;
    [SerializeField] float smoothTime;

    [Header("Zoom")]
    [SerializeField] float minZoom = 40f;
    [SerializeField] float maxZoom = 10f;
    [SerializeField] float zoomLimiter = 50f;

    Action onConnectionEvent;

    private void Awake() {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate() {
        if(targets == null || targets.Count == 0) return;
        Move();
        Zoom();
    }
    
    Vector3 GetTargetsCenterPoint(){
        if(targets.Count == 1) return targets[0].position;

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++){
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    float GetDistanceBetweenTargets(){
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++){
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    private void Move(){
        Vector3 newPos = GetTargetsCenterPoint() + offset;
        cam.transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    }

    private void Zoom(){
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetDistanceBetweenTargets()/ zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

}

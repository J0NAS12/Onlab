using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float interpolationDuration = 0.10f;
    private float extrapolationDuration = 0.10f;
    private float interpolationTime = 0f;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private Vector3 velocity;
    public int index;
    Rigidbody rb;
    void Awake() => rb = GetComponent<Rigidbody>();

    void FixedUpdate() {
        if(GameValues.roomPlayers[index].newData){
            GameValues.roomPlayers[index].newData = false;
            SetTargetPosition(GameValues.roomPlayers[index]);
        }
        interpolationTime += Time.deltaTime;
        if(interpolationTime < interpolationDuration){
            Interpolate();
        }
        else{
            Extrapolate();
        }
    }
    private void SetTargetPosition(PlayerData playerData)
    {
        targetPosition = playerData.position;
        targetRotation = playerData.rotation;
        interpolationTime = 0f;
        startPosition = transform.position;
        startRotation = transform.rotation;
        velocity = playerData.velocity;
    }

    private void Interpolate(){
        float t = Mathf.Clamp01(interpolationTime / interpolationDuration);
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
    }

    private void Extrapolate(){
        Vector3 extrapolatedPosition = transform.position + velocity * extrapolationDuration;
        transform.position = Vector3.Lerp(transform.position, extrapolatedPosition, Time.deltaTime / interpolationDuration);
    }
}

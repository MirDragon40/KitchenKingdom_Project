using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBoxCap : MonoBehaviour
{
    public Transform centerPoint; // 기준점
    public float moveAngle = -70f; // 이동 각도 (x축 기준)
    public float moveDuration = 1f; // 이동 시간

    private bool isMoving = false; // 이동 중인지 확인

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            StartCoroutine(MoveObject());
        }
    }

    private IEnumerator MoveObject()
    {
        isMoving = true;

        // 현재 위치 저장
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        // 목표 회전 계산
        Quaternion targetRotation = Quaternion.Euler(moveAngle, 0f, 0f) * startRotation;

        float elapsedTime = 0f;

        // 이동 시작
        while (elapsedTime < moveDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 목표 회전 설정
        transform.rotation = targetRotation;

        // 잠시 대기
        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0f;

        // 원래 회전으로 돌아오기
        while (elapsedTime < moveDuration)
        {
            transform.rotation = Quaternion.Slerp(targetRotation, startRotation, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 원래 회전 설정
        transform.rotation = startRotation;

        isMoving = false;
    }
}

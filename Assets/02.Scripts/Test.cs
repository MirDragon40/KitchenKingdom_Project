using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool IsCenter = false;

    // Start is called before the first frame update
    void Start()
    {
        var pivotOffset = new Vector3(0f, 0f, -0.6f);

        // 현재 위치를 기준으로 중간 위치 계산
        Vector3 pivotPosition = transform.position + pivotOffset;

        // 물체를 중간 위치로 이동
        if (IsCenter)
        {
            transform.position = pivotPosition;
        }
        // 플레이어가 쳐다보는 방향으로 회전 적용
        // 플레이어가 쳐다보는 방향으로 회전 각도 계산
        ///transform.rotation = Quaternion.Euler(0, 180, 0);
        transform.Rotate(new Vector3(0, 180, 0));

        // 다시 원래 위치로 오프셋 적용하여 이동
        if (IsCenter)
        {
            transform.position += pivotOffset;

        }
    }

 
}

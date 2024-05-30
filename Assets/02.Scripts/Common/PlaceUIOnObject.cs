using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceUIOnObject : MonoBehaviour
{
    public float FixedYPosition; // 고정할 y 위치 값입니다.

    void Start()
    {
        // 시작할 때 자식 오브젝트의 현재 y 위치를 고정된 위치로 설정합니다.
        FixedYPosition = transform.position.y;
    }

    void LateUpdate()
    {
        // 현재 자식 오브젝트의 월드 위치를 가져옵니다.
        Vector3 worldPosition = transform.position;

        // y 값을 고정된 값으로 설정합니다.
        worldPosition.y = FixedYPosition;

        // 자식 오브젝트의 위치를 업데이트합니다.
        transform.position = worldPosition;
    }
    


}

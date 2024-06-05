using UnityEngine;

public class Table : MonoBehaviour
{
    public bool isOnFire = false;
    public ParticleSystem fireParticles; // 불 파티클 시스템
    public float spreadDelay = 5f; // 불이 퍼지는 딜레이 시간
    public float rayDistance = 10f; // 레이의 거리

    private void Start()
    {
        if (fireParticles != null)
        {
            fireParticles.Stop();
        }
    }

    // 테이블에 불을 붙이는 함수
    public void Ignite()
    {
        if (!isOnFire)
        {
            isOnFire = true;
            Debug.Log($"{gameObject.name} is on fire!");

            // 불이 붙었을 때 파티클 재생
            if (fireParticles != null)
            {
                fireParticles.Play();
            }

            Invoke("SpreadFireToNearbyTables", spreadDelay);
        }
    }

    private void SpreadFireToNearbyTables()
    {
        RaycastHit hit;

        // 왼쪽으로 레이캐스트
        if (Physics.Raycast(transform.position, -transform.right, out hit, rayDistance))
        {
            Table leftTable = hit.collider.GetComponent<Table>();
            if (leftTable != null && !leftTable.isOnFire)
            {
                leftTable.Ignite();
            }
        }

        // 오른쪽으로 레이캐스트
        if (Physics.Raycast(transform.position, transform.right, out hit, rayDistance))
        {
            Table rightTable = hit.collider.GetComponent<Table>();
            if (rightTable != null && !rightTable.isOnFire)
            {
                rightTable.Ignite();
            }
        }
    }
}
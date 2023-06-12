using UnityEngine;
using AdvancedPeopleSystem;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;  // 생성할 NPC 프리팹
    public Transform[] waypoints; // NPC가 따라갈 웨이포인트 배열
    public int totalNumberOfNPCs; // 생성할 NPC의 총 수
    public float spawnInterval = 1f; // NPC 생성 간격 (초)

    private int spawnedNPCs = 0; // 생성된 NPC 수를 저장하는 변수

    private void Start()
    {
        InvokeRepeating("SpawnNPC", 0f, spawnInterval); // 시작 시 NPC 생성 반복 호출 시작
    }

    private void SpawnNPC()
    {
        if (spawnedNPCs >= totalNumberOfNPCs)
        {
            CancelInvoke("SpawnNPC"); // 원하는 NPC 수가 생성되면 NPC 생성 반복 호출 중지
            return;
        }

        GameObject npc = Instantiate(npcPrefab, transform.position, Quaternion.identity); // NPC 생성
        npc.GetComponent<CharacterCustomization>().Randomize();
        npc.GetComponent<NPCMovement>().InitializeMovement(waypoints); // NPCMovement 컴포넌트 초기화

        spawnedNPCs++; // 생성된 NPC 수 증가
    }
}

using UnityEngine;
using AdvancedPeopleSystem;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // 생성할 NPC 프리팹
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
            CancelInvoke("SpawnNPC"); // 원하는 수의 NPC가 생성되면 NPC 생성 반복 호출 중단
            return;
        }

        var spawnPos = waypoints[Random.Range(0, waypoints.Length)];

        GameObject npc = Instantiate(npcPrefab, spawnPos.position, Quaternion.identity); // NPC 생성

        CharacterCustomization characterCustomization = npc.GetComponent<CharacterCustomization>();
        characterCustomization.SwitchCharacterSettings(Random.Range(0, 2) == 0 ? "Male" : "Female"); // 랜덤으로 성별 선택
        characterCustomization.Randomize(); // 외모 랜덤화

        NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
        npcMovement.InitializeMovement(waypoints); // NPCMovement 컴포넌트 초기화

        spawnedNPCs++; // 생성된 NPC 수 증가
    }
}

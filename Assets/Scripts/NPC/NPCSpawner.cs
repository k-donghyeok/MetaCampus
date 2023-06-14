using UnityEngine;
using AdvancedPeopleSystem;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // ������ NPC ������
    public Transform[] waypoints; // NPC�� ���� ��������Ʈ �迭
    public int totalNumberOfNPCs; // ������ NPC�� �� ��
    public float spawnInterval = 1f; // NPC ���� ���� (��)

    private int spawnedNPCs = 0; // ������ NPC ���� �����ϴ� ����

    private void Start()
    {
        InvokeRepeating("SpawnNPC", 0f, spawnInterval); // ���� �� NPC ���� �ݺ� ȣ�� ����
    }

    private void SpawnNPC()
    {
        if (spawnedNPCs >= totalNumberOfNPCs)
        {
            CancelInvoke("SpawnNPC"); // ���ϴ� ���� NPC�� �����Ǹ� NPC ���� �ݺ� ȣ�� �ߴ�
            return;
        }

        var spawnPos = waypoints[Random.Range(0, waypoints.Length)];

        GameObject npc = Instantiate(npcPrefab, spawnPos.position, Quaternion.identity); // NPC ����

        CharacterCustomization characterCustomization = npc.GetComponent<CharacterCustomization>();
        characterCustomization.SwitchCharacterSettings(Random.Range(0, 2) == 0 ? "Male" : "Female"); // �������� ���� ����
        characterCustomization.Randomize(); // �ܸ� ����ȭ

        NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
        npcMovement.InitializeMovement(waypoints); // NPCMovement ������Ʈ �ʱ�ȭ

        spawnedNPCs++; // ������ NPC �� ����
    }
}

using UnityEngine;
using AdvancedPeopleSystem;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;  // ������ NPC ������
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
            CancelInvoke("SpawnNPC"); // ���ϴ� NPC ���� �����Ǹ� NPC ���� �ݺ� ȣ�� ����
            return;
        }

        GameObject npc = Instantiate(npcPrefab, transform.position, Quaternion.identity); // NPC ����
        npc.GetComponent<CharacterCustomization>().Randomize();
        npc.GetComponent<NPCMovement>().InitializeMovement(waypoints); // NPCMovement ������Ʈ �ʱ�ȭ

        spawnedNPCs++; // ������ NPC �� ����
    }
}

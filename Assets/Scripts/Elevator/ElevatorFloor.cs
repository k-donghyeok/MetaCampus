using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ElevatorFloor : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtStatus = null;

    [SerializeField]
    private TMP_Text txtName = null;

    private ElevatorController owner = null;
    private int index = -1;

    private Animator animator = null;

    public void SetOwner(ElevatorController owner)
        => this.owner = owner;

    /// <summary>
    /// 기본 데이터 입력
    /// </summary>
    public void Initiate(ElevatorController owner,
        int index, string name)
    {
        this.owner = owner;
        this.index = index;
        txtName.text = name;
        animator = GetComponent<Animator>();

        this.owner.OnStatusUpdate += (status) => txtStatus.text = status;
    }

    public void CallElevator()
    {
        owner.RequestMoveToFloor(index, true);
    }

    public void UpdateAnim(float open)
    {
        animator.SetFloat(nameof(open), open);
    }
}

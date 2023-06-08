using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElevatorButtonFloor : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtName;

    private ElevatorController owner;
    private int index;

    public void Initiate(ElevatorController owner, int index, string name)
    {
        this.owner = owner;
        this.index = index;
        txtName.text = name;
    }

    public void OnPressed()
    {
        owner.RequestMoveToFloor(index, false);
    }
}

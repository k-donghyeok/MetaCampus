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

    [SerializeField]
    private AudioClip buttonPressSound;

    public void Initiate(ElevatorController owner, int index, string name)
    {
        this.owner = owner;
        this.index = index;
        txtName.text = name;
    }

    public void OnPressed()
    {
        AudioSource.PlayClipAtPoint(buttonPressSound, transform.position);
        owner.RequestMoveToFloor(index, false);
    }
}

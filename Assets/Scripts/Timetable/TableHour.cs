using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableHour : MonoBehaviour
{
    private TMP_Text text;

    private int hour = 1;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }
    
    public void UpdateText(string text)
    {
        this.text.text = text;
        hour = 1;
        // ũ�� ����
    }

    public void AddSize()
    {
        hour++;
        // ������ ũ�⸦ �Ʒ��� �ø���
    }
}

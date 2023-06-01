using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableHour : MonoBehaviour
{
    private TMP_Text text;

    public float hourWidth = 326f;
    public float hourHeight = 120f;

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
        Image image = GetComponentInChildren<Image>();
        RectTransform rectTransform = image.gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(hourWidth, hourHeight);
    }

    public void AddSize()
    {
        hour++;
        // ������ ũ�⸦ �Ʒ��� �ø���
        Image image = GetComponentInChildren<Image>();
        RectTransform rectTransform = image.gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(hourWidth, hourHeight * hour);
        rectTransform.localPosition += (hourHeight * 0.5f) * Vector3.down;

        rectTransform = text.gameObject.GetComponent<RectTransform>();
        rectTransform.localPosition += (hourHeight * 0.5f) * Vector3.down;
    }
}

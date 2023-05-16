using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableHour : MonoBehaviour
{
    private TMP_Text text;

    public float hourWidth = 200f;
    public float hourHeight = 100f;

    private int hour = 1;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }
    
    public void UpdateText(string text)
    {
        this.text.text = text;
        hour = 1;
        // 크기 리셋
        Image image = GetComponentInChildren<Image>();
        RectTransform rectTransform = image.gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(hourWidth, hourHeight);
    }

    public void AddSize()
    {
        hour++;
        // 실제로 크기를 아래로 늘리기
        Image image = GetComponentInChildren<Image>();
        RectTransform rectTransform = image.gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(hourWidth, hourHeight * hour);
        rectTransform.position += (hourHeight * 0.5f) * Vector3.down;

        rectTransform = text.gameObject.GetComponent<RectTransform>();
        rectTransform.position += (hourHeight * 0.5f) * Vector3.down;
    }
}

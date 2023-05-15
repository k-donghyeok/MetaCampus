using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Timetable : MonoBehaviour
{
    // CSV 파일 이름을 저장할 문자열 변수와 CSV 파일 데이터를 저장할 리스트 변수를 선언
    public string csvFileName = "TimetableTest";
    private List<Dictionary<string, object>> csvData;

    [SerializeField]
    private GameObject prefabTableHour = null;

    void Start()
    {
        LoadCSVData();
        string[,] timetableData = ConvertDataTo2DArray();

        for (int d = 0; d < 5; d++)
        {
            string lastHour = string.Empty;
            TableHour lastScript = null;
            for (int h = 0; h < 8; h++)
            {
                if (string.IsNullOrEmpty(timetableData[h, d]))
                {
                    lastHour = string.Empty;
                }
                else
                {
                    if (lastHour == timetableData[h, d])
                    {
                        // 이전 TableHour를 늘린다
                        lastScript.AddSize();
                    }
                    else
                    {
                        lastHour = timetableData[h, d];
                        //새 TableHour를 만든다
                        var go = Instantiate(prefabTableHour, transform);
                        lastScript = go.GetComponent<TableHour>();
                        lastScript.UpdateText(timetableData[h, d]);

                        (go.transform as RectTransform).position
                            = new Vector3((2 - d) * lastScript.hourWidth, (3 - h) * lastScript.hourHeight, 900f);
                    }
                }
            }
        }


    }

    private string[,] ConvertDataTo2DArray()
    {

        // 시간표 데이터를 저장할 2차원 문자열 배열을 선언
        string[,] timetableData = new string[8, 5];

        // csvData 리스트 변수를 반복하여 시간표 데이터를 timetableData 배열에 저장
        foreach (var row in csvData)
        {
            // "교시" 컬럼 값을 period 변수에 저장
            int period = (int)row["교시"];
            for (int i = 1; i <= 5; i++)
            {
                // i 값에 따라 "월요일", "화요일" 등의 문자열을 day 변수에 저장
                string day = GetDayOfWeek(i);
                // day 변수에 해당하는 과목명을 className 변수에 저장
                string className = (string)row[day];
                // timetableData 배열의 해당 위치에 className 값을 저장
                timetableData[period - 1, i - 1] = className;
            }
        }

        return timetableData;
    }

    private void LoadCSVData()
    {
        // CSVReader 스크립트를 사용하여 CSV 파일을 읽어옴
        csvData = CSVReader.Read(csvFileName);

        // csvData 리스트 변수를 "교시" 컬럼 값을 기준으로 오름차순으로 정렬
        csvData = csvData.OrderBy(row => (int)row["교시"]).ToList();
    }

    // 요일을 나타내는 정수 값을 문자열로 변환하는 GetDayOfWeek 함수를 정의
    private string GetDayOfWeek(int day)
    {
        switch (day)
        {
            case 1:
                return "월요일";
            case 2:
                return "화요일";
            case 3:
                return "수요일";
            case 4:
                return "목요일";
            case 5:
                return "금요일";
            default:
                return "";
        }
    }
}

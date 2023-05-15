using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Timetable : MonoBehaviour
{
    // CSV 파일 이름을 저장할 문자열 변수와 CSV 파일 데이터를 저장할 리스트 변수를 선언
    public string csvFileName = "TimetableTest";
    private List<Dictionary<string, object>> csvData;

    // 시간표를 출력할 UI 텍스트를 저장할 변수를 선언
    public Text timetableText;

    void Start()
    {
        // CSVReader 스크립트를 사용하여 CSV 파일을 읽어옴
        csvData = CSVReader.Read(csvFileName);

        // csvData 리스트 변수를 "교시" 컬럼 값을 기준으로 오름차순으로 정렬
        csvData = csvData.OrderBy(row => (int)row["교시"]).ToList();

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

        // 시간표 데이터를 문자열 형태로 저장할 timetableString 변수를 선언
        string timetableString = "";
        // timetableData 배열의 값을 하나씩 문자열에 추가
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                // timetableData 배열의 값과 일정한 간격을 두고 timetableString에 추가
                timetableString += timetableData[i, j] + "         ";
            }
            // 한 줄을 출력한 뒤 다음 줄로 이동
            timetableString += "\n";
        }

        // timetableText UI 텍스트의 값을 timetableString으로 설정
        timetableText.text = timetableString;
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

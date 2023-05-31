<?php

// 데이터베이스에 접근하는 정보들을 가진 변수들
$servername = "localhost"; // 서버 주소
$username = "root"; // 데이터베이스 접근 아이디
$password = "asdf1234"; // SQL 접근 비밀번호
$dbname = "db_score"; // 데이터베이스 이름

$id = $_POST["id"]; 
$score = $_POST["score"];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("데이터베이스 연결 오류: " . $conn->connect_error);
}

$sqli = "SELECT * FROM tb_score WHERE id = '".$id."'"; // 특정 ID의 점수를 가져오는 쿼리
$result = $conn->query($sqli);

if (!$result) {
    die("쿼리 실행 오류: " . $conn->error);
}

if ($result->num_rows > 0) {
    $row = $result->fetch_assoc();
    if ($row['score'] < $score) { // 현재 저장된 점수보다 더 높을 때
        $update_sql = "UPDATE tb_score SET score = '".$score."' WHERE id = '".$id."'"; // 점수를 업데이트하는 쿼리
        $conn->query($update_sql);
        echo "0"; // 성공
    } else {
        echo "1"; // 이미 더 높은 점수가 있는 경우
    }
} else {
    $insert_sql = "INSERT INTO tb_score (id, score) VALUES ('".$id."', '".$score."')"; // 새로운 사용자의 점수를 추가하는 쿼리
    $conn->query($insert_sql);
    echo "2"; // 새로운 사용자의 점수 추가
}

$conn->close();
?>

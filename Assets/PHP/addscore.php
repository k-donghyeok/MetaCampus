<?php

// �����ͺ��̽��� �����ϴ� �������� ���� ������
$servername = "localhost"; // ���� �ּ�
$username = "root"; // �����ͺ��̽� ���� ���̵�
$password = "asdf1234"; // SQL ���� ��й�ȣ
$dbname = "db_score"; // �����ͺ��̽� �̸�

$id = $_POST["id"]; 
$score = $_POST["score"];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("�����ͺ��̽� ���� ����: " . $conn->connect_error);
}

$sqli = "SELECT * FROM tb_score WHERE id = '".$id."'"; // Ư�� ID�� ������ �������� ����
$result = $conn->query($sqli);

if (!$result) {
    die("���� ���� ����: " . $conn->error);
}

if ($result->num_rows > 0) {
    $row = $result->fetch_assoc();
    if ($row['score'] < $score) { // ���� ����� �������� �� ���� ��
        $update_sql = "UPDATE tb_score SET score = '".$score."' WHERE id = '".$id."'"; // ������ ������Ʈ�ϴ� ����
        $conn->query($update_sql);
        echo "0"; // ����
    } else {
        echo "1"; // �̹� �� ���� ������ �ִ� ���
    }
} else {
    $insert_sql = "INSERT INTO tb_score (id, score) VALUES ('".$id."', '".$score."')"; // ���ο� ������� ������ �߰��ϴ� ����
    $conn->query($insert_sql);
    echo "2"; // ���ο� ������� ���� �߰�
}

$conn->close();
?>

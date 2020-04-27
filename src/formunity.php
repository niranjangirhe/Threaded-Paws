<?php
    

// $post_data = (
//     'X' =>  $_POST["X"]
//     ,    
//     'userID' => $_POST["userID"],
//     'levelNo' => $_POST["levelNo"],
//     'isLevelCleared' => $_POST["isLevelCleared"],
//     'isLevelSteps' => $_POST["isLevelSteps"],
//     'levelClearedTime' => $_POST["levelClearedTime"],
//     'levelClearAmount' => $_POST["levelClearAmount"],
//     'failedAttempts' => $_POST["failedAttempts"],
//     'infoButtonCount' => $_POST["infoButtonCount"],
//     'agendaButtonCount' => $_POST["agendaButtonCount"]
    
// );
$post_data =( $_POST["log"]);


//$myJSON = stripslashes(json_encode($post_data));

if($_POST["log"]!="")
{
    echo("Message Successfully send");
    // echo("Field 1: ". $text1);
    // echo("Field 2: ". $text3);
    // echo("Field 3: ". $text3);

    $file= fopen("LogFile.json","a");

    fwrite($file, $post_data);
    // fwrite($file, $text2);
    // fwrite($file, $text3);
    fclose($file);

}else
{
    echo("Message delivery failed...");
}


?>
<?php
//header('Access-Control-Allow-Origin: http://www.baidu.com'); //设置http://www.baidu.com允许跨域访问
//header('Access-Control-Allow-Headers: X-Requested-With,X_Requested_With'); //设置允许的跨域header
date_default_timezone_set("Asia/chongqing");
error_reporting(E_ERROR);
header("Content-Type: text/html; charset=utf-8");

$CONFIG = json_decode(preg_replace("/\/\*[\s\S]+?\*\//", "", file_get_contents("config.json")), true);
$action = $_GET['action'];

// array (
//   '__token__' => 
//   array (
//     '539016f9bc96f393f95ebde1737f32d5' => 'c9d0ee61f6f24c8471dde8887a39e7ed',
//   ),
//   'admin' => 
//   array (
//     'user_id' => '1',
//     'type' => '1',
//     'username' => 'admin',
//     'password' => 'q8x8iC/B',
//     'create_time' => '2014-05-17 14:03:29',
//     'login_time' => '2015-07-24 20:40:36',
//     'login_ip' => '127.0.0.1',
//     'user_status' => '1',
//     'avatar' => 'avatar-2',
//     'gender' => '1',
//     'is_login' => true,
//     'superadmin' => true,
//     'logs_start_time' => '0.98969000 1437975646',
//   ),
// )

session_start();
$admin = $_SESSION['admin'];
if (!($admin && $admin['user_id'] && $admin['is_login'])) {
    exit(json_encode(array('state' => '请登录后再试！')));
}//if

switch ($action) {
    case 'config':
        $result = json_encode($CONFIG);
        break;

    /* 上传图片 */
    case 'uploadimage':
    /* 上传涂鸦 */
    case 'uploadscrawl':
    /* 上传视频 */
    case 'uploadvideo':
    /* 上传文件 */
    case 'uploadfile':
        $result = include("action_upload.php");
        break;

    /* 列出图片 */
    case 'listimage':
        $result = include("action_list.php");
        break;
    /* 列出文件 */
    case 'listfile':
        $result = include("action_list.php");
        break;

    /* 抓取远程文件 */
    case 'catchimage':
        $result = include("action_crawler.php");
        break;

    default:
        $result = json_encode(array(
            'state'=> '请求地址出错'
        ));
        break;
}//switch

/* 输出结果 */
if (isset($_GET["callback"])) {
    if (preg_match("/^[\w_]+$/", $_GET["callback"])) {
        echo htmlspecialchars($_GET["callback"]) . '(' . $result . ')';
    } else {
        echo json_encode(array(
            'state' => 'callback参数不合法'
        ));
    }//if
} else {
    echo $result;
}//if
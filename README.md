# 留言板系統(MVC架構)

**作品名稱:留言板系統(MVC架構)**

## 描述:
使用API、sql sever資料庫、Postman工具和MVC架構寫的程式。


## 功能:


## 畫面:

## 工具:

Visual Studio 2022

Postman

SQL Sever (資料庫)

## 資料夾說明:

#### 說明文件 :

　　/Postman的API/API留言板.postman_collection2.json : Postman的API和API簡單文件說明

　　/指令/SQL/select.sql : 展示全部留言資料

　　/指令/SQL/創建comments資料表.sql : 創建資料表

　　/指令/SQL/插入資料.sql : 插入三筆資料

　　/筆記/筆記.txt: 筆記

#### messageboradAPI:

為主程式所在位置。

#### /Controllers:
　　/APICommentsController.cs : API程式碼所在

　　/CommentsController.cs : View中Comments的控制器，會操控API做事
  
　　/HomeController.cs : 範本的控制器
  
　　
#### /Models:
　　/Data/CommentContext.cs : 連接資料庫
  
　　/Comment.cs : 留言類別
  
　　/DataReaderExtensions2.cs : 類別中有MapToCommentsAsync函數，用來提升代碼利用率(似乎沒有重複代碼)，功能: 資料存入comments
  

#### 資料庫結構ERD:



#### /Views:

　　/Comments/Edit.cshtml : 編輯留言的UI
  
　　/Comments/Index.cshtml : 範本的UI


Program.cs : (寫入API相關程式)

appsettings.json : (資料庫格式)

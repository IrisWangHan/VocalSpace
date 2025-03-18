//此JS提供留言partial view的功能

//提交留言
$(document).on("click", "#submit-comment", function () {
    var comment = $("#comment-input").val().trim();
    if (comment === "") {
        alert("請輸入留言內容！");
        return;
    }

    $.ajax({
        url: "/Global/Comment",
        type: "POST",
        data: JSON.stringify({
            "Comment": comment
        }),
        contentType: "application/json",
        success: function (response) {
            if (response === "success") {
                $("#comment-input").val("");  // 清空輸入框
                loadComments();  // 重新載入留言列表
            } else {
                alert("留言失敗！");
            }
        }
    });
});
//載入留言 
function loadComments(songId) {
    $.ajax({
        url: "/Song/GetComments?songId=" + songId,  // API URL
        type: "GET",
        success: function (data) {
            $("#comment-list").html(data);  // 將返回的 HTML 插入到 comment-list 中
        },
        error: function (error) {
            console.log("Error loading comments:", error);  // 輸出錯誤信息
        }
    });
}

// 當頁面載入時，自動載入留言
$(function () {
    var songId = $("#song-id").val();
    loadComments(songId);
});
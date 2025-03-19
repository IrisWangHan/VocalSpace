//此JS提供留言partial view的功能
//確認按鈕id為submit-comment，留言輸入框id為comment-input

$(function () {
    console.log("✅ Comment.js 已載入！");
    $(document).on("click", "#submit-comment", submitComment);
    loadComments(); // 頁面加載時載入留言
});

// 送出留言
function submitComment() {
    let comment = $("#comment-input").val().trim();
    if (comment === "") {
        alert("請輸入留言內容！");
        return;
    }

    $.ajax({
        url: "/Song/PostComment", // 連接後端路由
        type: "POST",
        data: JSON.stringify({
            TargetId: $("#comment-section").data("target-id"),
            TargetType: $("#comment-section").data("target-type"), // "Song"
            Comment: comment
        }),
        contentType: "application/json",
        success: function (response) {
            if (response.success) {
                $("#comment-input").val("");  // 清空輸入框
                loadComments();  // 重新載入留言列表
            } else {
                alert("留言失敗：" + response.message);
            }
        },
        error: function (xhr) {
            console.error("AJAX 錯誤：", xhr.responseText);
            alert("留言失敗，請稍後再試！");
        }
    });
}

//載入留言
function loadComments() {
    let targetId = $("#comment-section").data("target-id");

    $.ajax({
        url: `/Song/${targetId}/Comments`,
        type: "GET", // ❗這裡要明確寫 "GET"
        success: function (partialView) {
            $("#comment-list-container").html(partialView);
        },
        error: function () {
            $("#comment-list-container").html("<p class='error-message'>留言載入失敗，請稍後再試。</p>");
        }
    });
}

// 刪除留言
$(document).on("click", ".delete-comment", function () {
    let commentId = $(this).data("comment-id");

    if (!confirm("確定要刪除這則留言嗎？")) return;

    $.ajax({
        url: `/Song/DeleteComment/${commentId}`,
        type: "DELETE",
        success: function (response) {
            if (response.success) {
                alert("留言已刪除！");
                loadComments(); // 重新載入留言列表
            } else {
                alert("刪除失敗：" + response.message);
            }
        },
        error: function (xhr) {
            console.error("AJAX 錯誤：", xhr.responseText);
            alert("留言失敗，請稍後再試！");
        }
    });
});

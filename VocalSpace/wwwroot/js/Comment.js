//此JS提供留言partial view的功能
//確認按鈕id為submit-comment，留言輸入框id為comment-input

$(function () {
    $(document).on("click", "#submit-comment", submitComment);
    console.log("Comment.js已載入")
    loadComments(); // 頁面加載時載入留言
});

// 送出留言
//先判斷留言是否為空
function submitComment() {
    let comment = $("#comment-input").val().trim();
    if (comment === "") {
        alert("請輸入留言內容！");
        return;
    }

    let targetType = $("#comment-section").data("target-type"); // "Song" 或 "Activity"
    let postUrl = getPostCommentUrl(targetType); // 使用動態方法取得 URL

    $.ajax({
        url: postUrl,
        type: "POST",
        data: JSON.stringify({
            TargetId: $("#comment-section").data("target-id"),
            TargetType: targetType,
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

//載入留言(AJAX動態載入)
function loadComments() {
    let targetId = $("#comment-section").data("target-id");
    let targetType = $("#comment-section").data("target-type"); // "Song" 或 "Activity"
    let loadUrl = getLoadCommentsUrl(targetType, targetId); // 使用動態方法取得 URL

    $.ajax({
        url: loadUrl,
        type: "GET",
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
    let targetType = $("#comment-section").data("target-type"); // "Song" 或 "Activity"
    let deleteUrl = getDeleteCommentUrl(targetType, commentId); // 使用動態方法取得 URL

    if (!confirm("確定要刪除這則留言嗎？")) return;

    $.ajax({
        url: deleteUrl,
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

// 根據留言類型，選擇相應的API路徑
function getPostCommentUrl(targetType) {
    switch (targetType) {
        case "Song":
            return "/Song/PostComment";
        case "Activity":
            return "/Activity/PostComment"; // 當留言類型為 "Activity"
        default:
            throw new Error("不支援的留言類型！");
    }
}

// 根據留言類型和TargetId選擇相應的API路徑載入留言
function getLoadCommentsUrl(targetType, targetId) {
    switch (targetType) {
        case "Song":
            return `/Song/${targetId}/Comments`;
        case "Activity":
            return `/Activity/${targetId}/Comments`; // 當留言類型為 "Activity"
        default:
            throw new Error("不支援的留言類型！");
    }
}

// 根據留言類型，選擇相應的API路徑刪除留言
function getDeleteCommentUrl(targetType, commentId) {
    switch (targetType) {
        case "Song":
            return `/Song/DeleteComment/${commentId}`;
        case "Activity":
            return `/Activity/DeleteComment/${commentId}`; // 當留言類型為 "Activity"
        default:
            throw new Error("不支援的留言類型！");
    }
}

//此JS提供留言partial view的功能

//載入留言
$(function () {
    loadComments();
});

function loadComments() {
    let targetId = $("#comment-section").data("target-id");

    $.get(`/Song/${targetId}/Comments`, function (partialView) {
        $("#comment-list-container").html(partialView);
    }).fail(function () {
        $("#comment-list-container").html("<p class='error-message'>留言載入失敗，請稍後再試。</p>");
    });
}

// 送出留言
function submitComment() {
    var comment = $("#comment-input").val().trim();
    if (comment === "") {
        alert("請輸入留言內容！");
        return;
    }

    $.ajax({
        url: "/Global/Comment", // Controller 端點
        type: "POST",
        data: JSON.stringify({
            TargetId: $("#comment-section").data("target-id"),
            TargetType: $("#comment-section").data("target-type"), // "Song" or "Activity"
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
        }
    });
}

//提交留言

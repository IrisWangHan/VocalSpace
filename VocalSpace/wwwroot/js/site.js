//AJAX 取得UserBar
$(function () {
    let userId = $("#userBarContainer").data("userid");
    $.ajax({
        url: `/Personal/GetUserBar/${userId}`, // Controller 端點，
        type: "GET",
        success: function (partialView) {
            $("#userBarContainer").html(partialView);
        },
        error: function (xhr) {
            $("#userBarContainer").html(`<p class='error-message'>上傳者載入失敗，錯誤訊息${xhr.responseText}</p>`);
        }
    });
});


//用戶追蹤功能，btn-follow是按鈕的class，需先在View中設定data-userid
$(document).on("click", ".btn-follow", function () {
    let button = $(this);
    let targetUserId = $(this).data("userid");
    $.ajax({
        url: `/Personal/ToggleFollow/${targetUserId}`,
        type: "POST",
        success: function (res) {
            if (res.isFollowing) {
                button.addClass("selected").text("已追蹤");
            } else {
                button.removeClass("selected").text("追蹤");
            }
        },
        error: function (xhr) {
            if (xhr.status === 400)
                alert(xhr.responseJSON?.message || "無法追蹤自己");
            else
                alert(xhr.responseJSON?.message || "發生錯誤，請稍後再試！");
        }
    });
});

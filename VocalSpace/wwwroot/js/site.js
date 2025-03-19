//追蹤用戶AJAX POST
$(function () {
    let $btn = $(this);
    let userId = $btn.data("userid");

    //$.post("/Personal/ToggleFollow", { followedUserId: userId }, function (response) {
    //    if (response.success) {
    //        if (response.isFollowing) {
    //            $btn.text("已追蹤").removeClass("btn-danger").addClass("btn-secondary");
    //        } else {
    //            $btn.text("追蹤").removeClass("btn-secondary").addClass("btn-danger");
    //        }
    //    } else {
    //        alert(response.message);
    //    }
    //}).fail(function () {
    //    alert("操作失敗，請稍後再試！");
    //});

});

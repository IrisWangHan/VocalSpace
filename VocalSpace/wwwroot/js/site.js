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
//加入歌單彈窗(AJAX動態載入)
$(document).on("click", ".btn-add-to-playlist", function () {
    let songId = $(this).data("songid"); // 取得歌曲 ID

    $.ajax({
        url: `/Song/GetAddToPlaylistModal/${songId}`,
        type: "GET",
        success: function (modal) {
            // 把載入的 Modal HTML 插入到頁面中
            $("#modalAJAXContainer").html(modal);
            $("#Modal1").modal("show"); // 顯示 Modal
        },
        error: function (xhr) {
            if (xhr.status === 401) {
                alert("請先登入");
            } else {
                alert("發生錯誤，請稍後再試！");
            }
        }
    });
});
//加入歌單功能
$(document).on("click", ".addbtn-add", function () {
    let button = $(this);
    let playlistId = button.data("playlist-id");
    let songId = button.data("song-id");

    $.ajax({
        url: `/Song/playlist/add-song`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ PlayListID: playlistId, SongID: songId }),
        success: function (res) {
            if (res.isAdded) {
                button.addClass("selected").text("✔ 已加入");
            } else {
                button.removeClass("selected").text("+ 加入歌單");
            }
        },
        error: function (xhr) {
            alert(xhr.responseJSON?.message || "發生錯誤，請稍後再試！");
        }
    });
});

//分享歌曲彈窗(AJAX動態載入)
$(document).on("click", ".btn-share-song", function () {
    let songId = $(this).data("songid"); // 取得歌曲 ID

    $.ajax({
        url: `/Song/GetShareModalData/${songId}`,
        type: "GET",
        success: function (modal) {
            // 把載入的 Modal HTML 插入到頁面中
            $("#modalAJAXContainer").html(modal);
            $("#Modal2").modal("show");
        },
        error: function (xhr) {
            alert("發生錯誤，請稍後再試！");
        }
    });
});
//分享歌曲彈窗-按鈕
function sharecopyLink() {
    let shareInput = $(".shareUrl");
    let shareText = shareInput.val();
    let shareName = $(".shareName").val();

    navigator.clipboard.writeText(shareText).then(() => {
        alert(`歌曲連結已複製！快分享給朋友一起聽《${shareName}》吧！`);
    }).catch(err => {
        console.error("複製失敗:", err);
        alert("複製失敗，請手動複製連結！");
    });
}
function shareOnFacebook() {
    let shareUrl = $(".shareUrl").val(); // 取得歌曲連結
    let fbUrl = `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(shareUrl)}`;
    window.open(fbUrl, "_blank");
}
function shareOnInstagram() {
    let shareUrl = $(".shareUrl").val(); // 取得歌曲連結
    let fbUrl = `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(shareUrl)}`;
    window.open(fbUrl, "_blank");
}

//歌曲按讚功能
$(document).on("click", ".btn-add-to-Likesong", function () {
    let button = $(this);
    let songId = button.data("songid");
    // 根據 songId 找到對應的 likeCount
    let count = $(`.likeCount[data-songid="${songId}"]`);
    $.ajax({
        url: `/Song/AddLikeSong`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ SongID: songId }),
        success: function (res) {
            if (res.isliked) {
                button.addClass("selected");
            }
            else {
                button.removeClass("selected");
            }
            // **只有當 count 存在時才更新讚數**
            if (count.length > 0) {
                count.text(res.likeCount);
            }
        },
        error: function (xhr) {
            alert(xhr.responseJSON?.message || "發生錯誤，請稍後再試！");
        }
    });
});

//歌曲播放功能(尚未開發，僅顯示提示訊息)
$(document).on("click", ".btn-play", function () {
    let songName = $(this).data("songname");  // 取得歌曲名稱
    alert(`現正播放：${songName}`);
});


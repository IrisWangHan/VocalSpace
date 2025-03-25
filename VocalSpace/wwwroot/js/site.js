//未登入跳轉，登入後導回
function nologin() {
    let currentUrl = window.location.href;
    let returnUrl = encodeURIComponent(currentUrl);
    window.location.href = `/Accounts/Login?returnUrl=${returnUrl}`;
}

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
            else if (xhr.status === 401)
            {
                alert(xhr.responseJSON?.message || "請先登入");
                nologin();
            }
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
                nologin();
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
        alert(`連結已複製！快分享給朋友《${shareName}》吧！`);
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
    console.log(button);
    console.log(songId);
    console.log(count);
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
            if (xhr.status === 401) {
                alert("請先登入");
                nologin();
            } else {
                alert("發生錯誤，請稍後再試！");
            }
        }
    });
});

//歌曲投票功能
$(document).on("click", ".btn-vote-to-selectionSong", function () {
    let button = $(this);
    let selectionDetailId = button.attr("data-selectionDetailId");

    console.log("選擇的 selectionDetailId:", selectionDetailId);

    console.log(button);
    console.log(selectionDetailId);

    $.ajax({
        url: `/Selection/AddSelectionVoteSong`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ SelectionDetailId: selectionDetailId }),
        success: function (res) {
            if (res.success) {
                if (res.message.isVoted) {
                    button.addClass("selected");
                } else {
                    button.removeClass("selected");
                }
                $(`span[data-selectionDetailId="${selectionDetailId}"]`).text(res.message.voteCount);
            } else {
                alert("投票失敗，請稍後再試！");
            }
        },
        error: function (xhr) {
            if (xhr.status === 401) {
                alert(xhr.responseJSON?.message || "請先登入");
                nologin();
            }
            else
            alert(xhr.responseJSON?.message || "發生錯誤，請稍後再試！");
        }
    });
});
	

//歌曲播放功能(尚未開發，僅顯示提示訊息)
$(document).on("click", ".btn-play", function () {
    let songName = $(this).data("songname");  // 取得歌曲名稱
    alert(`現正播放：${songName}`);
});

//我也想去按鈕
$(document).on("click", "#btn-interested", function () {
    let activityId = $(this).data("activity-id");
    let button = $(this);
    $.ajax({
        url: `/Activity/ToggleInterested/${activityId}`,
        type: "POST",
        success: function (res) {
            if (res.success) {
                // Controller返回 success interested interestedCount message
                if (res.interested) {
                    button.addClass("selected");
                } else {
                    button.removeClass("selected");
                }

                // 更新興趣人數
                $('#interested-count').text(res.interestedCount);

                // 如果是加入感興趣，顯示分享模態框
                if (res.interested) {
                    $('#ShareActivityModal').modal('show');
                }
            } else {
                alert(res.message || "操作失敗");
            }
        },
        error: function (xhr) {
            if (xhr.status === 401) {
                alert(xhr.responseJSON?.message || "請先登入！");
                window.location.href = "/Accounts/Login";
            }
            else
                alert(xhr.responseJSON?.message || "發生錯誤，請稍後再試！");
        }
    });
});



//  搜尋功能
let searchText = document.querySelector('.header__search-input');

searchText.addEventListener('keydown', function (e) {
    if (e.key === 'Enter') {
        console.log('searchText.value:', searchText.value);
        search();
    }
});

function search() {
    fetch("/search/searchAll/?q=" + searchText.value).
        then(response => {
            //  response.ok 為 true
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            
            //  得到View的URL
            return response.url;
        }).
        then(responseUrl => {
            //  導向到搜尋結果url
         
            window.location.assign(responseUrl);
            
            document.getElementById('All').setAttribute('href', '/search/searchAll/?q=' + searchText.value);
                      
        }).
        catch(error => {
            console.error("載入歌曲失敗");
        })
}

//  收藏歌單
$(document).on("click", ".btn-add-to-Likeplaylist", function () {
    let button = $(this);
    let playlistid = button.data("playlistid");
    // 根據 songId 找到對應的 likeCount
    //let count = $(`.likeCount[data-songid="${songId}"]`);
    console.log(button);
    console.log(playlistid);
    //console.log(count);
    $.ajax({
        url: `/Personal/AddLikePlaylist`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ PlaylistId: playlistid }),
        success: function (res) {
            if (res.isliked) {
                button.addClass("selected");
            }
            else {
                button.removeClass("selected");
            }
            
        },
        error: function (xhr) {
            alert(xhr.responseJSON?.message || "發生錯誤，請稍後再試！");
        }
    });
});
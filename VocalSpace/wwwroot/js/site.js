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
                    if (res.message.oldSelectionDetailId!=null) {
                        $(`.btn-vote-to-selectionSong[data-selectionDetailId="${res.message.oldSelectionDetailId}"]`).removeClass("selected");
                    }
                } else {
                    button.removeClass("selected");
                }
                $(`span[data-selectionDetailId="${selectionDetailId}"]`).text(res.message.voteCount);
                $(`span[data-selectionDetailId="${res.message.oldSelectionDetailId}"]`).text(res.message.oldVoteCount);
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
    let songId = $(this).data("songid");
    //檢查有取得id
    if (!songId) {
        console.error("songId 無效！");
        return;
    }
    $.ajax({
        url: '/MusicPlayer/GetSong',
        method: "GET",
        data: { songId: songId },
        success: function (data) {
            if (data) {
                //初始化音樂撥放器
                InitPartialView(data);
                localStorage.setItem("currentTime", 0);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", error);
        }
    });
});

function InitPartialView(data,continueFromLast = false) {
    // 時間軸計時器
    let songTimer;
    // 取得localStorage播放時間
    var currentTime = continueFromLast ? (localStorage.getItem("currentTime") || 0) : 0;
    console.log(typeof (data));
    $('#musicPlayerContainer').html(data);
    var audioPlayer = document.getElementById("audioPlayer");
    var playButton = document.querySelectorAll(".icon__play-circle")
    var pauseButton = document.querySelectorAll(".icon__stop-circle")
    audioPlayer.currentTime = currentTime;
    audioPlayer.load();
    audioPlayer.play();
            startSongTimer();

    //【音樂撥放器-點選縮小 展開】
    $(".window__header-zoom-out").on("click", function () {
        $(".music-player__window").fadeOut();
    })
    $(".music-player__for-lg-dots,.music-player__for-md").on("click", function () {
        $(".music-player__window").fadeIn();
    })

    //【音樂撥放器-播放/暫停按鈕切換】
    // 音樂播放器 - 播放/暫停按鈕切換
    $(".icon__play-circle, .icon__stop-circle").on("click", function () {
        let $this = $(this);
        let $nextIcon = $this.siblings(".icon__play-circle, .icon__stop-circle");

        // 隱藏目前的按鈕，顯示另一個按鈕
        $this.addClass("hidden");
        $nextIcon.removeClass("hidden");

        // 如果下一個按鈕是 icon__stop-circle，則播放音樂
        if ($nextIcon.is(".icon__stop-circle")) {
            console.log("播放音樂...");
            $("#audioPlayer")[0].play();
            startSongTimer();
        } else {
            console.log("暫停音樂...");
            $("#audioPlayer")[0].pause();
            stopSongTimer();
        }
    });

    // 監聽播放事件

    audioPlayer.addEventListener('play', function () {
        playButton.forEach(button => button.style.display = "none");
        pauseButton.forEach(button => button.style.display = "block");
    });
    //監聽音樂播放結束事件
    audioPlayer.addEventListener('ended', function () {
        playButton.forEach(button => button.style.display = "block");
        pauseButton.forEach(button => button.style.display = "none");
    });

    // 為每個播放按鈕添加點擊事件
    playButton.forEach(button => {
        button.addEventListener('click', function () {
            audioPlayer.play();
            playButton.forEach(btn => btn.style.display = "none");
            pauseButton.forEach(btn => btn.style.display = "block");
        });
    });
    // 為每個暫停按鈕添加點擊事件
    pauseButton.forEach(button => {
        button.addEventListener('click', function () {
            audioPlayer.pause();
            playButton.forEach(btn => btn.style.display = "block");
            pauseButton.forEach(btn => btn.style.display = "none");
        });
    });
    /******點選循環撥放時的效果****/
    $(".music-player__circle-list, .music-player__circle-one").on("click", function () {
        $(this).toggleClass("active");
    });

    // 添加事件監聽器
    audioPlayer.addEventListener("timeupdate", throttle(function () {
        var playedDuration = audioPlayer.currentTime;
        updatePlayedDurationToServer(playedDuration);
    }, 1000)); 

    function updatePlayedDurationToServer(playedDuration) {
        localStorage.setItem("currentTime", playedDuration);
    }
    function throttle(fn, delay) {
        let lastCall = 0;
        return function (...args) {
            let now = Date.now();
            if (now - lastCall >= delay) {
                lastCall = now;
                fn(...args);
            }
        };
    }

    function startSongTimer() {
        let Interval = 1000;
        songTimer = setInterval(sliderProgress, Interval);
    }
    // 停止時間軸計時器
    function stopSongTimer() {
        if (songTimer) {
            clearInterval(songTimer);
            songTimer = null;
        }
    }
    // 時間軸進度條
    function sliderProgress() {
        let duration = $('#audioPlayer').get(0).duration;
        let currentTime = $('#audioPlayer')[0].currentTime;
        let sliderPercent = (currentTime / duration) * 100;
        $('.window__controls-range').val(sliderPercent);
        songTimeText();
    }

    // 更新歌曲現在時間，parseint()轉整數
    function songTimeText() {
        let currentTime = $('#audioPlayer')[0].currentTime;
  
        let min = parseInt(currentTime / 60);
        let seconds = parseInt(currentTime % 60);
        // 10秒內，text呈現'00:00'
        if (seconds < 10) {
            $('#songTimeText').text('0' + min + ':' + '0' + seconds);
        } else {
            $('#songTimeText').text('0' + min + ':' + seconds);
        }

    }

};






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
            if (xhr.status === 401) {
                alert(xhr.responseJSON?.message || "請先登入");
                nologin();
            } else {
                alert(xhr.responseJSON?.message || "發生錯誤，請稍後再試！");
            }
        }
    });
});


$(".btn-play-playlist").on("click", function () {
    var playListId = $(this).attr("data-playListId");
    var songId = $(this).attr("data-songId");
    if (!playListId) {
        console.error("playListId 無效！");
        return;
    }
    $.ajax({
        url: '/MusicPlayer/GetPlayListSong',
        method: "GET",
        data: { playListId: playListId, songId: songId },
        success: function (data) {
            if (data) {
                //初始化音樂撥放器
                InitPartialView(data);
                var audioPlayer = document.getElementById("audioPlayer");
                localStorage.setItem("currentTime", 0);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", error);
        }
    });
});



//  左邊歌曲類別，點選按鈕後變紅色
window.addEventListener("DOMContentLoaded", (event) => {
    const btnWhites = document.getElementsByClassName("btn-white");
    const btnSorts = document.getElementsByClassName("btn-sort");
    const btnArray = Array.from(btnWhites);
    const btnSortArray = Array.from(btnSorts);

    btnArray.forEach(btn => {
        btn.addEventListener("click", function () {
            if (this.classList.contains('active')) {
                this.classList.remove('active');
            } else {
                btnArray.forEach(btn => {
                    btn.classList.remove('active');
                });
                this.classList.add('active');
            }
        })
    })

    btnSortArray.forEach(btn => {
        btn.addEventListener("click", function () {
            if (this.classList.contains('active')) {
                this.classList.remove('active');
            } else {
                btnSortArray.forEach(btn => {
                    btn.classList.remove('active');
                });
                this.classList.add('active');
            }
        })
    })
})

function playSong(songId) {
    alert('播放歌曲 ID: ' + songId);
}

function playAll() {
    alert('播放全部歌曲');
}

// 取得所有按鈕
const buttons = document.querySelectorAll('.btn-outline-secondary.heart');

// 為每個按鈕加上點擊事件
buttons.forEach(button => {
    button.addEventListener('click', function () {
        // 切換選取效果：如果已選取就取消選取，反之則加入選取效果
        this.classList.toggle('selected');
    });
});

// 分享歌曲按鈕之彈跳視窗
function shareshowPopup() {
    document.getElementById('shareoverlay').style.display = 'flex';
}
function shareclosePopup() {
    document.getElementById('shareoverlay').style.display = 'none';
}
function sharecopyLink() {
    const input = document.querySelector('.sharepopup-footer input');
    input.select();
    document.execCommand('copy');
    alert('連結已複製');
}

// 加入歌單按鈕之彈跳視窗
function addopenPopup() {
    document.getElementById("addoverlay").style.display = "block";
    document.getElementById("addpopup").style.display = "block";
    // 加入淡入效果
    setTimeout(() => {
        document.getElementById("addoverlay").style.opacity = "1";
        document.getElementById("addpopup").style.opacity = "1";
        document.getElementById("addpopup").style.top = "30%";
    }, 10);
}

function addclosePopup() {
    // 淡出效果
    document.getElementById("addoverlay").style.opacity = "0";
    document.getElementById("addpopup").style.opacity = "0";
    document.getElementById("addpopup").style.top = "20%";

    // 延遲隱藏（等待動畫完成）
    setTimeout(() => {
        document.getElementById("addoverlay").style.display = "none";
        document.getElementById("addpopup").style.display = "none";
    }, 300);
}

function addtoggleSelection(button) {
    if (button.classList.contains("selected")) {
        button.classList.remove("selected");
        button.textContent = "+ 加入歌單";
    } else {
        button.classList.add("selected");
        button.textContent = "✔ 已選擇";
    }
}
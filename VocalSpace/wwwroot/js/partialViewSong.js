
function playSong(songId) {
    alert('播放歌曲 ID: ' + songId);
}

function playAll() {
    alert('播放全部歌曲');
}

//  
//const observer = new MutationObserver((MutationRecords) => {

//    const tabContent = document.querySelector('.tab-content');
//    // 取得所有按鈕
//    const buttons = document.querySelectorAll('.btn-outline-secondary.heart');

//    // 為每個按鈕加上點擊事件
//    buttons.forEach(button => {
//        button.addEventListener('click', function () {
//            // 切換選取效果：如果已選取就取消選取，反之則加入選取效果
//            this.classList.toggle('selected');
//        });
//    });

//    observer.observe( tabContent , {
//        subtree: true,
//        childList: true
//    })

//})




window.addEventListener("DOMContentLoaded", () => {
    // 取得所有按鈕
    const buttons = document.querySelectorAll('.btn-outline-secondary.heart');

    // 為每個按鈕加上點擊事件
    buttons.forEach(button => {
        button.addEventListener('click', function () {
            // 切換選取效果：如果已選取就取消選取，反之則加入選取效果
            this.classList.toggle('selected');
        });
    });
})


// 分享歌曲按鈕之彈跳視窗
//document.querySelector('.copy').addEventListener('click', sharecopyLink);

async function sharecopyLink() {
    const input = document.querySelector('.share');
    try {
        //  複製 input.value 到 剪貼簿
        await navigator.clipboard.writeText(input.value);
        alert('連結已複製');
    } catch (error) {
            console.error('Failed to copy!', error);
    }
   
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
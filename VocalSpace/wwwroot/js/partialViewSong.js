
function playSong(songId) {
    alert('播放歌曲 ID: ' + songId);
}

function playAll() {
    alert('播放全部歌曲');
}

//  加入歌單按鈕
function addtoggleSelection(button) {
    if (button.classList.contains("selected")) {
        button.classList.remove("selected");
        button.textContent = "+ 加入歌單";
    } else {
        button.classList.add("selected");
        button.textContent = "✔ 已選擇";
    }
}

//  歌曲，音樂人，歌單區塊父容器
const tabContent = document.querySelector('.tab-content');

function ButtonHeart(el) {
    el.classList.toggle('selected');
} 

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


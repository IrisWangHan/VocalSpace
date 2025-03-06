
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

//  愛心按鈕
const tabContent = document.querySelector('.tab-content');
// 取得所有按鈕
const buttons = document.querySelectorAll('.btn-outline-secondary.heart');

function ButtonHeart(  ) {
    buttons.forEach(button => {
        button.addEventListener('click', function () {
            // 切換選取效果：如果已選取就取消選取，反之則加入選取效果
            this.classList.toggle('selected');
        });
    });
} 

window.addEventListener("DOMContentLoaded", () => {
    // 為每個按鈕加上點擊事件
    ButtonHeart();
})

//  MutationObserver 獨立觀察元素內的節點、屬性及文字內容的變化
const observer = new MutationObserver((MutationRecords) => {
    console.log(MutationRecords[0]);
    MutationRecords[0].addedNodes.forEach((node) => {
        

        if (node.nodeType === node.ELEMENT_NODE && node.nextSibling ) {
            console.log(node);
            node.querySelector('.btn-outline-secondary.heart').addEventListener('click', function () {
                // 切換選取效果：如果已選取就取消選取，反之則加入選取效果
                this.classList.toggle('selected');
            });
        }

        //node.querySelectorAll('.btn-outline-secondary.heart').forEach((heart) => {
        //    heart.addEventListener('click', function () {
        //        // 切換選取效果：如果已選取就取消選取，反之則加入選取效果
        //        this.classList.toggle('selected');
        //    });
        //});
        
    });
});

observer.observe(tabContent, {
    subtree: true,
    childList: true
});

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


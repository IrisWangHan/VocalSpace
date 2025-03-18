
//  左邊歌曲類別，點選按鈕後變紅色
window.addEventListener("DOMContentLoaded", (event) => {
    const btnWhites = document.getElementsByClassName("btn-white");

    //let params = new URL(document.location).searchParams;
    //let type = params.get("id");
    //console.log(type);
    const btnSorts = document.getElementsByClassName("btn-sort");
    const btnArray = Array.from(btnWhites);
    const btnSortArray = Array.from(btnSorts);

    //switch (type) {
    //    case type == '1'
    //        document.getElementById('RockLink').classList.toggle('active');
    //        break;

    //}

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



// 分享歌曲按鈕之彈跳視窗

function loadmore(type) {

    fetch("/exploreMusic/loadmore?type=" + type).
        then(response => {
            //  response.ok 為 true
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.text();
        }).
        then(responseHTML => {
            document.getElementById('tab-content').innerHTML += responseHTML;

        }).
        catch(error => {
            console.error("載入歌曲失敗");
        })

}
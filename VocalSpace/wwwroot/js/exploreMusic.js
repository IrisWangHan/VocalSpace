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
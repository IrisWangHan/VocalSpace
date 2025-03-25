window.addEventListener("DOMContentLoaded", (event) => {

    const navnameArray = Array.from(document.getElementsByClassName("nav-name"));
    
    navnameArray.forEach(btn => {
        btn.addEventListener("click", function () {
            if (this.classList.contains('active')) {
                this.classList.remove('active');
            } else {
                navnameArray.forEach(btn => {
                    btn.classList.remove('active');
                });
                this.classList.add('active');
            }
        })
    }) 
})

//  音樂人的追蹤按鈕
function toggleFollow(btn) {
    if (btn.innerText === "追蹤") {
        btn.innerText = "已追蹤";
    } else {
        btn.innerText = "追蹤";
    }
}

//  點 "看更多" 按鈕，顯示更多歌曲，歌單，音樂人
//  <div class="tab-content">裡面一直加入@await Html.PartialAsync("partialViewSong")，帶入partialViewSong的內容
function loadmore(type) {

    fetch("/search/loadmore?type=" + type).
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

////  搜尋功能
//let search = document.querySelector('.header__search-input');

//search.addEventListener('keydown', function (e) {
//    if (e.key === 'Enter') {
//        console.log('searchText.value:', search.value);
//        window.location.href = `/search/searchAll?q=${encodeURIComponent(search.value)}`;
//    }
//});
// 取得目前頁面 URL
const currentPath = window.location.pathname;

// 確保目前頁面是 /search/searchAll
if (currentPath === '/search/searchAll') {
    console.log("search.js");
    // 取得 URL 參數
    const urlParams = new URLSearchParams(window.location.search);
    const query = urlParams.get('q');

    // 如果有 q 參數，則執行 search()
    if (query) {
        document.getElementById('search-query').innerHTML = query;
        searchQuery(query);
    }
}
function searchQuery(content) {
    console.log("/search/searchResult?q=" + content);

    fetch("/search/searchResult?q=" + encodeURIComponent(content)) // 確保 URL 參數編碼正確
        .then(response => {
function search() {
    fetch("/search/searchAll/?q=" + searchText.value).
        then(response => {
            //  response.ok 為 true
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json(); // 解析 JSON
        })
        .then(data => {
            console.log("搜尋結果:", data);
            //alert("搜尋成功！找到 " + data.length + " 首歌曲");
            console.log(response.url);
            return response.url;
        }).
        then(responseUrl => {
            //  導向到搜尋結果url
            window.location.assign(responseUrl);
        }).
        catch(error => {
            console.error("載入歌曲失敗");
        })
        .catch(error => {
            console.error("載入歌曲失敗:", error);
            alert("搜尋失敗，請稍後再試");
        });
}




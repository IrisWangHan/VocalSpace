window.addEventListener("DOMContentLoaded", (event) => {
    const navnameArray = Array.from( document.getElementsByClassName("nav-name") );
    
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
function loadmore() {
    fetch("/search/partialViewSong").
        then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.text();
        }).
        then(responseHTML => {
            document.querySelector('.tab-content').innerHTML += responseHTML;
        }).
        catch(error => {
            console.error("載入歌曲失敗");
        });
}
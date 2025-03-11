// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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
            console.log(response.url);
            //  得到View的URL
            return response.url;
        }).
        then(responseUrl => {
            //  導向到搜尋結果url
            window.location.assign(responseUrl);
        }).
        catch(error => {
            console.error("載入歌曲失敗");
        })
}

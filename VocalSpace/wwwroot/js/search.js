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
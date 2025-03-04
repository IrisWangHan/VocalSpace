//  修改密碼頁面的顯示密碼icon
document.addEventListener("DOMContentLoaded", function () {
    let checkEye = Array.from(document.getElementsByClassName("checkEye"));

    checkEye.forEach(function (element) {
        element.addEventListener("click", function () {
            //  previousElementSibling : 取得icon前一個同層元素
            let password = element.previousElementSibling;
            
            if (password.type == "password") {
                password.type = "text";
            }
            else {
                password.type = "password";
            }
        });
    });
});



    

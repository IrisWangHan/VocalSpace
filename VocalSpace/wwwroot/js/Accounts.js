console.log("測試一下有無載入!!!!");

// 更新註冊頁的進度條顯示
function updateProgress(currentStep) {
    // 更新步驟狀態
    $(".step").each(function () {
        const stepNum = parseInt($(this).data("step"), 10); // 加入基數參數以避免警告
        if (stepNum <= currentStep) {
            $(this).addClass("active");
        } else {
            $(this).removeClass("active");
        }
    });

    // 更新線條狀態
    $(".progress-line").each(function () {
        const lineSteps = $(this).data("line").split("-");
        const startStep = parseInt(lineSteps[0], 10); // 明確指定基數
        const endStep = parseInt(lineSteps[1], 10);
        if (currentStep >= endStep) {
            $(this).addClass("active");
        } else {
            $(this).removeClass("active");
        }
    });
}

// 檢查 Email 是否已存在
function checkUserEmail() {
    return new Promise((resolve) => {
        const userEmail = $("#email").val();
        $.post("/Accounts/CheckUserEmail", { UserEmail: userEmail }, function (data) {
            if (data.exists) {
                alert("此 Email 已被使用！");
                resolve(false);
            } else {
                resolve(true);
            }
        });
    });
}

// 檢查帳號是否已存在
function checkUserAccount() {
    return new Promise((resolve) => {
        const userAccount = $("#userAccount").val();
        $.post("/Accounts/CheckUserAccount", { UserAccount: userAccount }, function (data) {
            if (data.exists) {
                alert("此帳號已被使用！");
                resolve(false);
            } else {
                resolve(true);
            }
        });
    });
}

// 檢查 reCAPTCHA
function validateRecaptcha() {
    let recaptchaResponse = grecaptcha.getResponse();
    if (!recaptchaResponse) {
        alert("請完成 reCAPTCHA 驗證再繼續！");
        return false;
    }
    return true;
}
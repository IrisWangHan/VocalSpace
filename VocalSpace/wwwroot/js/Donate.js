$(document).ready(function () {
    console.log("載入donate.js");
    let selectedAmount = null;
    $(".donation-btn").on("click", function () {
        $(".donation-btn").removeClass("btn-outline-warning").addClass("btn-outline-light");
        $(this).removeClass("btn-outline-light").addClass("btn-outline-warning");
        // 記錄選擇的金額
        selectedAmount = $(this).val();
    });

    $("#confirmDonate").on("click", function () {
        if (!selectedAmount) {
            alert("請先選擇贊助金額！");
            return;
        }

        $("#redirectModal").modal("show");
        $("#donateModal").modal("hide");

        var data = {
            TotalAmount : selectedAmount
        };
        $.ajax({
            type: 'POST',
            url: 'AddOrder',
            data: JSON.stringify(data),
            contentType: 'application/json',        //  送到Server的資料型態
            
            success: function (res) {
                //console.log(res); 
                $('body').html(res);
            },
            error: function (err) {
                console.log(err);
            }
        })

        //         偽 API 導向
        //         setTimeout(function () {

        //         window.location.href = "https:www.ecpay.com.tw/" + selectedAmount;

        //         }, 3000);
        //     });
        // });
    });
})
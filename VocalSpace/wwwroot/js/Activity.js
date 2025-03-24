$(function () {
    console.log('Activity.js已載入');
    let userId = $('#userId').data("userid");
    console.log(userId);
    loadActivityList();
    loadActivityCarousel();

    //宣告一個 filters 物件，用來存放篩選條件
    var filters = {
        keyword: '', // 預設為空字串
        region: 'all', // 默認為全部地區
        startDate: '',
        endDate: ''
    };

    // 點擊所有活動我的活動 按鈕時，重載活動列表
    $('.nav-link').on('click', function () {
        $('.nav-link').removeClass('active');
        $(this).addClass('active');

        initionalize()

        let type = $(this).data('type');

        if (type === "mine" && !userId) {
            alert("請先登入會員！");
            window.location.href = "/Accounts/Login";
        } else {
            // 更新活動列表
            loadActivityList(type === "mine" ? userId : null);
        }
    })

    //初始化篩選條件
    function initionalize(){
            filters = {
            keyword: '', // 預設為空字串
            region: 'all', // 默認為全部地區
            startDate: '',
            endDate: ''
        };
        // 重置篩選按鈕的樣式
        $('.btn-toggle').removeClass('active');
        $('.btn-toggle[data-region="all"]').addClass('active'); // 預設為「全部」
        // 重置日期選擇器的選項
        $('#dateSelect').val(''); // 預設為「近期活動」
        $('#keywordFilter').text(''); // 清空關鍵字
    }

    //載入活動列表
    function loadActivityList(userId) {
        if (typeof filters === 'undefined' || filters === null) {
            filters = { // 如果 filters 是 undefined，重新初始化
                keyword: '',
                region: 'all',
                startDate: '',
                endDate: ''
            };
        }

        let requestData = {
            keyword: filters.keyword || '', // 防止 filters.keyword 為 undefined
            region: filters.region || 'all', // 防止 filters.region 為 undefined
            startDate: filters.startDate || '', // 防止 filters.startDate 為 undefined
            endDate: filters.endDate || '' // 防止 filters.endDate 為 undefined
        };

        if (userId) {
            // 如果有 userId，則傳遞 id
            requestData.id = userId;
        }
        console.log(requestData);
        $.ajax({
            url: '/Activity/GetActivityList/',
            data: requestData,
            success: function (html) {
                $("#activityList-container").html(html);
            },
            error: function (xhr) {
                if (xhr.status === 401) {
                    // 未登入，導向登入頁面
                    window.location.href = "/Accounts/Login";
                } else {
                    alert("發生錯誤，請稍後再試！");
                }
            }
        });
    }

    //載入活動輪播圖
    function loadActivityCarousel() {
        $.ajax({
            url: "/Activity/GetActivityCarousel",
            success: function (html) {
                $("#carousel-container").html(html);
                $('#eventCarousel').carousel(); // 啟動輪播
            }
        });
    }


    // 監聽關鍵字篩選
    $('#keywordFilter').on('input', function () {
        filters.keyword = $(this).val();
        loadActivityList(userId);
    });

    // 讓點擊「地區」按鈕時可以切換 active 狀態
    $('.btn-toggle').on('click', function () {
        $('.btn-toggle').removeClass('active');
        $(this).addClass('active');

        // 獲取選中的地區
        filters.region = $(this).data('region');
        loadActivityList(userId)
    });

    // 更新活動列表標題
    let dateModal = new bootstrap.Modal($('#datePickerModal')[0]); // 初始化日期選擇 Modal

    // 監聽日期篩選變更事件 (14天內、這個月、下個月、**選擇日期不監聽，另外處理) 
    $('#dateSelect').on('change', function () {
        let today = new Date();
        let startDate = "", endDate = "";
        let value = $(this).val();

        if (value === "selectDate") {
            dateModal.show(); // 手動顯示彈窗
            $(this).val(''); // **重置選項，讓下次選擇"4"仍能觸發 change**
            return;
        }

        switch (value) {
            case "14days": // 14天內
                startDate = formatDate(today);
                endDate = formatDate(addDays(today, 14));
                break;
            case "thisMonth": // 這個月
                startDate = formatDate(new Date(today.getFullYear(), today.getMonth(), 1));
                endDate = formatDate(new Date(today.getFullYear(), today.getMonth() + 1, 0));
                break;
            case "nextMonth": // 下個月
                startDate = formatDate(new Date(today.getFullYear(), today.getMonth() + 1, 1));
                endDate = formatDate(new Date(today.getFullYear(), today.getMonth() + 2, 0));
                break;
            default:
                $('#activityListTitle').text("活動列表");
                filters.startDate = "";
                filters.endDate = "";
                loadActivityList(userId)
                return;
        }
        updateTitle(startDate, endDate);
    });

    // 確認選擇日期範圍
    $('#confirmDateRange').on('click', function () {
        let startDate = $('#startDatePicker').val();
        let endDate = $('#endDatePicker').val();

        if (startDate && endDate) {
            if (new Date(startDate) > new Date(endDate)) {
                alert("開始日期不能大於結束日期！");
                return;
            }
            updateTitle(startDate, endDate);
            dateModal.hide();
        } else {
            alert("請選擇完整的日期區間！");
        }
    });

    // 日期格式化 yyyy-MM-dd
    function formatDate(date) {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const day = String(date.getDate()).padStart(2, "0");
        return `${year}-${month}-${day}`;
    }

    // 更新標題
    function updateTitle(start, end) {
        filters.startDate = start;
        filters.endDate = end;
        loadActivityList(userId)
        $('#activityListTitle').text(`活動列表：${start} ~ ${end}`);
    }

    // 格式化日期 (yyyy-MM-dd)
    function formatDate(date) {
        return date.toISOString().split('T')[0];
    }

    // 增加天數
    function addDays(date, days) {
        let result = new Date(date);
        result.setDate(result.getDate() + days);
        return result;
    }
});

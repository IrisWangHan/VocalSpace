
$(".btn-white").on('click', function () {
        // $(this)新增或移除active，其他按鈕移除active
        // $(this).toggleClass('active');
        if ($(this).hasClass('active')) {
            $('.btn-white').removeClass('active');
        } else {
            $(this).addClass('active');
            $(this).parent().siblings().find('a').removeClass('active');
        }
});


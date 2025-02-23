
// 更新註冊頁的進度調顯示
function updateProgress(currentStep) {
    // 更新步驟狀態
    $('.step').each(function() {
        var stepNum = parseInt($(this).data('step'));
        if (stepNum <= currentStep) {
            $(this).addClass('active');
        } else {
            $(this).removeClass('active');
        }
    });

    // 更新線條狀態
    $('.progress-line').each(function() {
        var lineSteps = $(this).data('line').split('-');
        var startStep = parseInt(lineSteps[0]);
        var endStep = parseInt(lineSteps[1]);
        if (currentStep >= endStep) {
            $(this).addClass('active');
        } else {
            $(this).removeClass('active');
        }
    });
}


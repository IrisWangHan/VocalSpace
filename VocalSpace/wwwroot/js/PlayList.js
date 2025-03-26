// 點擊刪除
$(function () {
    let userId = $('.deletelist').data("user-id");
    let playlistIdToDelete = null;
    // 當點擊刪除按鈕時，取得對應的 PlaylistId
    $(".deletelist").on("click", function () {
        playlistIdToDelete = $(this).data("playlist-id");
        console.log(playlistIdToDelete)
    });

    // 當點擊「確定」按鈕時發送刪除請求
    $(document).on("click", ".confirmDelete", function () {
        if (playlistIdToDelete) {
            $.ajax({
                url: `/Collection/Delete/${playlistIdToDelete}`,
                type: "POST",
                success: function (response) {
                    $('#deleteplaylist').on('hidden.bs.modal', function () {
                        alert("刪除sussece");
                        window.location.href =`/Collection/mylist/${userId}`;
                    });
                },
                error: function () {
                    alert("刪除失敗，請稍後再試！");
                }
            });
        }
    });
});

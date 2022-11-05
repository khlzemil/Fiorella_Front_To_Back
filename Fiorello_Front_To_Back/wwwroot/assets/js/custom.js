jQuery(document).ready(function () {

    var skipRow = 0;
    $(document).on('click', '#loadmore', function () {
        console.log("here")
        $.ajax({
            method: "GET",
            url: "/products/loadmore",
            data: {
                skipRow: skipRow
            },
            success: function (result) {
                $('#products').append(result);
                skipRow++;
            }
        })

    })
})
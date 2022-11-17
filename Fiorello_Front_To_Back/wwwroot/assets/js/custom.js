jQuery(function ($) {

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


    $(document).on('click', '#addToCart', function () {
        var id = $(this).data('id');
        $.ajax({
            method: "POST",
            url: "/basket/add",
            data: {
                id: id
            },
            success: function (result) {
                console.log(result);
            }
        })
    })

    $(document).on('click', '#deleteButton', function () {
        var id = $(this).data('id');


        $.ajax({
            method: "POST",
            url: "/basket/delete",
            data: {
                id: id
            },
            success: function (result) {
                $(`.basketProduct[id=${id}]`).remove();
            }

        })
    })


    $(document).on('click', '#upcounter', function () {
        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/basket/upcount",
            data: {
                id: id
            },
            success: function (result) {
                console.log(result);
            }

        })
    })


    $(document).on('click', '#downcounter', function () {
        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/basket/downcount",
            data: {
                id: id
            },
            success: function (result) {
                console.log(result);
            }

        })
    })

})






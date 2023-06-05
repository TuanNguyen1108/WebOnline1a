//thêm sp giỏ hàng
$('#addToCartButton').on('click', function () {
    var selectedOption = $('input[name="productOption"]:checked').val();
    var selectedQuantity = $('#quantity').val();
    // Gửi yêu cầu Ajax để thêm sản phẩm vào giỏ hàng
    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        dataType: "JSON",
        data: {
            idprice: selectedOption,
            SoLuong: selectedQuantity,
            id: $(this).data("id"),
            type: "ajax"
        },
        success: function (response) {
            //alert(response.message);
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Thêm Sản Phẩm Vào Giỏ Hàng Thành Công',
                showConfirmButton: false,
                timer: 1500
            });
            $("#cart_count1").html(response.soLuong);
        },
        error: function () {
            // Hiển thị thông báo lỗi
            alert('Thêm Sản phẩm thất bại.');
        }
    });
});

// thêm sp yêu thích
$(".ajax-add-to-cart1").click(function () {
    $.ajax({
        url: "/CartYeuThich/AddToCart",
        data: {
            id: $(this).data("id"),
            SoLuong: 1,
            type: "ajax"
        },
        success: function (data) {
            console.log(data);
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Thêm Sản Phẩm Yêu Thích Thành Công ! Vui Lòng Xem Ở Sản Phẩm Yêu Thích',
                showConfirmButton: false,
                timer: 1500
            });
            $("#cart_count").html(data.soLuong);
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Thêm Sản Phẩm Yêu Thích Thất Bại',
                text: 'Vui Lòng Thử Lại',
                showConfirmButton: false,
                timer: 1500
            });
        }
    });
});


/*$('#addToCartButton').on('click', function () {
    // Lấy giá trị option được chọn
    var selectedOption = $('input[name="productOption"]:checked').val();
    if (!selectedOption) {
        alert("Vui lòng chọn màu sắc của bạn!");
        *//*
        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'Bạn phải lựa chọn option màu sắc!',
            timer: 1500
        });
        *//*
        return; // Dừng việc thực hiện Ajax request và chuyển tiếp đến controller
    }

    var selectedQuantity = $('#quantity').val();

    // Gửi yêu cầu Ajax để thêm sản phẩm vào giỏ hàng
    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        dataType: "JSON",
        data: {
            idprice: selectedOption,
            SoLuong: selectedQuantity,
            id: $(this).data("id"),

        },
        success: function (response) {
            if (response.success) {
                // Hiển thị thông báo thành công
                //console.log(response.message);
                //alert(response.message);
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Thêm Sản Phẩm Vào Giỏ Hàng Thành Công',
                    showConfirmButton: false,
                    timer: 1500
                });
                $("#cart_count1").html(data.soLuong);
            } else {
                // Hiển thị thông báo lỗi
                // alert('Vui Lòng chọn màu trước khi thêm sản phẩm vào giỏ hàng.');
                Swal.fire({
                    icon: 'error',
                    title: 'Vui Lòng chọn màu trước khi thêm sản phẩm vào giỏ hàng.',
                    text: 'Bạn chưa chọn màu',
                    showConfirmButton: false,
                    timer: 1500
                });
            }
        },
        error: function () {
            // Hiển thị thông báo lỗi
            alert('Thêm Sản phẩm thất bại.');
        }
    });
});*/


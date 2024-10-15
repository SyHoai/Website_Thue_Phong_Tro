

$(document).ready(function () {
    $.ajax({
        url: 'https://esgoo.net/api-tinhthanh/1/0.htm',
        method: 'GET',
        success: function (data) {
            var thanhPhoSelect = $('#thanhPhoSelect');
            var actionName = $('#actionName').data('action');
            if (actionName !== "Create" && actionName !== "Edit" && actionName !== "Register" && actionName !== "Manage" ) {
                thanhPhoSelect.append($('<option>', {
                    value: 0,
                    text: "Toàn quốc"
                }));
            }
            data.data.forEach(function (item) {
                thanhPhoSelect.append($('<option>', {
                    value: item.id,
                    text: item.full_name
                }));
            });
        },

    });


    $('#thanhPhoSelect').change(function () {
        var selectedCityId = $(this).val();

        $('#phuongXaSelect').empty()
        $(this).find('option[value=""]').remove();

        var selectedTinhThanhText = $('#thanhPhoSelect option:selected').text();
        $('#selectedTinhThanh').val(selectedTinhThanhText);
        $.ajax({
            url: 'https://esgoo.net/api-tinhthanh/2/' + selectedCityId + '.htm' ,
            method: 'GET',
            success: function (data) {
                var quanHuyenSelect = $('#quanHuyenSelect');
                quanHuyenSelect.empty()
                var actionName = $('#actionName').data('action');
                    if (selectedTinhThanhText !== "Toàn quốc") {
                        quanHuyenSelect.append($('<option>', {
                            value: 0,
                            text: "Chọn quận huyện",
                            disabled: 'disabled',
                            selected: 'selected',
                            hidden: 'hidden'
                        }));
                        if (actionName !== "Create" && actionName !== "Edit" && actionName !== "Register") {
                            quanHuyenSelect.append($('<option>', {
                                value: 0,
                                text: "Tất cả"
                            }));

                        }
                        
                    }

                data.data.forEach(function (item) {
                    quanHuyenSelect.append($('<option>', {
                        value: item.id,
                        text: item.full_name
                    }));
                });

            },
        });
    });


    $('#quanHuyenSelect').change(function () {
        var selectedDistrictsId = $(this).val();
        var selectedQuanHuyenText = $('#quanHuyenSelect option:selected').text();
        $('#selectedQuanHuyen').val(selectedQuanHuyenText);
        $.ajax({
            url: 'https://esgoo.net/api-tinhthanh/3/' + selectedDistrictsId + '.htm',
            method: 'GET',
            success: function (data) {
                var phuongXaSelect = $('#phuongXaSelect');
                phuongXaSelect.empty()
                var actionName = $('#actionName').data('action');
                if (selectedQuanHuyenText !== "Tất cả") {
                    phuongXaSelect.append($('<option>', {
                        value: 0,
                        text: "Chọn phường xã",
                        disabled: 'disabled',
                        selected: 'selected',
                        hidden: 'hiden'
                    }));
                    if (actionName !== "Create" && actionName !== "Edit" && actionName !== "Register") {
                        phuongXaSelect.append($('<option>', {
                            value: 0,
                            text: "Tất cả"
                        }));
                    }
                }
                data.data.forEach(function (item) {
                    phuongXaSelect.append($('<option>', {
                        value: item.id,
                        text: item.full_name
                    }));
                });

            },
        });
    });

    $('#phuongXaSelect').change(function () {
        var selectedPhuongXaText = $('#phuongXaSelect option:selected').text();
        $('#selectedPhuongXa').val(selectedPhuongXaText);
    });

    $('#loaiPhongSelect').change(function () {
        var selectedText = $(this).find("option:selected").text();
        $('#selectedLoaiPhong').val(selectedText);
    });

    
 /*   document.getElementById('sdtButton').addEventListener('click', function () {
        var fullPhoneNumber = $('#sdtUser').data('action');
        document.getElementById('sdtButton').textContent = fullPhoneNumber;
    });*/
    document.getElementById('chooseImageButton').addEventListener('click', function () {
        event.preventDefault();
        document.getElementById('imageInput').click();
       
    });

    document.getElementById('imageInput').addEventListener('change', function (event) {
        var previewImages = document.getElementById('previewImages');


        while (previewImages.firstChild) {
            previewImages.removeChild(previewImages.firstChild);

        }

        var files = event.target.files;
        for (var i = 0; i < files.length; i++) {
            displayImage(files[i]);
        }
    });


    function displayImage(file) {
        var reader = new FileReader();
        reader.onload = function (event) {
            var img = document.createElement('img');
            img.src = event.target.result;
            img.style.maxWidth = '100px';
            img.style.maxHeight = '100px';
            img.style.marginRight = '50px';
            
            var previewImages = document.getElementById('previewImages');


            previewImages.appendChild(img);
        };

        reader.readAsDataURL(file);
    }

});


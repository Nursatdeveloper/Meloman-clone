$(document).ready(function () {
    $(".header-offcanvas-section-items").hide();
    $(".order-form-wrapper").hide()
    $("#home-address-form").hide();
    $("#shop-address-form").hide();
    $("#show-order-form-btn").hide();
    console.log("hide");
});
function openSideBar(x) {
    x.classList.toggle("change");
}
$(".menu-icon").click(function () {
    var actionType = $("#header-offcanvas-action-type").val();
    if (actionType == "hide") {
        $("#header-offcanvas-action-type").val("show");
        $(".header-offcanvas-section-container").css('width', '360px');
        $(".header-offcanvas-section-container").css('border-top', '1px solid #a6a6a6');
        $(".header-offcanvas-section-items").show();
    }
    else {
        $("#header-offcanvas-action-type").val("hide");
        $(".header-offcanvas-section-container").css('width', '0px');
        $(".header-offcanvas-section-items").hide();
    }
})
$(".header-offcanvas-section-items").mouseover(function () {
    $(this).addClass('background-orange');
    $(".header-offcanvas-section-items a").mouseover(function () {
        $(this).addClass('background-gray');
    })

})
$(".header-offcanvas-section-items").mouseout(function () {
    $(".header-offcanvas-section-items a").mouseout(function () {
        $(this).removeClass('background-gray');
    })
    $(this).removeClass('background-orange');

});

$("#get-all-reviews").click(function () {
    $(".profile-main-menu-body").empty();
    $.ajax({
        url: "/Review/ReviewList",
        type: "POST",
        data: { 'productId': 0, 'productType': "All" },
        cache: false,
        success: function (data) {
            addReviewTable();
            for (var i = 0; i < data.length; i++) {
                var date = data[i].Time.slice(0, 10);
                addDataToReviewTable(data[i].ReviewId, data[i].ProductId, data[i].ProductType, data[i].Username, data[i].Advantages, data[i].Disadvantages, data[i].GeneralReview, data[i].Rating, date);
            }
            downloadToExcelBtn('Review');
        }
    })
})
$("#get-all-books").click(function () {
    $.ajax({
        url: "/Books/GetAllBooks",
        type: "GET",
        success: function (data) {
            addBookTable();
            for (var i = 0; i < data.length; i++) {
                addDataToBookTable(data[i].BookId, data[i].Name, data[i].Author, data[i].Category, data[i].Genre, data[i].PublishedYear, data[i].Price, data[i].Discount, data[i].Amount, data[i].Language)
            }
            downloadToExcelBtn('Books');

        }
    })
})

$("#get-my-basket-items").click(function () {
    showBasketItemsTable();
    var bookIds = JSON.parse(sessionStorage.getItem('bookIds'));
    if (bookIds == null || bookIds[0] == null) {
        return showMessage('Корзина пустая!')
    }
    
    for (var i = 0; i < bookIds.length; i++) {
        $.ajax({
            type: "POST",
            url: "/Books/FindBook",
            data: { 'id': bookIds[i] },
            cache: false,
            success: function (data) {
                addBooksToBasketItems(data.BookId, data.Name, data.Author, data.Price, data.PhotoFront);
            }
        })
    }
    if (bookIds.length != 0) {
        console.log("show")
        $("#show-order-form-btn").show()
        $.ajax({
            url: "/Books/GetFinalPrice",
            type: "POST",
            data: { 'bookIds': bookIds, 'operation': 'plus' },
            cache: false,
            success: function (data) {
                $("#first-price").text(data[0]);
                var discount = round(data[1], 2);
                $("#payment-summary-discount").text(discount);
                $("#final-price").text(data[2]);
            }
        })
    }
})
function ValidateEmail(mail) {
    if (/^\w+([\.-]?\w+)*@@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
        return (true)
    }
    showMessage("Вы ввели неправильный формат электронной почты!")
    return (false)
}
function resetOrderForm() {
    $("#receiver-tel").val('');
    $("#receiver-name").val('');
    $("#receiver-email").val('');
    $('#city-selection').prop('selectedIndex', 0);
    $('input[name="delivery-method"]').prop('checked', false);
    $("#delivery-street").val('')
    $("#delivery-house").val('');
    $("#delivery-office").val('');
    $("#shop-address-select").prop('selectedIndex', 0);
    if ($("#want-express-hidden-input").val() != 'want') {
        wantExpressClick()
    }
    $('input[name="payment-method"]').prop('checked', false);
    $('input[type=radio]').prop('checked', false);
    $("#comments-textarea").val('');
    $("#shop-address-form").hide();
    $("#home-address-form").hide();
    $("#delivery-method-banner-2").css('background-color', 'white');
    $("#delivery-method-banner-2").find('span:nth-child(1)').css('color', '#0100d3');
    $("#delivery-method-banner-2").find('span:nth-child(3)').css('color', '#4d4d4d');
    $("#delivery-method-banner-1").css('background-color', 'white');
    $("#delivery-method-banner-1").find('span:nth-child(1)').css('color', '#0100d3');
    $("#delivery-method-banner-1").find('span:nth-child(3)').css('color', '#4d4d4d');
    $("#pay-online").css('background-color', 'white');
    $("#pay-online").find('span').css('color', '#0100d3');
    $("#pay-on-delivery").css('background-color', 'white');
    $("#pay-on-delivery").find('span').css('color', '#0100d3');
}

function processOrder() {
    var telephone = $("#receiver-tel").val();
    if (telephone == '') {
        return showMessage('Заполните поле для телефона!')
    }
    if (telephone.length != 11 && telephone.length != 12) {
        return showMessage('Не правильный формат телефона!')
    }
    var name = $("#receiver-name").val();
    if (name == '') {
        return showMessage('Заполните поле для имя!')
    }
    var email = $("#receiver-email").val();
    if (email != '') {
        ValidateEmail(email);
    }
    else {
        email = "Не указан"
    }
    var city = $("#city-selection").val();
    if (city == 'Выберите регион') {
        return showMessage('Выберите город доставки!')
    }
    var address = '';
    var delivery = ''
    if ($("#deliver-to-shop").is(":checked")) {
        address = $("#shop-address-select").val();
        delivery = 'Доставка в магазин'
        if (address == 'Выберите адрес магазина') {
            return showMessage("Выберите адрес магазина!")
        }
    }
    else if ($("#deliver-to-home").is(":checked")) {
        delivery = "Доставка домой"
        var street = $("#delivery-street").val();
        var house = $("#delivery-house").val();
        var houseNumber = $("#delivery-office").val();
        if (street == '' || house == '' || houseNumber == '') {
            return showMessage('Заполните поля для вашего адреса!')
        }
        else {
            address = `ул. ${street} ${house}, ${houseNumber}`;
        }
    }
    else {
        return showMessage('Выберите способ доставки!')
    }
    var isExpress = 'нет';
    if ($("#want-express-hidden-input").val() != 'want') {
        isExpress = 'да';
    }
    var comments = $("#comments-textarea").val();
    if (comments == '') {
        comments = "Без комментариев"
    }
    var paymentType = ''
    if ($("#pay-on-delivery-radio").is(':checked')) {
        paymentType = 'При получении'
    } else if ($("#pay-online-radio").is(":checked")) {
        paymentType = 'Онлайн';
    }
    else {
        return showMessage('Выберите способ оплаты!')
    }
    var initialPrice = $("#first-price").text()
    var discount = $("#payment-summary-discount").text()
    var finalPrice = $("#final-price").text()
    var order = [telephone, name, email, city, delivery, address, isExpress, paymentType, initialPrice, discount, finalPrice, comments];
    var bookIds = JSON.parse(sessionStorage.getItem('bookIds'));
    $.ajax({
        type: "POST",
        url: "/Order/AddBookOrder",
        data: { 'order': order, "bookIds": bookIds },
        cache: false,
        success: function (data) {
            showMessage(data);
            resetOrderForm()
        }
    })
}
function showOrderForm() {
    console.log("i am showorderform")
    $(".order-form-wrapper").show();
}
function showBasketItemsTable() {
    //$(".profile-main-menu-body").empty();
    $(".table").hide();
    $(".profile-main-menu-body").prepend(`
            <div class="basket-list-container">
            </div>
            <button id="show-order-form-btn" onclick="showOrderForm()">Оформить заказ</button>
        `)
}
function addBooksToBasketItems(id, name, author, price, img) {
    $(".basket-list-container").append(`
            <div class="basket-list-item" id="basket-list-item-${id}">
                <div class="basket-list-img-container">
                    <img src="data:image/jpg;base64,${img}"/>
                </div>
                <div class="basket-list-product-name">
                    <span>Название товара:</span>
                    <span>${author}: ${name}</span>
                </div>
                <div class="basket-list-product-price">
                    <span>Цена:</span>
                    <span id="basket-list-product-price-${id}">${price}</span>
                    <img style="width:12px; height:13px; padding-bottom:2px;margin-left:2px;" src="https://upload.wikimedia.org/wikipedia/commons/thumb/5/51/Tenge_symbol.jpg/144px-Tenge_symbol.jpg" />
                </div>
                <div class="basket-list-product-quantity">
                    <button class="basket-list-btn" onclick="quantityMinus(this)">-</button>
                    <input id="${id}" value="1"/>
                    <button class="basket-list-btn" onclick="quantityPlus(this)">+</button>
                </div>
                <div class="basket-list-product-delete">
                    <button class="basket-list-btn" value="${id}" onclick="deleteFromBasket(this)">Удалить</button>
                </div>
            </div>
        `)
}
function deleteFromBasket(elem) {
    var id = elem.value;
    $(`#basket-list-item-${id}`).hide();
    var bookIds = JSON.parse(sessionStorage.getItem('bookIds'));
    for (var i = 0; i < bookIds.length; i++) {
        if (bookIds[i] == id) {
            bookIds[i] = null;
        }
    }
    var array = [];
    var j = 0;
    for (var i = 0; i < bookIds.length; i++) {
        if (bookIds[i] != null) {
            array[j] = bookIds[i];
            j++;
        }
    }
    if (array.length == 0) {
        $(".order-form-wrapper").hide()
        $("#show-order-form-btn").hide();
        sessionStorage.setItem('bookIds', JSON.stringify(array));
    }
    else {
        sessionStorage.setItem('bookIds', JSON.stringify(array));
        $.ajax({
            url: "/Books/GetFinalPrice",
            type: "POST",
            data: { 'bookIds': array, 'operation': 'plus' },
            cache: false,
            success: function (data) {
                $("#first-price").text(data[0]);
                var discount = round(data[1], 2);
                $("#payment-summary-discount").text(discount);
                $("#final-price").text(data[2]);
            }
        })
    }

}

function round(value, decimals) {
    return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}

function quantityMinus(elem) {
    var quantity = parseInt($(elem).next().val());
    var id = $(elem).next().attr('id');
    console.log('id: ' + id);
    if (quantity == 1) {

    } else {
        $(elem).next().val(quantity - 1);
        var price = parseInt($(`#basket-list-product-price-${id}`).text()); //id
        console.log(price)
        var oneItemPrice = price / quantity;
        var currentPrice = price - oneItemPrice;
        $(`#basket-list-product-price-${id}`).text(currentPrice)
        var array = JSON.parse(sessionStorage.getItem('bookIds'));


        $.ajax({
            url: "/Books/GetFinalPrice",
            type: "POST",
            data: { 'bookIds': array, 'operation': 'minus' },
            cache: false,
            success: function (data) {
                $("#first-price").text(data[0]);
                var discount = round(data[1], 2);
                $("#payment-summary-discount").text(discount);
                $("#final-price").text(data[2]);
            }
        })
        for (var j = array.length - 1; j >= 0; j--) {
            if (array[j] == id) {
                array[j] = null;
                break;
            }
        }
        var newArr = []
        var k = 0;
        for (var i = 0; i < array.length; i++) {
            if (array[i] == null) {
                continue;
            }
            else {
                newArr[k] = array[i];
                k++;
            }
        }
        sessionStorage.setItem('bookIds', JSON.stringify(newArr));

    }
}
function quantityPlus(elem) {
    var quantity = parseInt($(elem).prev().val());
    var id = $(elem).prev().attr('id');
    console.log('id: ' + id);
    $(elem).prev().val(quantity + 1);
    var price = parseInt($(`#basket-list-product-price-${id}`).text()); // id
    console.log(price)
    var oneItemPrice = price / quantity;
    var currentPrice = price + oneItemPrice;
    $(`#basket-list-product-price-${id}`).text(currentPrice)
    var array = JSON.parse(sessionStorage.getItem('bookIds'));
    array[array.length] = id;
    sessionStorage.setItem('bookIds', JSON.stringify(array));

    $.ajax({
        url: "/Books/GetFinalPrice",
        type: "POST",
        data: { 'bookIds': array, 'operation': 'plus' },
        cache: false,
        success: function (data) {
            $("#first-price").text(data[0]);
            var discount = round(data[1], 2);
            $("#payment-summary-discount").text(discount);
            $("#final-price").text(data[2]);
        }
    })

}

function showMessage(message) {
    var toastLiveExample = document.getElementById('liveToast')
    $(".toast-body").text(message);
    var toast = new bootstrap.Toast(toastLiveExample)
    toast.show()
}

function addBookTable() {
    $(".profile-main-menu-body").empty();
    $(".profile-main-menu-body").append(`
           <div class="table">
                <div class="table-header">
                    <div class="table-col-small">
                        <span>Id Книги</span>
                    </div>
                    <div class="table-col-medium">
                        <span>Название</span>
                    </div>
                    <div class="table-col-medium">
                        <span>Автор</span>
                    </div>
                    <div class="table-col-medium">
                        <span>Категория</span>
                    </div>
                    <div class="table-col-medium">
                        <span>Жанр</span>
                    </div>
                    <div class="table-col-small">
                        <span>Год</span>
                    </div>
                    <div class="table-col-small">
                        <span>Цена (тг)</span>
                    </div>
                    <div class="table-col-small">
                        <span>Скидка</span>
                    </div>
                    <div class="table-col-small-2">
                        <span>В наличии</span>
                    </div>
                    <div class="table-col-small">
                        <span>Язык</span>
                    </div>
                    <div class="table-col-small-2  table-col-options">
                        <span>Опции</span>
                    </div>
                </div>
                <div class="table-body">

                </div>
            </div >
            `);
}
function addDataToBookTable(bookId, name, author, category, genre, year, price, discount, amount, language) {
    $(".table-body").append(`
            <div class="review-table-row">
                <div class="table-col-small">
                    <span>${bookId}</span>
                </div>
                <div class="table-col-medium">
                    <span>${name}</span>
                </div>
                <div class="table-col-medium">
                    <span>${author}</span>
                </div>
                <div class="table-col-medium">
                    <span>${category}</span>
                </div>
                <div class="table-col-medium">
                    <span>${genre}</span>
                </div>
                <div class="table-col-small">
                    <span>${year}</span>
                </div>
                <div class="table-col-small">
                    <span>${price}</span>
                </div>
                <div class="table-col-small">
                    <span>${discount}%</span>
                </div>
                <div class="table-col-small-2">
                    <span>${amount}</span>
                </div>
                <div class="table-col-small">
                    <span>${language}</span>
                </div>
                <div class="table-col-small-2" >
                    <input type="hidden" id="options-hidden-input-${bookId}" value="open"/>
                    <button id="options-btn-${bookId}" value="${bookId}" class="options-btn" onclick="showOptions(this)">
                        Открыть
                    </button>
                    <div style="position:relative;">
                        <div id="option-list-${bookId}" class="options-list options-hide">
                            <li class="add-descrip-btn"><a class="options-list-link" href="/Books/AddBookDescription/${bookId}">Добавить описание</a></li>
                            <li id="${bookId}" onclick="deleteBook(this)" class="del-btn"><a class="options-list-link" >Удалить</a></li>
                        </div>
                    </div>
                </div>
            </div>
        `)
}
function deleteBook(elem) {
    var id = elem.id;
    var areYouSure = confirm("Вы действительно хотите удалить эту книгу?");
    if (areYouSure == true) {
        $.ajax({
            url: "/Books/Delete",
            type: "POST",
            data: { 'id': id },
            cache: false,
            success: function (data) {
                if (data == "success") {
                    showMessage("Книга успешно удалено!")
                } else {
                    showMessage("Не удалось удалить книгу!")
                }

            }
        })
    }
}

function showOptions(elem) {
    var btnId = elem.value;
    var hiddenInputVal = $(`#options-hidden-input-${btnId}`).val();
    if (hiddenInputVal == 'open') {
        $(`#options-btn-${btnId}`).text("Закрыть")
        $(`#option-list-${btnId}`).removeClass('options-hide');
        $(`#options-hidden-input-${btnId}`).val('close')
    } else {
        $(`#options-btn-${btnId}`).text("Открыть");
        $(`#option-list-${btnId}`).addClass('options-hide');
        $(`#options-hidden-input-${btnId}`).val('open')
    }

}

function addReviewTable() {
    $(".profile-main-menu-body").empty();
    $(".profile-main-menu-body").append(`
            <div class="reviews-table">
                <div class="review-table-header">
                    <div class="review-table-rev-id">
                        <span>Id Отзывов</span>
                    </div>
                    <div class="review-table-prod-id">
                        <span>Id Продукта</span>
                    </div>
                    <div class="review-table-prod-type">
                        <span>Тип Продукта</span>
                    </div>
                    <div class="review-table-user">
                        <span>Пользователь</span>
                    </div>
                    <div class="review-adv-disadv">
                        <span>Отзыв</span>
                    </div>
                    <div class="review-table-rating">
                        <span>Оценка</span>
                    </div>
                    <div class="review-table-date">
                        <span>Время публикации</span>
                    </div>
                </div>
                <div class="review-table-body">

                </div>
            </div >
            `);
}
function addDataToReviewTable(reviewId, productId, productType, user, advantage, disadvantage, review, rating, date) {
    $(".review-table-body").append(`
            <div class="review-table-row">
                <div class="review-table-rev-id">
                    <span>${reviewId}</span>
                </div>
                <div class="review-table-prod-id">
                    <span>${productId}</span>
                </div>
                <div class="review-table-prod-type">
                    <span>${productType}</span>
                </div>
                <div class="review-table-user">
                    <span>${user}</span>
                </div>
                <div class="review-adv-disadv">
                    <span>Достоинства: ${advantage}, Недостатки: ${disadvantage}, Отзыв: ${review}</span>
                </div>
                <div class="review-table-rating">
                    <span>${rating}</span>
                </div>
                <div class="review-table-date">
                    <span>${date}</span>
                </div>
            </div>
            `)
}

function downloadToExcelBtn(item) {
    $(".profile-main-menu-body").append(`
            <a id="download-excel-btn" href="/${item}/DownloadToExcel">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="white" class="bi bi-file-earmark-excel" viewBox="0 0 16 16">
                    <path d="M5.884 6.68a.5.5 0 1 0-.768.64L7.349 10l-2.233 2.68a.5.5 0 0 0 .768.64L8 10.781l2.116 2.54a.5.5 0 0 0 .768-.641L8.651 10l2.233-2.68a.5.5 0 0 0-.768-.64L8 9.219l-2.116-2.54z"/>
                    <path d="M14 14V4.5L9.5 0H4a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2zM9.5 3A1.5 1.5 0 0 0 11 4.5h2V14a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h5.5v2z"/>
                  </svg>
                  <span>Скачать в Excel</span>
            </a>
        `)
}

function wantExpressClick() {
    if ($("#want-express-hidden-input").val() == 'want') {
        $("#want-express-hidden-input").val('dont-want')
        $(".want-express-icon-container").addClass('rotate-45');
        $(".want-express-icon-container").css('background-color', "#0100d3");
        $(".want-express-banner").css('background-color', "#f15a2c");
        $(".want-express-text-container span:first-child").text('Не хочу экспесс');
        $(".want-express-text-container span:nth-child(3)").text('Убрать экспресс доставку');
    } else {
        $("#want-express-hidden-input").val('want')
        $(".want-express-icon-container").removeClass('rotate-45');
        $(".want-express-icon-container").css('background-color', "#f15a2c");
        $(".want-express-banner").css('background-color', "#0100d3");
        $(".want-express-text-container span:first-child").text('Xочу экспесс');
        $(".want-express-text-container span:nth-child(3)").text('Вернуть экспресс доставку');
    }
}

$(".want-express-banner").click(function () {
    if ($("#want-express-hidden-input").val() == 'want') {
        $("#want-express-hidden-input").val('dont-want')
        $(".want-express-icon-container").addClass('rotate-45');
        $(".want-express-icon-container").css('background-color', "#0100d3");
        $(".want-express-banner").css('background-color', "#f15a2c");
        $(".want-express-text-container span:first-child").text('Не хочу экспесс');
        $(".want-express-text-container span:nth-child(3)").text('Убрать экспресс доставку');
    } else {
        $("#want-express-hidden-input").val('want')
        $(".want-express-icon-container").removeClass('rotate-45');
        $(".want-express-icon-container").css('background-color', "#f15a2c");
        $(".want-express-banner").css('background-color', "#0100d3");
        $(".want-express-text-container span:first-child").text('Xочу экспесс');
        $(".want-express-text-container span:nth-child(3)").text('Вернуть экспресс доставку');
    }

})
$("#delivery-method-banner-1").click(function () {
    $(this).find('input[type=radio]').attr('checked', true);
    $(this).css('background-color', '#0100d3');
    $(this).find('span:nth-child(1)').css('color', 'white');
    $(this).find('span:nth-child(3)').css('color', 'white');
    $("#delivery-method-banner-2").css('background-color', 'white');
    $("#delivery-method-banner-2").find('span:nth-child(1)').css('color', '#0100d3');
    $("#delivery-method-banner-2").find('span:nth-child(3)').css('color', '#4d4d4d');
    $("#shop-address-form").show();
    $("#home-address-form").hide();

})
$("#delivery-method-banner-2").click(function () {
    $(this).find('input[type=radio]').attr('checked', true);
    $(this).css('background-color', '#0100d3');
    $(this).find('span:nth-child(1)').css('color', 'white');
    $(this).find('span:nth-child(3)').css('color', 'white');
    $("#delivery-method-banner-1").css('background-color', 'white');
    $("#delivery-method-banner-1").find('span:nth-child(1)').css('color', '#0100d3');
    $("#delivery-method-banner-1").find('span:nth-child(3)').css('color', '#4d4d4d');
    $("#home-address-form").show();
    $("#shop-address-form").hide();
})
$("#shop-address-select").click(function () {
    if ($("#shop-address-select option").length == 1) {
        showMessage('К сожалению доставка не работает в вашем городе!')
    }
    else {
        //do smth else
    }
})

$("#city-selection").change(function () {
    if ($("#city-selection").val() == 'Nursultan') {
        $("#shop-address-select").empty();
        $("#shop-address-select").append(`
                <option value='Meloman-clone ТРЦ Керуен'>Meloman-clone ТРЦ "Керуен"</option>
                <option value='Meloman-clone ТРЦ Mega Silk Way'>Meloman-clone ТРЦ "Mega Silk Way"</option>
                <option value='Meloman-clone прос. Республика 57'>Meloman-clone прос. Республика 57</option>
            `)
    } else if ($("#city-selection").val() == 'Almaty') {
        $("#shop-address-select").empty();
        $("#shop-address-select").append(`
                <option value='Meloman-clone ТРЦ Mega Park'>Meloman-clone ТРЦ "Mega Park"</option>
                <option value='Meloman-clone ТРЦ Dostyk Plaza'>Meloman-clone ТРЦ "Dostyk Plaza"</option>
                <option value='Meloman-clone прос. Ал-Фараби 71'>Meloman-clone прос. Ал-Фараби 71</option>
            `)
    }
    else if ($("#city-selection").val() == 'Shymkent') {
        $("#shop-address-select").empty();
        $("#shop-address-select").append(`
                <option value='Meloman-clone ул Кошкарбаева 47'>Meloman-clone ул Кошкарбаева 47</option>
                <option value='Meloman-clone ул Кудайбергенов 6/4"'>Meloman-clone ул Кудайбергенов 6/4"</option>
                <option value='Meloman-clone ТРЦ МАРТ'>Meloman-clone ТРЦ МАРТ</option>
            `)
    } else {
        $("#shop-address-select").empty();
        $("#shop-address-select").append(`
                <option value=''>Выберите адрес магазина</option>
            `)
    }

})
$("#pay-on-delivery").click(function () {
    $(this).find('input[type=radio]').attr('checked', true);
    $("#pay-online").find('input[type=radio]').attr('checked', false);
    $(this).css('background-color', '#0100d3');
    $(this).find('span').css('color', 'white');
    $("#pay-online").css('background-color', 'white');
    $("#pay-online").find('span').css('color', '#0100d3');
    $("#payment-information-container").css('border-top', '1px solid #ccc')
    $("#payment-information-container").text('Оплата при получении: доступна оплата заказа наличными или банковскими картами Visa, MasterCard, Maestro и American Express')
})
$("#pay-online").click(function () {
    $(this).find('input[type=radio]').attr('checked', true);
    $("#pay-on-delivery").find('input[type=radio]').attr('checked', false);
    $(this).css('background-color', '#0100d3');
    $(this).find('span').css('color', 'white');
    $("#pay-on-delivery").css('background-color', 'white');
    $("#pay-on-delivery").find('span').css('color', '#0100d3');
    $("#payment-information-container").css('border-top', '1px solid #ccc')
    $("#payment-information-container").text('Онлайн банковской картой: принимаем карты Visa, MasterCard, Maestro и American Express')
})

//my orders
function showProductsModal(orderId) {
    $.ajax({
        url: "/Order/GetOrder",
        type: "POST",
        data: { 'id': orderId },
        cache: false,
        success: function (data) {
            var products = data.Products;
            $(".modal-body").empty();
            $("#exampleModalLabel").text("Товары")
            $(".modal-body").append(`
                <div class="product-list-modal"></div >`)
            for (var i = 0; i < products.length; i++) {
                $(".product-list-modal").append(`
                        <div class="product-list-modal-row">
                            <div class="product-type-container">
                                <span class="product-type">Тип продукта:</span>
                                <span>
                                    ${products[i].ProductType}
                                </span>
                            </div>
                            <div class="product-name-container">
                                <span class="product-name">Название:</span>
                                <span>
                                    ${products[i].ProductName}
                                </span>
                            </div>
                        </div>
                    `)
            }
        }
    })
}
function showOrderDetails(orderId) {
    $.ajax({
        url: "/Order/GetOrder",
        type: "POST",
        data: { 'id': orderId },
        cache: false,
        success: function (data) {
            $(".modal-body").empty();
            $(".modal-body").append(`
                <div class="order-details-modal"></div >`)
            $("#exampleModalLabel").text("Детали заказа")
            $(".order-details-modal").append(`
                        <div class="order-information">
                            <div class="order-details-col">
                                <div class="order-details-row">
                                    <span>Заказчик</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Email</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Город доставки</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Вид доставки</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Адрес доставки</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Экспресс доставка</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Платеж</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Цена без учета скидок</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Скидка</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Итоговая цена</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Дата</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Комментарий</span>
                                </div>
                                <div class="order-details-row">
                                    <span>Статус доставки</span>
                                </div>
                            </div>
                            <div class="order-details-col">
                                <div class="order-details-row">
                                    <span>${data.Name}</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.Email}</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.City}</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.DeliveryType}</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.Address}</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.IsExpressDelivery}</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.PaymentType}</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.InitialPrice}тг</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.Discount}%</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.FinalPrice}тг</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.Date}</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.Comments}</span>
                                </div>
                                <div class="order-details-row">
                                    <span>${data.DeliveryStatus}</span>
                                </div>
                            </div>
                        </div>
                        <div class="product-information">
                            
                        </div>
                    `)
            var products = data.Products;
            for (var i = 0; i < products.length; i++) {
                $(".product-information").append(`
                        <div class="product-information-row">
                                <div class="product-type-container">
                                    <span>Тип продукта</span>
                                    <span>${products[i].ProductType}</span>
                                </div>
                                <div class="product-name-container">
                                    <span>Название</span>
                                    <span>${products[i].ProductName}</span>
                                </div>
                            </div>
                    `)
            }
        }
    })
}
function showDeliveryStatusModal(orderId) {
    $("#delivery-status-modal-content").append(`
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Изменить статус доставки</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <select id="delivery-status">
                    <option value="В пути">В пути</option>
                    <option value="Доставлен">Доставлен</option>
                </select>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-bs-dismiss="modal" onclick="changeDeliveryStatus(${orderId})">Сохранить</button>
            </div>`)
}
function changeDeliveryStatus(orderId) {
    var status = $("#delivery-status").val();
    $.ajax({
        url: "/Order/EditOrder",
        type: "POST",
        data: { 'id': orderId, 'status': status },
        cache: false,
        success: function (data) {
            showMessage(data);
        }
    })
}
function addOrderTable() {
    $(".basket-list-container").hide();
    $(".order-form-wrapper").hide();
    $("#show-order-form-btn").hide();
    $(".profile-main-menu-body").append(`
           <div class="table">
                <div class="table-header">
                    <div class="table-col-small">
                        <span>Id Заказа</span>
                    </div>
                    <div class="table-col-medium">
                        <span>Заказчик</span>
                    </div>
                    <div class="table-col-medium">
                        <span>Город</span>
                    </div>
                    <div class="table-col-medium">
                        <span>Вид доставки</span>
                    </div>
                    <div class="table-col-medium">
                        <span>Адрес доставки</span>
                    </div>
                    <div class="table-col-small-2">
                        <span>Express</span>
                    </div>
                    <div class="table-col-small-2">
                        <span>Цена (тг)</span>
                    </div>
                    <div class="table-col-medium">
                        <span>Телефон</span>
                    </div>
                    <div class="table-col-small-2">
                        <span>Статус</span>
                    </div>
                    <div class="table-col-small-2  table-col-options">
                        <span>Опции</span>
                    </div>
                </div>
                <div class="table-body">

                </div>
            </div >
            `);
}
function addDataToOrderTable(id, person, city, deliveryType, address, isExpress, price, tel, status, products) {
    $(".table-body").append(`
            <div class="review-table-row">
                <div class="table-col-small">
                    <span>${id}</span>
                </div>
                <div class="table-col-medium">
                    <span>${person}</span>
                </div>
                <div class="table-col-medium">
                    <span>${city}</span>
                </div>
                <div class="table-col-medium">
                    <span>${deliveryType}</span>
                </div>
                <div class="table-col-medium">
                    <span>${address}</span>
                </div>
                <div class="table-col-small-2">
                    <span>${isExpress}</span>
                </div>
                <div class="table-col-small-2">
                    <span>${price}</span>
                </div>
                <div class="table-col-medium">
                    <span>${tel}</span>
                </div>
                <div class="table-col-small-2">
                    <span>${status}</span>
                </div>
                <div class="table-col-small-2" >
                    <input type="hidden" id="options-hidden-input-${id}" value="open"/>
                    <button id="options-btn-${id}" value="${id}" class="options-btn" onclick="showOptions(this)">
                        Открыть
                    </button>
                    <div style="position:relative;">
                        <div id="option-list-${id}" class="options-list options-hide">
                            <li class="add-descrip-btn"><a class="options-list-link" data-bs-toggle="modal" data-bs-target="#productsModal" onclick="showOrderDetails(${id})">Детали заказа</a></li>
                            <li class="add-descrip-btn"><a class="options-list-link" data-bs-toggle="modal" data-bs-target="#productsModal" onclick="showProductsModal(${id})">Посмотреть товары</a></li>
                        </div>
                    </div>
                </div>
            </div>
        `)
}
$("#get-my-orders").click(function () {
    addOrderTable();
    $.ajax({
        url: "/Order/GetOrderList",
        type: "POST",
        data: { 'requestedBy': 'user' },
        cache: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                addDataToOrderTable(data[i].OrderId, data[i].Name, data[i].City, data[i].DeliveryType, data[i].Address, data[i].IsExpressDelivery, data[i].FinalPrice, data[i].Telephone, data[i].DeliveryStatus, data[i].Products)
            }
        }
    })
})
$("#get-all-orders").click(function () {
    $(".profile-main-menu-body").empty();

    addOrderTable();
    $.ajax({
        url: "/Order/GetOrderList",
        type: "POST",
        data: { 'requestedBy': 'admin' },
        cache: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                addDataToOrderTable(data[i].OrderId, data[i].Name, data[i].City, data[i].DeliveryType, data[i].Address, data[i].IsExpressDelivery, data[i].FinalPrice, data[i].Telephone, data[i].DeliveryStatus, data[i].Products)
                $(`#option-list-${data[i].OrderId}`).append(`<li class="add-descrip-btn"><a class="options-list-link" data-bs-toggle="modal" data-bs-target="#changeStatusModal" onclick="showDeliveryStatusModal(${data[i].OrderId})">Изменить статус</a></li>`)
            }
            downloadToExcelBtn('Order');
        }
    })
})
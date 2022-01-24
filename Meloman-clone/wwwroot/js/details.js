$(document).ready(function () {
    var id = $("#id-input").val();
    $("#description-hide-icon").hide();
    $("#characteristics-hide-icon").hide();
    $("#home-address-form").hide();
    $("#shop-address-form").hide();
    $("#no-delivery").hide();
    $.ajax({
        type: "POST",
        url: "/Books/FindBook",
        data: { 'id': id },
        cache: false,
        success: function (data) {
            $("#genre-store").val(data.Genre);
            $("#side-img-1").attr("src", "data:image/jpg;base64," + data.PhotoFront + "");
            if (data.PhotoBack == null) {
                $(".img-container2").hide();
            }
            else {
                $("#side-img-2").attr("src", "data:image/jpg;base64," + data.PhotoBack + "");
            }
            $("#main-img").attr("src", "data:image/jpg;base64," + data.PhotoFront + "");
            $("#front-photo-store").val("data:image/jpg;base64," + data.PhotoFront + "");
            $("#back-photo-store").val("data:image/jpg;base64," + data.PhotoBack + "");
            var price = data.Price;
            var oldprice = data.OldPrice;
            var moneysave = oldprice - price;
            $("#money-going-to-save").text(`${moneysave} T`);

            if (data.AuthorPhoto == null) {
                $("#author-img").hide();
                $(".author-wrapper").hide();
            } else {
                $("#author-img").attr("src", "data:image/jpg;base64," + data.AuthorPhoto + "");
                $("#author-name").text(data.Author);
            }
            $.ajax({
                type: "POST",
                url: "/Books/SortByGenre",
                data: { 'size': 8, 'genre': data.Genre },
                cache: false,
                success: function (book) {
                    for (var i = 1; i < book.length+1; i++) {
                        $(`#img-${i}`).attr("src", "data:image/jpg;base64," + book[i - 1].PhotoFront + "");
                        if (book[i - 1].Author != '') {
                            $(`#author-name-${i}`).text(book[i - 1].Author + ':');
                        }
                        $(`#book-name-${i}`).text(book[i - 1].Name);
                        if (book[i - 1].Discount != 0) {
                            $(`#discount-block-${i}`).text(`-${book[i - 1].Discount}%`);
                            $(`#discount-block-${i}`).css('background-color', '#f15a2c');
                        }
                        else {
                            $(`#discount-block-${i}`).css('background-color', 'white');
                        }
                        if (book[i - 1].OldPrice != 0) {
                            $(`#old-price-${i}`).text(book[i - 1].OldPrice + " T");
                        }
                        $(`#price-${i}`).text(book[i - 1].Price + " T");
                        $(`#book-name-wrapper-${i}`).attr("id", `${book[i - 1].BookId}`);
                        $(`#to-cart-input-${i}`).val(book[i - 1].BookId);
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
            $.ajax({
                type: "POST",
                url: "/Books/GetBookDescription",
                data: { 'bookid': data.BookId },
                cache: false,
                success: function (description) {
                    if (description == "empty") {
                        $("#book-description-body-id").text("Описание отсутствует");
                    }
                    $("#book-description-body-id").text(description);
                },
                error: function (err) {
                    console.log(err);
                }

            })
        },
        error: function (err) {
            console.log(err);
        }

    });



});
var swiper = new Swiper('.swiper-container', {
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    }
})
function filledStar() {
    return `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-star-fill" viewBox="0 0 16 16">
                    < path d = "M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z" />
                </svg >`;
}
function emptyStar() {
    return `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-star" viewBox="0 0 16 16">
                    <path d="M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z"/>
                </svg>`;
}
function showMessage(message) {
    var toastLiveExample = document.getElementById('liveToast')
    $(".toast-body").text(message);
    var toast = new bootstrap.Toast(toastLiveExample)
    toast.show()
}



$("#side-img-2").click(function () {
    $(".img-container2").addClass("border-visible");
    $(".img-container").removeClass("border-visible");
    var photoBase64String = $("#back-photo-store").val();
    $("#main-img").attr("src", photoBase64String);
});
$("#side-img-1").click(function () {
    $(".img-container").addClass("border-visible");
    $(".img-container2").removeClass("border-visible");
    var photoBase64String = $("#front-photo-store").val();
    $("#main-img").attr("src", photoBase64String);
});
$(".book-name-wrapper").click(function () {
    var id = $(this).attr("id");
    $.ajax({
        type: "POST",
        url: "/Books/RedirectToDetails",
        data: { 'id': id },
        cache: false,
        success: function (data) {
            window.location.href = data;
        },
        error: function (err) {
            console.log(err);
        }
    });
});

$("#book-description-toggle-button").click(function () {
    var actionType = $("#description-toggle-button-action").val();
    if (actionType == 'hide') {
        $("#description-toggle-button-action").val('show');
        $(".book-description-container").css("height", "fit-content");
        $("#book-description-toggle-button").text("Свернуть описание");
        $("#description-hide-icon").show();
        $("#description-show-icon").hide();
        $(".book-description-container").removeClass("linear-gradient-opacity");
    }

    if (actionType == 'show') {
        $("#description-toggle-button-action").val('hide');
        $(".book-description-container").css("height", "100px");
        $("#book-description-toggle-button").text("Развернуть описание");
        $("#description-hide-icon").hide();
        $("#description-show-icon").show();
        $(".book-description-container").addClass("linear-gradient-opacity");
    }
});
$("#book-characteristics-toggle-button").click(function () {
    var actionType = $("#characteristics-toggle-button-action").val();
    if (actionType == 'hide') {
        $("#characteristics-toggle-button-action").val('show');
        $(".book-characteristics-container").css("height", "fit-content");
        $("#book-characteristics-toggle-button").text("Свернуть характеристики");
        $("#characteristics-hide-icon").show();
        $("#characteristics-show-icon").hide();
        $(".book-characteristics-container").removeClass("linear-gradient-opacity");
    }

    if (actionType == 'show') {
        $("#characteristics-toggle-button-action").val('hide');
        $(".book-characteristics-container").css("height", "100px");
        $("#book-characteristics-toggle-button").text("Развернуть характеристики");
        $("#characteristics-hide-icon").hide();
        $("#characteristics-show-icon").show();
        $(".book-characteristics-container").addClass("linear-gradient-opacity");
    }
})

$("#add-btn").click(function () {
    var bookId = $(`#id-input`).val();
    if (sessionStorage.getItem("bookIds") === null) {
        var bookIds = [];
        bookIds[0] = bookId;
        sessionStorage.setItem('bookIds', JSON.stringify(bookIds));
        showMessage('Книга добавлена в корзину!')
    } else {
        var storedBookIds = JSON.parse(sessionStorage.getItem('bookIds'));
        var arrLength = storedBookIds.length;
        storedBookIds[arrLength] = bookId;
        sessionStorage.setItem('bookIds', JSON.stringify(storedBookIds));
        showMessage('Книга добавлена в корзину!')
    }
})

function addToCart(elem) {
    var elemId = elem.id;
    var bookId = $(`#${elemId}`).children('input').val();
    if (sessionStorage.getItem("bookIds") === null) {
        var bookIds = [];
        bookIds[0] = bookId;
        sessionStorage.setItem('bookIds', JSON.stringify(bookIds));
        showMessage('Книга добавлена в корзину!')
    } else {
        var storedBookIds = JSON.parse(sessionStorage.getItem('bookIds'));
        var arrLength = storedBookIds.length;
        storedBookIds[arrLength] = bookId;
        sessionStorage.setItem('bookIds', JSON.stringify(storedBookIds));
        showMessage('Книга добавлена в корзину!')
    }
}

//order-form
function redirectToLogin() {
    window.location.href = `/User/Login?ReturnUrl=${window.location.pathname}`
}
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
    var userId = ''
    var order = [telephone, name, email, city, delivery, address, isExpress, paymentType, initialPrice, discount, finalPrice, comments, userId];
    console.log(order);
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
        $("#no-delivery").show();
    }
    else {
        //do smth else
    }
})

$("#city-selection").change(function () {
    if ($("#city-selection").val() == 'Nursultan') {
        $("#shop-address-select").empty();
        $("#shop-address-select").append(`
                <option value='Meloman-clone ТРЦ "Керуен"'>Meloman-clone ТРЦ "Керуен"</option>
                <option value='Meloman-clone ТРЦ "Mega Silk Way"'>Meloman-clone ТРЦ "Mega Silk Way"</option>
                <option value='Meloman-clone прос. Республика 57'>Meloman-clone прос. Республика 57</option>
            `)
    } else if ($("#city-selection").val() == 'Almaty') {
        $("#shop-address-select").empty();
        $("#shop-address-select").append(`
                <option value='Meloman-clone ТРЦ "Mega Park"'>Meloman-clone ТРЦ "Mega Park"</option>
                <option value='Meloman-clone ТРЦ "Dostyk Plaza"'>Meloman-clone ТРЦ "Dostyk Plaza"</option>
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
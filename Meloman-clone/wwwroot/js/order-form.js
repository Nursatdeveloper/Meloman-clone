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
    if ($("#shop-address-select").length == 1) {
        //message: delivery to shop is not supported in your selected city
        alert("is not supported")
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
$(document).ready(function () {
    $(".sort-price-options").hide();
    $(".sort-discount-options").hide();
    $.ajax({
        url: "/Books/GetBooksSortedByCategory",
        type: "POST",
        data: { 'category': 'Детская литература' },
        cache: false,
        success: function (data) {
            var len = data.length;
            for (let i = 0; i < len; i++) {
                $(`#img-${data[i].BookId}`).attr("src", "data:image/jpg;base64," + data[i].PhotoFront + "");
                if (data[i].Discount != 0) {
                    $(`#discount-block-${data[i].BookId}`).text(`-${data[i].Discount}%`);
                } else {
                    $(`#discount-block-${data[i].BookId}`).css("background-color", "white");
                }
            }
        }
    });
    $.ajax({
        type: "POST",
        url: "/Books/GetAuthorsOfExistingBooks",
        data: { 'category': 'Детская литература' },
        cache: false,
        success: function (data) {
            var len = data.length;
            for (var i = 0; i < len; i++) {
                $("#books-author-list-id").append(`<div class="books-author-list-item">
                                                            <input type="checkbox" onchange="authorCheckboxHandler(this)" value="${data[i]}" />
                                                            <span>${data[i]}</span>
                                                        </div>`)
            }
        }
    });
    $.ajax({
        type: "POST",
        url: "/Books/GetGenresOfExistingBooks",
        data: { 'category': 'Детская литература' },
        cache: false,
        success: function (data) {
            var len = data.length;
            for (var i = 0; i < len; i++) {
                $("#books-genre-list-id").append(`<div class="books-author-list-item">
                                                        <input type="checkbox" onchange="genreCheckboxHandler(this)" value="${data[i]}"/>
                                                        <span>${data[i]}</span>
                                                    </div>`)
            }
        }
    })

    $("#price-range-from-input").change(function () {
        var fromPrice = $("#price-range-from-input").val();
        sortByPrice(fromPrice)
    });
    $("#price-range-to-input").change(function () {
        var fromPrice = $("#price-range-from-input").val();
        var toPrice = $("#price-range-to-input").val();
        sortByPrice(fromPrice, toPrice)
    })
    $("#books-author-search-input").change(function () {
        var searchWord = $("#books-author-search-input").val();
        searchForAuthor(searchWord)
    });

    $("#sort-selling-hits").click(function () {
        $("#sort-selling-hits").addClass("font-bold");
        $("#sort-news").removeClass("font-bold");
        $("#sort-prices").removeClass("font-bold");
        $("#sort-discount").removeClass("font-bold");
        getSortedBooks("/Books/GetSortedBooks"); //logic needs to be changed after implementation of purchase functionality
    })

    $("#sort-news").click(function () {
        $("#sort-news").addClass("font-bold");

        $("#sort-selling-hits").removeClass("font-bold");
        $("#sort-prices").removeClass("font-bold");
        $("#sort-discount").removeClass("font-bold");
        $.ajax({
            type: "POST",
            url: "/Books/GetBooksSortedByCategory",
            data: { 'category': 'Детская литература' },
            cache: false,
            success: function (data) {
                $(".book-list-container").empty();
                if (data.length == 0) {
                    $(".book-list-container").append(`
                        <div style="width:100%; font-size:20px; font-family:600; margin-top:30px; text-align:center;">Найдено 0 товаров по сортировке!</div>
                    `)
                }

                var len = data.length;
                for (var i = len - 1; i >= 0; i--) {
                    addBooksToBookListContainer(data[i].BookId, data[i].Author, data[i].Name, data[i].PhotoFront, data[i].Discount, data[i].OldPrice, data[i].Price);
                    if (data[i].Discount == 0) {
                        $(`#discount-block-${data[i].BookId}`).css("background-color", "white");
                    }
                    if (data[i].OldPrice == 0) {
                        $(`#item-old-price-${data[i].BookId}`).hide();
                    }
                }
            }
        })
    })
    $("#sort-prices").click(function () {
        $(".sort-price-options").toggle();
        $("#sort-prices").addClass("font-bold");
        $("#sort-news").removeClass("font-bold");
        $("#sort-selling-hits").removeClass("font-bold");
        $("#sort-discount").removeClass("font-bold");
    })
    $("#sort-by-increasing").click(function () {
        getSortedBooks("/Books/SortByIncreasingPrice");
    })
    $("#sort-by-decreasing").click(function () {
        getSortedBooks("/Books/SortByDecreasingPrice");
    })

    $("#sort-discount").click(function () {
        $(".sort-discount-options").toggle();
        $("#sort-prices").removeClass("font-bold");
        $("#sort-news").removeClass("font-bold");
        $("#sort-selling-hits").removeClass("font-bold");
        $("#sort-discount").addClass("font-bold");
    });
    $("#sort-discount-by-increasing").click(function () {
        getSortedBooks("/Books/SortByIncreasingDiscount");
    })
    $("#sort-discount-by-decreasing").click(function () {
        getSortedBooks("/Books/SortByDecreasingDiscount");
    })


})



function sortByPrice(fromPrice, toPrice) {
    if (toPrice == '') {
        $.ajax({
            url: "/Books/GetBooksSortedByPrice",
            type: "POST",
            data: { 'fromPrice': fromPrice, 'toPrice': 0 },
            cache: false,
            success: function (data) {
                $(".book-list-container").empty();

                var len = data.length;
                for (var i = 0; i < len; i++) {
                    addBooksToBookListContainer(data[i].BookId, data[i].Author, data[i].Name, data[i].PhotoFront, data[i].Discount, data[i].OldPrice, data[i].Price);
                    if (data[i].Discount == 0) {
                        $(`#discount-block-${data[i].BookId}`).css("background-color", "white");
                    }
                    if (data[i].OldPrice == 0) {
                        $(`#item-old-price-${data[i].BookId}`).hide();
                    }
                }
            }
        });
    }
    else {
        var toPrice = $("#price-range-to-input").val()
        $.ajax({
            url: "/Books/GetBooksSortedByPrice",
            type: "POST",
            data: { 'fromPrice': fromPrice, 'toPrice': toPrice, 'category': 'Детская литература' },
            cache: false,
            success: function (data) {
                $(".book-list-container").empty();

                var len = data.length;
                for (var i = 0; i < len; i++) {
                    addBooksToBookListContainer(data[i].BookId, data[i].Author, data[i].Name, data[i].PhotoFront, data[i].Discount, data[i].OldPrice, data[i].Price);
                    if (data[i].Discount == 0) {
                        $(`#discount-block-${data[i].BookId}`).css("background-color", "white");
                    }
                    if (data[i].OldPrice == 0) {
                        $(`#item-old-price-${data[i].BookId}`).hide();
                    }
                }
            }
        })
    }
}

function addBooksToBookListContainer(BookId, Author, Name, PhotoFront, Discount, OldPrice, Price) {
    $(".book-list-container").append(`
            <div id="${BookId}" class="item-block" onclick="redirectToDetails(this)">
                <div class="book-container">
                    <div class="img-wrapper">
                        <img id="img-${BookId}" src="data:image/jpg;base64,${PhotoFront}" />
                        <div class="add-towishlist-and-search-btn">
                            <a>
                                <div class="add-to-wishlist">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="28" height="30" fill="white" class="bi bi-heart" viewBox="0 0 16 16">
                                        <path d="m8 2.748-.717-.737C5.6.281 2.514.878 1.4 3.053c-.523 1.023-.641 2.5.314 4.385.92 1.815 2.834 3.989 6.286 6.357 3.452-2.368 5.365-4.542 6.286-6.357.955-1.886.838-3.362.314-4.385C13.486.878 10.4.28 8.717 2.01L8 2.748zM8 15C-7.333 4.868 3.279-3.04 7.824 1.143c.06.055.119.112.176.171a3.12 3.12 0 0 1 .176-.17C12.72-3.042 23.333 4.867 8 15z" />
                                    </svg>
                                </div>
                            </a>
                            <a>
                                <div class="details-modal">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="28" height="30" fill="white" class="bi bi-search" viewBox="0 0 16 16">
                                        <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z" />
                                    </svg>
                                </div>
                            </a>
                        </div>
                    </div>
                    <div class="book-name-wrapper" id="${BookId}" onclick="redirectToDetails(this)">
                        <span id="">${Author}</span>: <span id="book-name-1">${Name}</span>
                    </div>
                    <div class="discount-wrapper">
                        <div id="discount-block-${BookId}" class="discount-block">
                            ${Discount}%
                        </div>

                        <div class="book-location">
                            На складе
                        </div>
                    </div>
                    <div class="bottom-wrapper">
                        <div class="price-wrapper">
                                <div class="item-block-old-price" id="item-old-price-${BookId}">
                                    <span id="old-price-1">
                                        ${OldPrice}
                                    </span>
                                    <img style="width:10px; height:17px; padding-bottom:5px;" src="https://upload.wikimedia.org/wikipedia/commons/thumb/5/51/Tenge_symbol.jpg/144px-Tenge_symbol.jpg" />
                                </div>

                            <div class="item-block-price">
                                <span id="price-1">${Price}</span>
                                <img style="width:12px; height:20px; padding-bottom:5px;" src="https://upload.wikimedia.org/wikipedia/commons/thumb/5/51/Tenge_symbol.jpg/144px-Tenge_symbol.jpg" />
                            </div>
                        </div>
                        <div class="tocart-icon" id="tocart-icon-${BookId}" onclick="addToBasketList(this)">
                            <input class="to-cart-input" type="hidden" value="${BookId}"/>
                            <svg xmlns="http://www.w3.org/2000/svg" width="50" height="31" fill="white" class="bi bi-cart" viewBox="0 0 16 16">
                                <path d="M0 1.5A.5.5 0 0 1 .5 1H2a.5.5 0 0 1 .485.379L2.89 3H14.5a.5.5 0 0 1 .491.592l-1.5 8A.5.5 0 0 1 13 12H4a.5.5 0 0 1-.491-.408L2.01 3.607 1.61 2H.5a.5.5 0 0 1-.5-.5zM3.102 4l1.313 7h8.17l1.313-7H3.102zM5 12a2 2 0 1 0 0 4 2 2 0 0 0 0-4zm7 0a2 2 0 1 0 0 4 2 2 0 0 0 0-4zm-7 1a1 1 0 1 1 0 2 1 1 0 0 1 0-2zm7 0a1 1 0 1 1 0 2 1 1 0 0 1 0-2z" />
                            </svg>
                        </div>
                    </div>
                </div>
            </div>
`)
}

function searchForAuthor(searchWord) {
    $.ajax({
        type: "POST",
        url: "/Books/GetAuthorsOfExistingBooks",
        data: { 'category': 'Детская литература' },
        cache: false,
        success: function (data) {
            console.log(data);
            $("#books-author-list-id").empty();
            var len = data.length;
            for (var i = 0; i < len; i++) {
                var authorName = data[i].toLowerCase()
                if (authorName.includes(searchWord.toLowerCase())) {
                    $("#books-author-list-id").append(`<div class="books-author-list-item">
                                                            <input type="checkbox" onchange="authorCheckboxHandler(this)" value="${data[i]}" />
                                                            <span>${data[i]}</span>
                                                        </div>`)
                }
            }
        }
    });
}

function redirectToDetails(elem) {
    var id = elem.id;
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
}

var authors = [];
function authorCheckboxHandler(elem) {
    if (elem.checked) {
        authors.push(elem.value);
    }
    else {
        authors = authors.filter(author => author != elem.value);
    }
    getSortedBooks("/Books/GetSortedBooks")
}

var suggestedAges = [];
function ageCheckboxHandler(elem) {
    if (elem.checked) {
        suggestedAges.push(elem.value);
    }
    else {
        suggestedAges = suggestedAges.filter(age => age != elem.value);
    }
    getSortedBooks("/Books/GetSortedBooks")
}

var publishedYears = [];
function yearCheckboxHandler(elem) {
    if (elem.checked) {
        publishedYears.push(elem.value);
    }
    else {
        publishedYears = publishedYears.filter(year => year != elem.value);
    }
    getSortedBooks("/Books/GetSortedBooks")
}

var covers = [];
function coverCheckboxHandler(elem) {
    if (elem.checked) {
        covers.push(elem.value);
    }
    else {
        covers = covers.filter(cover => cover != elem.value);
    }
    getSortedBooks("/Books/GetSortedBooks")
}

var genres = [];
function genreCheckboxHandler(elem) {
    if (elem.checked) {
        genres.push(elem.value);
    }
    else {
        genres = genres.filter(genre => genre != elem.value);
    }
    getSortedBooks("/Books/GetSortedBooks")
}

function getSortedBooks(urlPath) {
    $.ajax({
        url: urlPath,
        type: "POST",
        data: {
            'category': 'Детская литература',
            'authors': authors,
            'suggestedAges': suggestedAges,
            'publishedYears': publishedYears,
            'covers': covers,
            'genres': genres
        },
        cache: false,
        success: function (data) {
            $(".book-list-container").empty();
            if (data.length == 0) {
                $(".book-list-container").append(`
                        <div style="width:100%; font-size:20px; font-family:600; margin-top:30px; text-align:center;">Найдено 0 товаров по сортировке!</div>
                    `)
            }

            var len = data.length;
            for (var i = 0; i < len; i++) {
                addBooksToBookListContainer(data[i].BookId, data[i].Author, data[i].Name, data[i].PhotoFront, data[i].Discount, data[i].OldPrice, data[i].Price);
                if (data[i].Discount == 0) {
                    $(`#discount-block-${data[i].BookId}`).css("background-color", "white");
                }
                if (data[i].OldPrice == 0) {
                    $(`#item-old-price-${data[i].BookId}`).hide();
                }
            }
        }
    })
}

function addToBasketList(elem) {
    var id = elem.id;
    var bookId = $(`#${id}`).children('input').val();
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
function showMessage(message) {
    var toastLiveExample = document.getElementById('liveToast')
    $(".toast-body").text(message);
    var toast = new bootstrap.Toast(toastLiveExample)
    toast.show()
}
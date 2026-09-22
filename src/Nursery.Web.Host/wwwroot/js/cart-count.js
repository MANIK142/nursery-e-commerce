

var cart = document.getElementById("CartCount");
UpdateCartBadge();
function UpdateCartBadge() {
    $.ajax({
        url: '/customer/plants/getcartcount',
        type: 'GET',
        error: function (xhr, error, code) {
            console.error("Status:", xhr.status);
            console.error("Response:", xhr.responseText);
        },
        success: function (response) {
            cart.innerHTML = response;
        }
    });
}




let addBtns = document.querySelectorAll(".add-product-to-basket-btn");

addBtns.forEach(a => a.addEventListener("click", function (e) {
    e.preventDefault();
    fetch(e.target.href)
        .then(response => response.text())
        .then(data => {
            $('.cart-block').html(data);
        })
}));





$(document).on('click', '.remove-product-to-basket-btn', function (e) {

    e.preventDefault();
    fetch(e.target.href)
        .then(response => response.text())
        .then(data => {
            $('.cart-block').html(data);
        })

})

$(document).on('click', '.modal-show', function (e) {

    e.preventDefault();
    console.log(e.target.href)

    fetch(e.target.href)
        .then(response => response.text())
        .then(data => {
            $('.product-details-modal').html(data);
        })

    $("#quickModal").modal("show");

})





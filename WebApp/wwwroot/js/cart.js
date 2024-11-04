const openCartBtns = document.querySelectorAll(".cart-btn-open");
const closeCartBtns = document.querySelectorAll(".cart-btn-close");
const cart = document.querySelector(".cart");


function cartToggle() {
    if (cart.classList.contains("hide")) {
        cart.classList.remove("hide");
        cart.classList.add("show");
        //fetchCart();
    } else {
        cart.classList.remove("show");
        cart.classList.add("hide");
    }
}

window.addEventListener("load", function () {
    openCartBtns.forEach(btn => btn.addEventListener("click", cartToggle));
    closeCartBtns.forEach(btn => btn.addEventListener("click", cartToggle));
});

function fetchCart() {
    console.log("fetchCart")
    //fetch("/Cart/Index")
    //    .then(response => response.text())
    //    .then(html => {
    //        const overlayContainer = document.getElementById("cartOverlayContainer");
    //        overlayContainer.innerHTML = html;
    //        overlayContainer.style.display = "block";
    //    })
    //    .catch(error => console.error("Error loading cart items:", error));
}

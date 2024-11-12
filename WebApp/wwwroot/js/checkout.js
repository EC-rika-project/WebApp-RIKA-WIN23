// Function to toggle credit card form
function toggleCreditCardForm(selectedOption) {
    const creditCardForm = document.getElementById("creditCardForm");

    if (selectedOption === "CreditCard") {
        creditCardForm.style.display = "flex";
    } else {
        creditCardForm.style.display = "none";
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const selectedOption = document.querySelector('input[name="SelectedOption"]:checked');
    if (selectedOption && selectedOption.value === "CreditCard") {
        toggleCreditCardForm(selectedOption.value);
    }
});

// Function to format correct price
function formatCheckoutPrice(value) {
    return `${value.toFixed(2)} Sek`;
}

// Function to calculate subtotal
function calculateCheckoutSubtotal() {
    let coSubtotal = 0;

    document.querySelectorAll(".co-cart-item").forEach(item => {
        const coPrice = parseFloat(item.dataset.coPrice);
        const coQuantity = parseInt(item.querySelector(".quantity .co-quantity").textContent);

        if (!isNaN(coPrice) && !isNaN(coQuantity)) {
            coSubtotal += coPrice * coQuantity;
        }
    });

    document.querySelector(".subtotal-row .co-subtotal").textContent = formatCheckoutPrice(coSubtotal);
    calculateCheckoutShipping(coSubtotal)
}

// Function to calculate shipping cost
function calculateCheckoutShipping(coSubtotal) {
    const coShippingElement = document.querySelector(".co-shipping");
    let coShippingCost = coSubtotal > 1000 || coSubtotal === 0 ? 0 : 50;
    coShippingElement.textContent = formatCheckoutPrice(coShippingCost);
    calculateCheckoutTotal(coSubtotal, coShippingCost)
}

// Function to calculate order total price
function calculateCheckoutTotal(coSubtotal, coShippingCost) {
    const coTotalElement = document.querySelector(".co-cart-total");
    let coTotal = coSubtotal + coShippingCost
    coTotalElement.textContent = formatCheckoutPrice(coTotal)
}

// Function to save updated quantity of a product to local storage
function saveCheckoutUpdate(articleNumber, newQuantity) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    const itemIndex = cart.findIndex(item => item.product.articleNumber === articleNumber);

    if (newQuantity === 0 && itemIndex !== -1) {
        cart.splice(itemIndex, 1);
        localStorage.setItem('cart', JSON.stringify(cart));
    }
    else if (itemIndex !== -1) {
        cart[itemIndex].quantity = newQuantity;
        localStorage.setItem('cart', JSON.stringify(cart));
    }
    loadCartFromLocalStorage();
}

// Function to add eventlisteners to the quantity/delete buttons
function setupEventListeners() {
    document.querySelectorAll(".qty-btn").forEach(button => {
        button.addEventListener("click", () => {
            const item = button.closest(".co-cart-item");
            const quantityElement = item.querySelector(".co-quantity");
            const priceElement = item.querySelector(".co-price"); 
            let quantity = parseInt(quantityElement.textContent);
            let price = parseFloat(item.getAttribute("data-co-price"));
            let articleNumber = item.getAttribute("data-co-articleNumber")
            const decreaseButton = item.querySelector(".co-decrease");


            if (button.classList.contains("co-increase")) {
                quantity++;
                saveCheckoutUpdate(articleNumber, quantity)
               
            } else if (button.classList.contains("co-decrease") && quantity > 1) {
                quantity--;
                saveCheckoutUpdate(articleNumber, quantity)
               
            }

            priceElement.textContent = formatCheckoutPrice(price * quantity)
            quantityElement.textContent = quantity;
            calculateCheckoutSubtotal();

            if (quantity <= 1) {
                decreaseButton.style.visibility = "hidden";
            } else {
                decreaseButton.style.visibility = "visible";
            }
        });
    });

    document.querySelectorAll('.co-delete').forEach(deleteButton => {
        deleteButton.addEventListener('click', function (event) {
            const cartItem = event.target.closest('.co-cart-item');
            let articleNumber = cartItem.getAttribute("data-co-articleNumber")

            if (cartItem) {
                cartItem.remove();
                saveCheckoutUpdate(articleNumber, 0)
                calculateCheckoutSubtotal();
             }
        });
    });
}

// Load the initial cart items list
const intervalId = setInterval(() => {
    if (document.querySelectorAll(".co-cart-item").length > 0) {
        calculateCheckoutSubtotal();
        setupEventListeners();
        clearInterval(intervalId);
    }
}, 100);

document.addEventListener("DOMContentLoaded", () => {

    if (window.location.pathname === "/checkout") {
        const cartData = localStorage.getItem("cart");
        if (cartData) {
            fetch("/checkout/loadCartItems", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: cartData
            })
                .then(response => response.text())
                .then(html => {
                    document.getElementById("checkout-productlist-wrapper").innerHTML = html;
                })
                .catch(error => console.error("Error loading cart items:", error));
        }
    }
});
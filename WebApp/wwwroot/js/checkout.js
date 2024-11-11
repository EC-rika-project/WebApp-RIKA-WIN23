
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
function formatCheckoutPrice(value) {
    return `${value.toFixed(2)} Sek`;
}

function calculateCheckoutSubtotal() {
    console.log("subtotal körs")
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

function calculateCheckoutShipping(coSubtotal) {
    const coShippingElement = document.querySelector(".co-shipping");
    let coShippingCost = coSubtotal > 1000 || coSubtotal === 0 ? 0 : 50;
    coShippingElement.textContent = formatCheckoutPrice(coShippingCost);
    calculateCheckoutTotal(coSubtotal, coShippingCost)
}

function calculateCheckoutTotal(coSubtotal, coShippingCost) {
    const coTotalElement = document.querySelector(".co-cart-total");
    let coTotal = coSubtotal + coShippingCost
    coTotalElement.textContent = formatCheckoutPrice(coTotal)
}

function saveCheckoutCartToLocalStorage() {
    console.log("save")
    const cartItems = [];
    document.querySelectorAll(".co-cart-item").forEach(item => {
        const name = item.querySelector(".co-title").textContent;
        const ingress = item.querySelector(".co-ingress").textContent;
        const price = parseFloat(item.getAttribute("data-co-price"));
        const quantity = parseInt(item.querySelector(".qty-number p").textContent);
        cartItems.push({ name, price, ingress, quantity });
    });
    localStorage.setItem("cart", JSON.stringify(cartItems));

    loadCheckoutCartFromLocalStorage();
    loadCartFromLocalStorage();
}

function loadCheckoutCartFromLocalStorage() {
    const cartItems = JSON.parse(localStorage.getItem("cart"));
    const checkoutProductListWrapper = document.getElementById("checkout-productlist-wrapper");

    checkoutProductListWrapper.innerHTML = '';

    if (cartItems) {
        cartItems.forEach(item => {
            const cartItemElement = document.createElement("article");
            cartItemElement.classList.add("co-cart-item");
            cartItemElement.dataset.coPrice = item.price;

            cartItemElement.innerHTML += `
                <div class="image-wrapper">
                    <img src="https://picsum.photos/200" alt="" class="image" />
                </div>
                <div class="text-grid">
                    <h5 class="h5 co-title">${item.name}</h5>
                    <div class="co-delete">
                        <i class="fa-regular fa-trash"></i>
                    </div>
                    <p class="body text co-ingress">${item.ingress || ""}</p>
                    <p class="h6 co-price">${formatCheckoutPrice(item.price * item.quantity)}</p>
                    <div class="quantity bg-gray">
                        <button class="qty-btn co-decrease">-</button>
                        <div class="qty-number">
                            <p class="body co-quantity">${item.quantity}</p>
                        </div>
                        <button class="qty-btn co-increase">+</button>
                    </div>
                </div>
                <input type="hidden" name="Products[${item.articleNumber}].Name" value="${item.name}" />
            `;

            checkoutProductListWrapper.appendChild(cartItemElement);
        });
        calculateCheckoutSubtotal();
        setupEventListeners();
    }
}

function setupEventListeners() {
    document.querySelectorAll(".qty-btn").forEach(button => {
        button.addEventListener("click", () => {
            const item = button.closest(".co-cart-item");
            const quantityElement = item.querySelector(".co-quantity");
            let quantity = parseInt(quantityElement.textContent);

            if (button.classList.contains("co-increase")) {
                quantity++;
            } else if (button.classList.contains("co-decrease") && quantity > 1) {
                quantity--;
            }

            quantityElement.textContent = quantity;
            calculateCheckoutSubtotal();
            saveCheckoutCartToLocalStorage();
        });
    });

    document.querySelectorAll('.co-delete').forEach(deleteButton => {
        deleteButton.addEventListener('click', function (event) {
            const cartItem = event.target.closest('.co-cart-item');
            if (cartItem) {
                cartItem.remove();
                calculateCheckoutSubtotal();
                saveCheckoutCartToLocalStorage();
            }
        });
    });
}


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
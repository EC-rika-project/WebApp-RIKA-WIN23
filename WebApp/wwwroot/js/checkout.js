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

function saveCartToLocalStorage() {
    console.log("save")
    const cartItems = [];
    document.querySelectorAll(".co-cart-item").forEach(item => {
        const name = item.querySelector(".co-title").textContent;
        const ingress = item.querySelector(".co-ingress").textContent;
        const price = parseFloat(item.getAttribute("data-co-price"));
        const quantity = parseInt(item.querySelector(".qty-number p").textContent);
        cartItems.push({ name, price, ingress, quantity });
    });
    localStorage.setItem("cart", JSON.stringify(cartItems)); // Spara varukorgen till localStorage
}

function loadCartFromLocalStorage() {
    const cartItems = JSON.parse(localStorage.getItem("cartItems"));
    const checkoutProductListWrapper = document.getElementById("checkout-productlist-wrapper");

    if (cartItems) {
        cartItems.forEach(item => {
            const cartItemElement = document.createElement("article");
            cartItemElement.classList.add("co-cart-item");
            cartItemElement.dataset.coPrice = item.price;

            // Skapa innehållet för artikeln
            cartItemElement.innerHTML = `
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

            // Lägg till det skapade artikeln i checkout-productlist-wrapper
            checkoutProductListWrapper.appendChild(cartItemElement);
        });
        calculateCheckoutSubtotal();
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
            saveCartToLocalStorage();
        });
    });

    document.querySelectorAll('.co-delete').forEach(deleteButton => {
        deleteButton.addEventListener('click', function (event) {
            const cartItem = event.target.closest('.co-cart-item');
            if (cartItem) {
                cartItem.remove();
                calculateCheckoutSubtotal(); 
                saveCartToLocalStorage();
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
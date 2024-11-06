const openCartBtns = document.querySelectorAll(".cart-btn-open");
const closeCartBtns = document.querySelectorAll(".cart-btn-close");
const cart = document.querySelector(".cart");

// Function to toggle the cart on the site
function cartToggle() {
    if (cart.classList.contains("hide")) {
        cart.classList.remove("hide");
        cart.classList.add("show");
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
    //add fetch functionality
}

document.addEventListener("DOMContentLoaded", () => {
    const cartItems = document.querySelectorAll(".cart-item");
    const subtotalElement = document.querySelector(".sub-total");
    const shippingElement = document.querySelector(".shipping");
    const cartTotalElement = document.querySelector(".cart-total");
    const itemCountElement = document.querySelector(".total-items");

    function formatPrice(value) {
        return `${value.toFixed(2)} Sek`;
    }

    // Function to save cart to local storage
    function saveCartToLocalStorage() {
        const cartData = [];
        document.querySelectorAll(".cart-item").forEach(item => {
            const name = item.querySelector(".title").textContent;
            const ingress = item.querySelector(".ingress").textContent;
            const price = parseFloat(item.getAttribute("data-price"));
            const quantity = parseInt(item.querySelector(".qty-number p").textContent);
            cartData.push({ name, price, ingress, quantity });
        });
        localStorage.setItem("cart", JSON.stringify(cartData));
    }

    // Function to load the cart from local storage
    function loadCartFromLocalStorage() {
        let cartData = JSON.parse(localStorage.getItem("cart"));
        const cartContainer = document.querySelector(".cart-item-wrapper");

        if (!cartData || cartData.length === 0) {
            cartData = [
                { name: "Produkt 1", ingress: "ingress", price: 100, quantity: 2 },
                { name: "Produkt 2", ingress: "ingress", price: 200, quantity: 1 }
            ];
            localStorage.setItem("cart", JSON.stringify(cartData));
        }

        cartData = JSON.parse(localStorage.getItem("cart"));

        cartContainer.innerHTML = '';

        cartData.forEach(({ name, price, quantity, ingress }) => {
            cartContainer.innerHTML += `
            <article class="cart-item" data-price="${price}">
                <div class="image-wrapper">
                    <img src="https://picsum.photos/200" alt="" class="image" />
                </div>
                <div class="text-grid">
                    <h5 class="h5 title">${name}</h5>
                    <div class="delete">
                        <i class="fa-regular fa-trash"></i>
                    </div>
                    <p class="body ingress">${ingress}</p>
                    <p class="h6 price">${formatPrice(price * quantity)}</p>
                    <div class="quantity bg-gray">
                        <button class="qty-btn decrease">-</button>
                        <div class="qty-number">
                            <p class="body quantity">${quantity}</p>
                        </div>
                        <button class="qty-btn increase">+</button>
                    </div>
                </div>
            </article>
        `;
        });

        addEventListenersToCartItems();
    }

    // Function for calculate the total price in the cart
    function calculateTotal() {
        let subtotal = 0;
        let totalItems = 0;
        const updatedCartItems = document.querySelectorAll(".cart-item");
        updatedCartItems.forEach(item => {
            const price = parseFloat(item.getAttribute("data-price"));
            const quantity = parseInt(item.querySelector(".qty-number p").textContent);
            subtotal += price * quantity;
            totalItems += quantity;

            const totalPriceElement = item.querySelector(".price");
            totalPriceElement.textContent = formatPrice(price * quantity);
        });

        let shippingCost = subtotal > 1000 || subtotal == 0 ? 0 : 50;
        shippingElement.textContent = formatPrice(shippingCost);

        const totalWithShipping = subtotal + shippingCost;
        subtotalElement.textContent = formatPrice(subtotal);
        cartTotalElement.textContent = formatPrice(totalWithShipping);
        itemCountElement.textContent = `(${totalItems} items)`;

        saveCartToLocalStorage();
    }

    // Function for buttons in cart
    function addEventListenersToCartItems() {
        const cartItems = document.querySelectorAll(".cart-item");

        cartItems.forEach(item => {
            const decreaseBtn = item.querySelector(".decrease");
            const increaseBtn = item.querySelector(".increase");
            const quantityElement = item.querySelector(".qty-number p");
            const deleteBtn = item.querySelector(".delete");

            function updateButtonState(quantity) {
                if (quantity <= 1) {
                    decreaseBtn.style.visibility = "hidden";
                } else {
                    decreaseBtn.style.visibility = "visible";
                }
            }

            decreaseBtn.addEventListener("click", () => {
                let quantity = parseInt(quantityElement.textContent);
                if (quantity > 1) {
                    quantity--;
                    quantityElement.textContent = quantity;
                    updateButtonState(quantity);
                    calculateTotal();
                }
            });

            increaseBtn.addEventListener("click", () => {
                let quantity = parseInt(quantityElement.textContent);
                quantity++;
                quantityElement.textContent = quantity;
                updateButtonState(quantity);
                calculateTotal();
            });

            deleteBtn.addEventListener("click", () => {
                item.remove();
                calculateTotal();
            });

            updateButtonState(parseInt(quantityElement.textContent));
        });
    }
    
    loadCartFromLocalStorage();
    calculateTotal();
});

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












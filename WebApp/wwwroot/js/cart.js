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

// Close the cart if clicked outside
function closeCartOnClickOutside(event) {
    if (!cart.contains(event.target) && !event.target.closest(".cart-btn-open")) {
        cart.classList.remove("show");
        cart.classList.add("hide");
    }
}

window.addEventListener("load", function () {
    openCartBtns.forEach(btn => btn.addEventListener("click", cartToggle));
    closeCartBtns.forEach(btn => btn.addEventListener("click", cartToggle));
    document.addEventListener("click", closeCartOnClickOutside);
});

document.addEventListener("DOMContentLoaded", () => {
    const cartItems = document.querySelectorAll(".cart-item");
    const subtotalElement = document.querySelector(".sub-total");
    const shippingElement = document.querySelector(".shipping");
    const cartTotalElement = document.querySelector(".cart-total");
    const itemCountElement = document.querySelector(".total-items");

    function formatPrice(value) {
        return `${value.toFixed(2)} Sek`;
    }

    function saveCartUpdate(articleNumber, newQuantity) {
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
        calculateTotal();
    }

    // Function to load the cart from local storage
    function loadCartFromLocalStorage() {

        let cartData = JSON.parse(localStorage.getItem("cart"));
        const cartContainer = document.querySelector(".cart-item-wrapper");

        cartContainer.innerHTML = '';

        if (cartData) {
            cartData.forEach(({ product, size, quantity }) => {
                const { name, price, ingress, coverImageUrl, articleNumber } = product;

                    cartContainer.innerHTML += `
                    <article class="cart-item" data-price="${price}" data-articleNumber=${articleNumber}>
                        <div class="image-wrapper">
                            <img src="${coverImageUrl || 'https://picsum.photos/200'}" alt="${name}" class="image" />
                        </div>
                        <div class="text-grid">
                            <h5 class="h5 title">${name}</h5>
                            <div class="delete">
                                <i class="fa-regular fa-trash"></i>
                            </div>
                            <p class="body ingress">${ingress}</p>
                            <p class="h6 price">${formatPrice(price * quantity)}</p>
                            <p class="body size">Storlek: ${size}</p> 
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
        }

        addEventListenersToCartItems();
        calculateTotal();
        window.loadCartFromLocalStorage = loadCartFromLocalStorage;
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
                let articleNumber = item.getAttribute("data-articleNumber")

                if (quantity > 1) {
                    quantity--;
                    quantityElement.textContent = quantity;

                    saveCartUpdate(articleNumber, quantity)
                    updateButtonState(quantity);
                }
            });

            increaseBtn.addEventListener("click", () => {
                let quantity = parseInt(quantityElement.textContent);
                let articleNumber = item.getAttribute("data-articleNumber")
                quantity++;
                quantityElement.textContent = quantity;

                saveCartUpdate(articleNumber, quantity)
                updateButtonState(quantity);
            });

            deleteBtn.addEventListener("click", () => {
                let articleNumber = item.getAttribute("data-articleNumber")
                item.remove();

                saveCartUpdate(articleNumber, 0)
            });

            updateButtonState(parseInt(quantityElement.textContent));
        });
    }
    
    loadCartFromLocalStorage();
    calculateTotal();
});















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

document.addEventListener("DOMContentLoaded", () => {
    const cartItems = document.querySelectorAll(".cart-item");
    const subtotalElement = document.querySelector(".sub-total");
    const shippingElement = document.querySelector(".shipping");
    const cartTotalElement = document.querySelector(".cart-total");
    const itemCountElement = document.querySelector(".total-items");

    function formatPrice(value) {
        return `${value.toFixed(2)} Sek`;
    }

    //// Function to save cart to local storage
    //function saveCartToLocalStorage() {
    //    //const cartData = [];
    //    //document.querySelectorAll(".cart-item").forEach(item => {
    //    //    const name = item.querySelector(".title").textContent;
    //    //    const ingress = item.querySelector(".ingress").textContent;
    //    //    const price = parseFloat(item.getAttribute("data-price"));
    //    //    const quantity = parseInt(item.querySelector(".qty-number p").textContent);
    //    //    const size = item.querySelector(".size").textContent;
    //    //    const coverImageUrl = item.querySelector("img").src;
    //    //    const articleNumber = parseFloat(item.getAttribute("data-articleNumber"));
    //    //    cartData.push({ name, price, ingress, quantity, size, coverImageUrl, articleNumber });
    //    //});


    //    //localStorage.setItem("cart", JSON.stringify(cartData));
        
    //    document.querySelectorAll(".cart-item").forEach(item => {

    //        // Här kan vi skapa en produktstruktur för varje artikel
    //        const cartItem = {
    //            product: {
    //                articleNumber: item.getAttribute("data-articleNumber"),  // Förutsatt att artikelnr finns som en data-attribut
    //                //categoryName: item.getAttribute("data-category-name"),    // Förutsatt att kategorin finns som en data-attribut
    //                //color: item.getAttribute("data-color"),                   // Förutsatt att färg finns som en data-attribut
    //                coverImageUrl: item.querySelector("img").src,             // Hämtar bild-URL
    //                /*                    description: item.querySelector(".description").textContent,*/ // Förutsatt att beskrivning finns som en text
    //                //imageUrls: item.querySelectorAll(".image-wrapper img").map(img => img.src), // Om det finns flera bilder
    //                ingress: item.querySelector(".ingress").textContent,
    //                name: item.querySelector(".title").textContent,
    //                price: parseFloat(item.getAttribute("data-price")),
    //                //variations: [], // Lägg till logik för att hämta varianter om det behövs
    //            },
    //            size: item.querySelector(".size").textContent, // Förutsatt att storleken finns i ett element med klassen "size"
    //            quantity: parseInt(item.querySelector(".qty-number p").textContent),
    //        };

    //        // Lägg till objektet i cartData
    //        let cartData = JSON.parse(localStorage.getItem("cart")) || []; // Hämta befintlig data om den finns
    //        cartData.push(cartItem);

    //        // Spara den uppdaterade cartData tillbaka till Local Storage
    //        localStorage.setItem("cart", JSON.stringify(cartData));
    //    });





    //}

    function saveQuantityUpdate(articleNumber, newQuantity) {
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
        console.log("load cart")
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
                            <p class="body size">Storlek: ${size}</p> <!-- Lägg till storlek -->
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

        //saveCartToLocalStorage();
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

                    saveQuantityUpdate(articleNumber, quantity)

                    updateButtonState(quantity);
                    //calculateTotal();
                }
            });

            increaseBtn.addEventListener("click", () => {
                let quantity = parseInt(quantityElement.textContent);
                let articleNumber = item.getAttribute("data-articleNumber")
                quantity++;
                quantityElement.textContent = quantity;

                saveQuantityUpdate(articleNumber, quantity)

                updateButtonState(quantity);
                //calculateTotal();
            });

            deleteBtn.addEventListener("click", () => {
                let articleNumber = item.getAttribute("data-articleNumber")
                item.remove();

                saveQuantityUpdate(articleNumber, 0)

                //calculateTotal();
            });

            updateButtonState(parseInt(quantityElement.textContent));
        });
    }
    
    loadCartFromLocalStorage();
    calculateTotal();
});















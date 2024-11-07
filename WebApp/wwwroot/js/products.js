// Define product details globally so both openModal and addToCart can access it
const productDetailsList = {
    1: { id: "1", name: "Smartphone", ingress: "A great smartphone with advanced features.", description: "Full description of the smartphone.", price: 299.99, coverImageUrl: "/images/product1.jpg" },
    2: { id: "2", name: "Laptop", ingress: "Powerful laptop for all your needs.", description: "Full description of the laptop.", price: 899.99, coverImageUrl: "/images/product2.jpg" },
    3: { id: "3", name: "Wireless Earbuds", ingress: "High-quality sound in a small package.", description: "Full description of the earbuds.", price: 49.99, coverImageUrl: "/images/product3.jpg" },
    4: { id: "4", name: "Running Shoes", ingress: "Comfortable and stylish running shoes.", description: "Full description of the running shoes.", price: 99.99, coverImageUrl: "/images/product4.jpg" },
    5: { id: "5", name: "Wireless Headphones", ingress: "Experience high-quality sound without the wires.", description: "Full description of the headphones.", price: 129.99, coverImageUrl: "/images/product5.jpg" },
    6: { id: "6", name: "Smartwatch", ingress: "Track your fitness and stay connected.", description: "Full description of the smartwatch.", price: 199.99, coverImageUrl: "/images/product6.jpg" }
};

// Function to open modal with product details
function openProductModal(articleNumber) {
    const productDetails = productDetailsList[articleNumber];

    // Update modal with product details
    document.getElementById("productImage").src = productDetails.coverImageUrl;
    document.getElementById("productName").innerText = productDetails.name;
    document.getElementById("productIngress").innerText = productDetails.ingress;
    document.getElementById("productDescription").innerText = productDetails.description;
    document.getElementById("productPrice").innerText = `Price: $${productDetails.price.toFixed(2)}`;

    // Set the product ID on the favorite and cart buttons
    document.getElementById("favoriteButton").setAttribute("data-id", articleNumber);
    document.getElementById("cartButton").setAttribute("data-id", articleNumber);

    // Check if the product is a favorite
    const favoriteButton = document.getElementById("favoriteButton");
    if (localStorage.getItem(`favorite_${articleNumber}`)) {
        favoriteButton.classList.add("favorite");
    } else {
        favoriteButton.classList.remove("favorite");
    }

    // Open the modal
    document.getElementById("productModal").style.display = "block";
}

function closeProductModal() {
    document.getElementById("productModal").style.display = "none";

    // Remove 'selected' class from all size buttons
    document.querySelectorAll('.btn-modal-size').forEach(btn => {
        btn.classList.remove('selected');
    });
}

function selectSize(button) {
    // Remove 'selected' class from all size buttons
    document.querySelectorAll('.btn-modal-size').forEach(btn => {
        btn.classList.remove('selected');
    });
    // Add 'selected' class to the clicked button
    button.classList.add('selected');
}

// Function to add a product to the cart
function addToCart(button) {
    const productId = button.getAttribute("data-id");

    // Retrieve product details dynamically based on the product ID
    const productDetails = productDetailsList[productId];

    // Get the current cart from localStorage or create a new one
    let cart = JSON.parse(localStorage.getItem("cart")) || [];

    // Check if product is already in the cart
    const existingProduct = cart.find(item => item.id === productId);

    if (existingProduct) {
        // If product is already in the cart, increase quantity
        existingProduct.quantity += 1;
    } else {
        // If product is new to the cart, add it with an initial quantity
        cart.push({ ...productDetails, quantity: 1 });
    }

    // Save the updated cart to localStorage
    localStorage.setItem("cart", JSON.stringify(cart));
}

// Toggle the 'favorite' class on the favorite button for the specific product
function toggleFavorite(button) {
    const productId = button.getAttribute("data-id");

    // Retrieve product details dynamically based on the product ID
    const productDetails = productDetailsList[productId];

    // Get the current favorites from localStorage or create a new empty array
    let favorites = JSON.parse(localStorage.getItem("favorites")) || [];

    // Check if the product is already in favorites
    const productIndex = favorites.findIndex(item => item.id === productId);

    if (productIndex === -1) {
        // Add to favorites if it's not already in the list
        favorites.push({ ...productDetails, quantity: 1 });
        button.classList.add("favorite");
    } else {
        // Remove from favorites if the product is already in the list
        favorites.splice(productIndex, 1);
        button.classList.remove("favorite");
    }

    // Save the updated favorites array to localStorage
    localStorage.setItem("favorites", JSON.stringify(favorites));
}


// Initialize favorite buttons on page load
document.addEventListener("DOMContentLoaded", function () {
    const favoriteButtons = document.querySelectorAll(".btn-like");

    favoriteButtons.forEach(button => {
        const productId = button.getAttribute("data-id");

        // Check local storage for the favorite state
        if (localStorage.getItem(`favorite_${productId}`)) {
            button.classList.add("favorite");
        } else {
            button.classList.remove("favorite");
        }
    });
});

function openSortModal() {
    /* Open the modal*/
    document.getElementById("sortModal").style.display = "block";

}

function closeSortModal() {
    document.getElementById("sortModal").style.display = "none";
}
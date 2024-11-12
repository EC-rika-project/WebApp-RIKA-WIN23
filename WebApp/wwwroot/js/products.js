const colorMap = {
    "Svart": "#000000",      
    "Vit": "#FFFFFF",       
    "Grå": "#808080",      
    "Beige": "#F5F5DC",     
    "Ljusblå": "#ADD8E6",  
    "Mörkgrå": "#505050",   
    "Marinblå": "#000033",  
    "Brun": "#5C4033",  
};

// Cache object to store fetched product details to minimize the fetching from the api, gets removed on page reload
const productCache = {};

async function fetchProductDetails(articleNumber) {
    // Check if the product details are already cached
    if (productCache[articleNumber]) {
        return productCache[articleNumber]; // Return cached details if available
    }

    try {
        const baseUrl = "https://rika-apiproducts.azurewebsites.net";
        const response = await fetch(`${baseUrl}/products/${articleNumber}`);

        if (!response.ok) {
            throw new Error("Failed to fetch product details");
        }

        const productDetails = await response.json();

        // Cache the product details for future use
        productCache[articleNumber] = productDetails;

        return productDetails;
    } catch (error) {
        console.error("Error fetching product details:", error);
        return null;
    }
}

async function openProductModal(articleNumber) {

    // Fetch product details from the server
    const productDetails = await fetchProductDetails(articleNumber);
    const product = productDetails.product;

    // Sort the size variations
    const sortedSizes = sortSizes(product.variations);

    // Populate size buttons
    const sizeContainer = document.querySelector(".modal-size");
    sizeContainer.innerHTML = ''; // Clear existing buttons

    sortedSizes.forEach(size => {
        const button = document.createElement("button");
        button.classList.add("btn-modal-size");
        button.textContent = size.name;

        // Check if the size is "onesize" and add a unique class
        if (size.name.toLowerCase() === "onesize") {
            button.classList.add("btn-onesize");
        }

        // Add the 'out-of-stock' class if stock is 0
        if (size.stock === 0) {
            button.classList.add("out-of-stock");
        }

        // Add the 'out-of-stock' class if it's a "onesize" and stock is 0
        if (size.name.toLowerCase() === "onesize" && size.stock === 0) {
            button.classList.add("out-of-stock");
        }

        button.onclick = () => selectSize(button); // Attach the click handler
        sizeContainer.appendChild(button);
    });


    // Update other modal content
    document.getElementById("productImage").src = product.coverImageUrl;
    document.getElementById("productName").innerText = product.name;
    document.getElementById("productIngress").innerText = product.ingress;
    document.getElementById("productDescription").innerText = product.description;
    document.getElementById("articleNumber").innerText = `Art.nr: ${product.articleNumber}`;
    document.getElementById("productPrice").innerText = `Price: $${product.price}`;

    // Set the product ID on the favorite and cart buttons
    document.getElementById("favoriteButton").setAttribute("data-id", articleNumber);
    document.getElementById("cartButton").setAttribute("data-id", articleNumber);

    // Open the modal
    document.getElementById("productModal").style.display = "block";

    // Update the favorite button
    updateFavoriteButton(articleNumber);

    // Populate color buttons with main product and variants
    populateColorButtons(productDetails);
    
}

function isProductFavorite(articleNumber) {
    const favorites = JSON.parse(localStorage.getItem("favorites")) || [];

    return favorites.some(fav => {
        return fav.product.articleNumber === articleNumber.toString();
    });
}

function updateFavoriteButton(articleNumber) {
    const favoriteButton = document.getElementById("favoriteButton");

    if (isProductFavorite(articleNumber)) {
        favoriteButton.classList.add("favorite");
    } else {
        favoriteButton.classList.remove("favorite");
    }
}

function closeProductModal() {
    document.getElementById("productModal").style.display = "none";

    // Remove 'selected' class from all size buttons
    document.querySelectorAll('.btn-modal-size').forEach(btn => {
        btn.classList.remove('selected');
    });
}

function selectSize(button) {

    // Check if the button is out of stock
    if (button.classList.contains("out-of-stock")) {
        return; // Do nothing if it's out of stock
    }

    // Find the previously selected button
    const selectedButton = document.querySelector('.btn-modal-size.selected');

    // If a button was previously selected, remove the 'selected' class
    if (selectedButton && selectedButton !== button) {
        selectedButton.classList.remove('selected');
    }

    // Add the 'selected' class to the clicked button
    button.classList.add('selected');
}

// Function to add a product to the cart
async function addToCart(button) {
    const productId = button.getAttribute("data-id");

    // Fetch product details from the server
    const productDetails = await fetchProductDetails(productId);
    const product = productDetails.product;

    // Get selected size
    const selectedSizeButton = document.querySelector(".btn-modal-size.selected");
    if (!selectedSizeButton) {
        showNotification("Please select a size before adding to the cart.", "error");
        return;
    }
    const selectedSize = selectedSizeButton.textContent;

    // Get the current cart from localStorage or create a new one
    let cart = JSON.parse(localStorage.getItem("cart")) || [];

    // Check if product with selected size is already in the cart
    const existingProduct = cart.find(
        item => item.product && item.product.articleNumber === product.articleNumber && item.size === selectedSize 
    );

    if (existingProduct) {
        // If product is already in the cart, increase quantity
        existingProduct.quantity += 1;
    } else {
        // If product is new to the cart, add it with an initial quantity of 1
        cart.push({ product: product, size: selectedSize, quantity: 1 });
    }

    // Save the updated cart to localStorage
    localStorage.setItem("cart", JSON.stringify(cart));
    showNotification("Product added to cart!", "success");

    loadCartFromLocalStorage();

}

// Toggle the 'favorite' class on the favorite button for the specific product
async function toggleFavorite(button) {
    const productId = button.getAttribute("data-id");

    // Fetch product details from the server
    const productDetails = await fetchProductDetails(productId);
    const product = productDetails.product;

    // Get the current favorites from localStorage or create a new empty array
    let favorites = JSON.parse(localStorage.getItem("favorites")) || [];

    // Check if the product is already in the favorites list
    const productIndex = favorites.findIndex(item => item.product.articleNumber === product.articleNumber);

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

    // Ensure the favorite button updates immediately
    updateFavoriteButton(productId);
}

// Initialize favorite buttons on page load
document.addEventListener("DOMContentLoaded", function () {
    const favoriteButtons = document.querySelectorAll(".btn-like");

    favoriteButtons.forEach(button => {
        const productId = button.getAttribute("data-id");

        // Check if the product is in the favorites list
        let favorites = JSON.parse(localStorage.getItem("favorites")) || [];
        const isFavorite = favorites.some(item => item.product.articleNumber === productId);

        if (isFavorite) {
            button.classList.add("favorite"); 
        } else {
            button.classList.remove("favorite"); 
        }
    });
});

function openSortModal() {
    document.getElementById("sortModal").style.display = "block";
}

function closeSortModal() {
    document.getElementById("sortModal").style.display = "none";
}

// Helper function to show a notification
function showNotification(message, type) {
    const notification = document.getElementById("notification");
    notification.textContent = message;
    notification.className = `notification ${type}`; // Adds 'success' or 'error' class
    notification.style.display = "block";

    // Hide the notification after a few seconds
    setTimeout(() => {
        notification.style.display = "none";
    }, 4000); // Adjust the duration as needed
}

// Function to populate color buttons in the modal
function populateColorButtons(productDetails) {
    const colorContainer = document.querySelector(".modal-color");
    colorContainer.innerHTML = '';  // Clear existing color buttons

    // Create the color button for the main product
    const mainColorButton = createColorButton(productDetails.product);
    colorContainer.appendChild(mainColorButton);

    // Create color buttons for each variant
    productDetails.variants.forEach(variant => {
        const colorButton = createColorButton(variant);
        colorContainer.appendChild(colorButton);
    });
}

// Function to create a color button with proper background color
function createColorButton(variant) {
    const button = document.createElement("button");
    button.classList.add("btn-modal-color");

    // Use color mapping or fallback to variant color
    const color = colorMap[variant.color] || variant.color;
    button.style.backgroundColor = color;

    // Set button click to update modal content with this variant
    button.onclick = () => updateModalContent(variant);

    // Optional: Tooltip with color name for accessibility
    button.title = variant.color;

    return button;
}

function updateModalContent(variant) {
    // Update the modal with the selected variant's product details
    document.getElementById("productImage").src = variant.coverImageUrl;
    document.getElementById("productName").innerText = variant.name;
    document.getElementById("productIngress").innerText = variant.ingress;
    document.getElementById("productDescription").innerText = variant.description;
    document.getElementById("articleNumber").innerText = `Art.nr: ${variant.articleNumber}`;
    document.getElementById("productPrice").innerText = `Price: $${variant.price}`;

    // Update favorite and cart buttons with new product ID
    document.getElementById("favoriteButton").setAttribute("data-id", variant.articleNumber);
    document.getElementById("cartButton").setAttribute("data-id", variant.articleNumber);

    // Update favorite button status for this variant
    updateFavoriteButton(variant.articleNumber);

    // Update size buttons based on the selected color variant
    updateSizeButtons(variant);
}

// Use the sortSizes function inside your code where needed
function updateSizeButtons(product) {
    const sizeContainer = document.querySelector(".modal-size");
    sizeContainer.innerHTML = ''; // Clear existing buttons

    // Sort variations before displaying
    const sortedVariations = sortSizes(product.variations || []);

    sortedVariations.forEach(size => {
        const button = document.createElement("button");
        button.classList.add("btn-modal-size");
        button.textContent = size.name;

        if (size.name.toLowerCase() === "onesize") {
            button.classList.add("btn-onesize");
        }
        if (size.stock === 0) {
            button.classList.add("out-of-stock");
        }
        button.onclick = () => selectSize(button);
        sizeContainer.appendChild(button);
    });
}

// Helper function to sort sizes
function sortSizes(variations) {
    // Custom order for letter-based sizes
    const sizeOrder = ["XS", "S", "M", "L", "XL", "XXL"];

    return variations.sort((a, b) => {
        const aSize = a.name;
        const bSize = b.name;

        // Check if sizes are in custom order
        if (sizeOrder.includes(aSize) && sizeOrder.includes(bSize)) {
            return sizeOrder.indexOf(aSize) - sizeOrder.indexOf(bSize);
        }

        // Sort numerical sizes
        const aIsNumber = !isNaN(aSize);
        const bIsNumber = !isNaN(bSize);

        if (aIsNumber && bIsNumber) {
            return parseInt(aSize) - parseInt(bSize);
        }

        // Prioritize numerical sizes over letter-based sizes
        return aIsNumber ? -1 : 1;
    });
}





document.addEventListener('DOMContentLoaded', renderFavorites);

async function renderFavorites() {
    const favoritesContainer = document.querySelector('.product-grid');

    // Retrieve favorites from localStorage
    const favorites = JSON.parse(localStorage.getItem('favorites')) || [];

    // Clear the container first
    favoritesContainer.innerHTML = '';

    if (favorites.length === 0) {
        // Remove the 'product-grid' class if no favorites
        favoritesContainer.classList.remove('product-grid');

        favoritesContainer.innerHTML = '<p class="items-notadded">No favorites added yet.</p>';
        return;
    } else {
        // Add the 'product-grid' class if there are favorites
        favoritesContainer.classList.add('product-grid');
    }

    // Loop through each favorite entry
    for (const favorite of favorites) {
        // Access the `product` object in each favorite entry
        const product = favorite.product;

        if (product) {
            // Create product item structure
            const productItem = document.createElement('div');
            productItem.classList.add('product-item');
            productItem.setAttribute('onclick', `openProductModal('${product.articleNumber}')`);

            productItem.innerHTML = `
                <img src="${product.coverImageUrl}" alt="${product.name}" />
                <button id="favoriteButton_${product.articleNumber}" class="btn-like" data-id="${product.articleNumber}" onclick="toggleFavorite(this); event.stopPropagation();">
                    <i class="fa-solid fa-heart"></i>
                </button>
                <h2>${product.name}</h2>
                <p>${product.ingress}</p>
                <p style="font-weight:bold;">$${product.price}</p>
            `;

            favoritesContainer.appendChild(productItem);
        }
    }
}



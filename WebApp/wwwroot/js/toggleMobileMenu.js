const openBtn = document.getElementById("mobile-menu-btn-open");
const closeBtn = document.getElementById("mobile-menu-btn-close");
const menu = document.getElementById("mobile-menu");

function menuToggle() {
    if (menu.classList.contains("hide")) {
        menu.classList.remove("hide");
        menu.classList.add("show");
    } else {
        menu.classList.remove("show");
        menu.classList.add("hide");
    }
}

window.addEventListener("load", function () {
    openBtn.addEventListener("click", menuToggle);
    closeBtn.addEventListener("click", menuToggle);
});
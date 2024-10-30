
//Toggle password funktion för sign up
function togglePassword(fieldId, button) {
        const passwordField = document.getElementById(fieldId);
    const icon = button.querySelector("i");

    if (passwordField.type === "password") {
        passwordField.type = "text";
    icon.classList.remove("fa-eye");
    icon.classList.add("fa-eye-slash");
        } else {
        passwordField.type = "password";
    icon.classList.remove("fa-eye-slash");
    icon.classList.add("fa-eye");
        }
}

// Overlay för terms and condition på sign up
function showOverlay(type) {
    document.getElementById("termsText").style.display = "none";
    document.getElementById("conditionsText").style.display = "none";

    if (type === 'terms') {
        document.getElementById("termsText").style.display = "block";
    } else if (type === 'conditions') {
        document.getElementById("conditionsText").style.display = "block";
    }

    document.getElementById("overlay").style.display = "flex";
}

function hideOverlay() {
    document.getElementById("overlay").style.display = "none";
}
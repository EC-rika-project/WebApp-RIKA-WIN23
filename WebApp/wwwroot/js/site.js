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
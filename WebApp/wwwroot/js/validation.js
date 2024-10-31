document.addEventListener('DOMContentLoaded', function () {
    const formErrorHandler = (element, validationResult, errorMessage = '') => {
        let spanElement = document.querySelector(`[data-valmsg-for="${element.name}"]`);

        if (validationResult) {
            element.classList.remove('input-validation-error');
            element.classList.add('input-validation-success');
            spanElement.classList.remove('field-validation-error', 'hidden');
            spanElement.classList.add('field-validation-valid');
            spanElement.innerHTML = '';
        }
        else {
            element.classList.remove('input-validation-success');
            element.classList.add('input-validation-error');
            spanElement.classList.remove('field-validation-valid', 'hidden');
            spanElement.classList.add('field-validation-error');
            spanElement.innerHTML = errorMessage;
        }
    };

    const textValidator = (element, minLength = 2) => {
        const value = element.value.trim();
        let errorMessage = '';
        let valid = true;

        if (value.length === 0) {
            errorMessage = element.dataset.valRequired;
            valid = false;
        } else if (value.length < minLength) {
            errorMessage = `At least ${minLength} characters is required`;
            valid = false;
        }

        formErrorHandler(element, valid, errorMessage);
    };

    const emailValidator = (element) => {
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const value = element.value.trim();
        let errorMessage = '';
        let valid = true;

        if (value.length === 0) {
            errorMessage = element.dataset.valRequired;
            valid = false;
        } else if (!regex.test(value)) {
            errorMessage = 'Your email address is invalid';
            valid = false;
        }

        formErrorHandler(element, valid, errorMessage);
    };

    const passwordValidator = (element) => {
        const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
        const value = element.value.trim();
        let errorMessage = '';
        let valid = true;

        if (value.length === 0) {
            errorMessage = element.dataset.valRequired;
            valid = false;
        } else if (!regex.test(value)) {
            errorMessage = 'Enter a valid password';
            valid = false;
        }

        formErrorHandler(element, valid, errorMessage);
    };

    const phonenumberValidator = (element) => {
        const regex = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/;
        const value = element.value.trim();
        let errorMessage = '';
        let valid = true;

        if (value.length === 0) {
            errorMessage = element.dataset.valRequired;
            valid = false;
        } else if (!regex.test(value)) {
            errorMessage = 'Your phonenumber is invalid';
            valid = false;
        }

        formErrorHandler(element, valid, errorMessage);
    };

    const postalcodeValidator = (element) => {
        const regex = /^[0-9a-zA-Z\s]{3,10}$/;
        const value = element.value.trim();
        let errorMessage = '';
        let valid = true;

        if (value.length === 0) {
            errorMessage = element.dataset.valRequired;
            valid = false;
        } else if (!regex.test(value)) {
            errorMessage = 'Enter a valid postal code';
            valid = false;
        }

        formErrorHandler(element, valid, errorMessage);
    };

    const cardNumberValidator = (element) => {
        const regex = /^[0-9]{16}$/;
        const value = element.value.trim().replace(/\s/g, '');
        let errorMessage = '';
        let valid = true;

        if (value.length === 0) {
            errorMessage = element.dataset.valRequired;
            valid = false;
        } else if (!regex.test(value)) {
            errorMessage = 'Enter a valid 16-digit card number';
            valid = false;
        }

        formErrorHandler(element, valid, errorMessage);
    };

    const expDateValidator = (element) => {
        const regex = /^(0[1-9]|1[0-2])\/([0-9]{2})$/;
        const value = element.value.trim();
        let errorMessage = '';
        let valid = true;

        if (value.length === 0) {
            errorMessage = element.dataset.valRequired;
            valid = false;
        } else if (!regex.test(value)) {
            errorMessage = 'Enter date as MM/YY';
            valid = false;
        }

        formErrorHandler(element, valid, errorMessage);
    };

    const cvvValidator = (element) => {
        const regex = /^[0-9]{3}$/;
        const value = element.value.trim();
        let errorMessage = '';
        let valid = true;

        if (value.length === 0) {
            errorMessage = element.dataset.valRequired;
            valid = false;
        } else if (!regex.test(value)) {
            errorMessage = 'Enter a valid 3-digit CVV';
            valid = false;
        }

        formErrorHandler(element, valid, errorMessage);
    };

    const checkboxValidator = (element) => {
        if (element.checked) {
            formErrorHandler(element, true);
        }
        else {
            formErrorHandler(element, false);
        }
    };

    let forms = document.querySelectorAll('form');

    forms.forEach(form => {
        let inputs = form.querySelectorAll('input, textarea, select');

        inputs.forEach(input => {
            if (input.dataset.val === 'true') {
                if (input.type === 'checkbox') {
                    input.addEventListener('change', (e) => {
                        checkboxValidator(e.target);
                    });
                }
                else {
                    input.addEventListener('keyup', (e) => {
                        if (input.name.includes("PostalCode")) {
                            postalcodeValidator(e.target);
                        }
                        else if (input.name.includes("PhoneNumber")) {
                            phonenumberValidator(e.target);
                        }
                        else if (input.type === 'email') {
                            emailValidator(e.target);
                        }
                        else if (input.type === 'password') {
                            passwordValidator(e.target);
                        }
                        else if (input.name.includes("CardNumber")) {
                            cardNumberValidator(e.target);
                        }
                        else if (input.name.includes("ExpiryDate")) {
                            expDateValidator(e.target);
                        }
                        else if (input.name.includes("CVV")) {
                            cvvValidator(e.target);
                        }
                        else {
                            textValidator(e.target);
                        }
                    });
                }
            }
        });
    });
});
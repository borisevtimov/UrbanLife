document.getElementById('profile-settings-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    const formData = new FormData(event.target);
    let isModelValid = true;

    const emailValidationUrl = new URL('https://localhost:7226/user/isEmailAddressUsed');
    emailValidationUrl.searchParams.append('email', formData.get('Email'));

    const passwordValidationUrl = new URL('https://localhost:7226/user/account/passwordAlreadyUsed');
    passwordValidationUrl.searchParams.append('email', formData.get('Email'));
    passwordValidationUrl.searchParams.append('password', formData.get('Password'));

    const emailResponse = await fetch(emailValidationUrl);
    const passwordResponse = await fetch(passwordValidationUrl);

    if (emailResponse.ok) {
        const emailUsed = await emailResponse.json();

        if (emailUsed) {

            if (formData.get('Email').length != 0) {
                document.querySelector('.error-field-email').textContent = 'Имейлът адресът вече е зает!';
                isModelValid = false;
            }
        }
    }
    if (passwordResponse.ok) {
        const passwordAlreadyUsed = await passwordResponse.json();

        if (passwordAlreadyUsed) {

            if (formData.get('Password').length != 0) {
                document.querySelector('.error-field-password').textContent = 'Вече използвате тази парола!';
                isModelValid = false;
            }
        }
    }
    if (isModelValid) {
        event.target.submit();
    }
});
document.getElementById('register-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    const formData = new FormData(event.target);

    const url = new URL('https://localhost:7226/user/isEmailAddressUsed');
    url.searchParams.append('email', formData.get('Email'));

    const response = await fetch(url);

    if (response.ok) {
        const emailIsUsed = await response.json();

        if (emailIsUsed) {

            if (formData.get('Email').length != 0) {
                document.querySelector('.error-field').textContent = 'Имейлът е използван!';
            }
        }
        else {
            event.target.submit();
        }
    }
});
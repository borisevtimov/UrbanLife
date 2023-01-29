document.getElementById('login-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    const formData = new FormData(event.target);

    const url = new URL('https://localhost:7226/user/checkLoginCredentials');
    url.searchParams.append('email', formData.get('Email'));
    url.searchParams.append('password', formData.get('Password'));

    const response = await fetch(url);

    if (response.ok) {
        const userExists = await response.json();

        if (!userExists) {

            if (formData.get('Email').length != 0 && formData.get('Password').length != 0) {
                document.querySelector('.error-field').textContent = 'Неправилен имейл адрес или парола!';
            }
        }
        else {
            event.target.submit();
        }
    }
});
document.querySelector('.payment-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    const formData = new FormData(event.target);

    const cardCredentialsUrl = new URL('https://localhost:7226/user/account/cardCredentialsAreValid');
    cardCredentialsUrl.searchParams.append('cardNumber', formData.get('CardNumber'));
    cardCredentialsUrl.searchParams.append('expireDate', formData.get('ExpireDate'));
    cardCredentialsUrl.searchParams.append('firstName', formData.get('FirstName'));
    cardCredentialsUrl.searchParams.append('lastName', formData.get('LastName'));
    cardCredentialsUrl.searchParams.append('amount', formData.get('Amount'));
    cardCredentialsUrl.searchParams.append('cvc', formData.get('CVC'));

    const cardAddedUrl = new URL('https://localhost:7226/user/account/cardAlreadyAdded');
    cardAddedUrl.searchParams.append('cardNumber', formData.get('CardNumber'));

    const cardCredentialsResponse = await fetch(cardCredentialsUrl);
    const cardAddedResponse = await fetch(cardAddedUrl);

    if (cardCredentialsResponse.ok && cardAddedResponse.ok) {
        const cardCredentialsAreValid = await cardCredentialsResponse.json();
        const cardAlearyAdded = await cardAddedResponse.json();

        if (!cardCredentialsAreValid) {

            if (formData.get('CardNumber').length != 0) {

                if (cardAlearyAdded) {
                    document.querySelector('.error-field-card-added').textContent = 'Вече сте добавили тази карта!';
                    document.querySelector('.error-field-card-credentials').textContent = '';
                }
                else {
                    document.querySelector('.error-field-card-credentials').textContent = 'Попълнени са некоректни данни!';
                    document.querySelector('.error-field-card-added').textContent = '';
                }
            }
        }
        else {
            event.target.submit();
        }
    }
});
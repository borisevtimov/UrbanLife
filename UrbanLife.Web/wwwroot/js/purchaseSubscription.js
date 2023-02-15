// GET THE INITIAL FUNDS
addEventListener('load', async function () {
    const funds = this.document.querySelector('.funds');

    if (funds != null && funds.textContent.length == 0) {
        const payment = this.document.querySelector('.payment-method');
        getFunds(payment.value);
    }
});

// GET FUNDS ON VALUE CHANGE
const paymentMethod = document.querySelector('.payment-method');

if (paymentMethod != null) {
    paymentMethod.addEventListener('change', async function (event) {
        getFunds(event.target.value);
    });
}

// UPDATE THE PRICE AND LINES WHEN DURATION IS CHANGED
document.querySelector('.duration').addEventListener('change', async function () {
    const linesList = document.querySelector('.lines-list');

    const selectedOptionsValues = addSelectedLines(linesList);

    if (selectedOptionsValues.length != 0) {
        document.querySelector('.hidden-chosen-lines').value = selectedOptionsValues;
        calculateTotalPrice(selectedOptionsValues);
    }
});

// ADDS LINES AND CALCULATES PRICE ON LOAD
addEventListener('load', async function () {
    const linesList = this.document.querySelector('.lines-list');

    const selectedOptionsValues = addSelectedLines(linesList);

    if (selectedOptionsValues.length != 0) {
        document.querySelector('.hidden-chosen-lines').value = selectedOptionsValues;
        calculateTotalPrice(selectedOptionsValues);
    }
});

// ADDS LINES TO PARAGRAPH
document.querySelector('.lines-list').addEventListener('change', async function (event) {
    const selectedOptionsValues = addSelectedLines(event.target);
    document.querySelector('.hidden-chosen-lines').value = selectedOptionsValues;

    calculateTotalPrice(selectedOptionsValues);
});

function addSelectedLines(linesList) {
    const chosenLinesParagraph = document.querySelector('.chosen-lines');

    const options = linesList.options;
    let selectedOptions = [];
    let selectedOptionsValues = [];

    for (const option of options) {
        if (option.selected) {
            selectedOptions.push(option.textContent);
            selectedOptionsValues.push(option.value);
        }
    }

    chosenLinesParagraph.textContent = selectedOptions.join(', ');
    return selectedOptionsValues.join(', ');
}

async function calculateTotalPrice(selectedOptionsValues) {
    const subscriptionType = document.querySelector('.subscription-type');
    const totalPriceParagraph = document.querySelector('.total-price');
    const chosenDuration = document.querySelector('.duration');

    const url = new URL('https://localhost:7226/subscription/getTotalPrice');
    url.searchParams.append('subscriptionType', subscriptionType.value);
    url.searchParams.append('lines', selectedOptionsValues);
    url.searchParams.append('duration', chosenDuration.value);

    const response = await fetch(url);

    if (response.ok) {
        const totalPrice = await response.json();

        if (!Number.isNaN(totalPrice)) {
            totalPriceParagraph.textContent = `Обща сума: ${totalPrice.toFixed(2)} лв.`;
            document.querySelector('.final-hidden-price').value = `${totalPrice.toFixed(2)}`;
        }
    }
}

async function getFunds(paymentNumber) {
    const fundsParagraph = document.querySelector('.funds');

    const url = new URL('https://localhost:7226/subscription/getFundsForPayment');
    url.searchParams.append('cardNumber', paymentNumber);

    const response = await fetch(url);

    if (response.ok) {
        const paymentFunds = await response.json();

        if (!Number.isNaN(paymentFunds)) {

            fundsParagraph.textContent = `Налична сума: ${paymentFunds.toFixed(2)} лв.`;
        }
    }
}
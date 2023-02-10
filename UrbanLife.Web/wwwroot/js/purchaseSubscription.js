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

document.querySelector('.lines-list').addEventListener('change', function (event) {
    const options = event.target.options;
    let selectedOptions = [];

    for (const option of options) {
        if (option.selected) {
            selectedOptions.push(option.textContent);
        }
    }

    document.querySelector('.chosen-lines').value = selectedOptions.join(', ');
});

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
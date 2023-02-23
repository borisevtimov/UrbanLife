// CHANGES THE DURATION TO 1 DAY IF SELECTED ALL-LINES
document.querySelector('.lines-list').addEventListener('change', function () {
    checkDurationLineCombination();
});

// CHANGES THE LINES TO ALL-LINES IF DURATION IS 1 DAY
document.querySelector('.duration').addEventListener('change', function () {
    checkDurationLineCombination();
});

// CHECKS IF PAYMENT METHOD IS DEFAULT
addEventListener('load', async function () {
    await checkIfPaymentIsDefault();
});

// GET THE INITIAL FUNDS
addEventListener('load', async function () {
    const funds = this.document.querySelector('.funds');

    if (funds != null && funds.textContent.length == 0) {
        const payment = this.document.querySelector('.payment-method');
        const availableFunds = await getFunds(payment.value);
        const totalPrice = Number(this.document.querySelector('.final-hidden-price').value);
        const insufficientFunds = this.document.querySelector('.insufficient-funds');

        if (availableFunds != undefined && availableFunds < totalPrice) {
            insufficientFunds.textContent = 'Нямате достатъчно средства!';
            this.document.querySelector('.submit-btn').disabled = true;
        }
        else if (availableFunds != undefined && availableFunds >= totalPrice) {
            insufficientFunds.textContent = '';
            this.document.querySelector('.submit-btn').disabled = false;
        }
    }
});

// GET FUNDS ON VALUE CHANGE
const paymentMethod = document.querySelector('.payment-method');

if (paymentMethod != null) {
    paymentMethod.addEventListener('change', async function (event) {
        await checkIfPaymentIsDefault();
        await checkForInsufficientFunds();
    });
}

// UPDATE THE PRICE AND LINES WHEN DURATION IS CHANGED
document.querySelector('.duration').addEventListener('change', async function () {
    const linesList = document.querySelector('.lines-list');

    const selectedOptionsValues = addSelectedLines(linesList);

    if (selectedOptionsValues.length != 0) {
        document.querySelector('.hidden-chosen-lines').value = selectedOptionsValues;
        calculateTotalPrice(selectedOptionsValues);

        await checkForInsufficientFunds();
    }
});

// ADDS LINES AND CALCULATES PRICE ON LOAD
addEventListener('load', async function () {
    const linesList = this.document.querySelector('.lines-list');

    const selectedOptionsValues = addSelectedLines(linesList);

    if (selectedOptionsValues.length != 0) {
        document.querySelector('.hidden-chosen-lines').value = selectedOptionsValues;
        calculateTotalPrice(selectedOptionsValues);

        await checkForInsufficientFunds();
    }
});

// ADDS LINES TO PARAGRAPH
document.querySelector('.lines-list').addEventListener('change', async function (event) {
    const selectedOptionsValues = addSelectedLines(event.target);
    document.querySelector('.hidden-chosen-lines').value = selectedOptionsValues;

    calculateTotalPrice(selectedOptionsValues);
    await checkForInsufficientFunds();
});

async function checkIfPaymentIsDefault() {
    const paymentMethod = document.querySelector('.payment-method');

    if (paymentMethod != null) {
        const url = new URL('https://localhost:7226/subscription/checkIfPaymentIsDefault');
        url.searchParams.append('cardNumber', paymentMethod.value);

        const response = await fetch(url);

        if (response.ok) {
            const isDefault = await response.json();

            if (isDefault) {
                document.querySelector('.default-payment').style.display = 'inline';
            }
            else {
                document.querySelector('.default-payment').style.display = 'none';
            }
        }
    }
}

function checkDurationLineCombination() {
    const subscriptionType = document.querySelector('.subscription-type');

    if (subscriptionType.value == 'TICKET') {
        const linesList = document.querySelector('.lines-list');
        const duration = document.querySelector('.duration');
        const invalidCombination = document.querySelector('.invalid-combination');
        const submitBtn = document.querySelector('.submit-btn');

        if (linesList.value == 'all-lines' && duration.value == 'one-way') {
            invalidCombination.textContent = 'Еднократният билет е в комбинация само с избрана линия!';
            submitBtn.disabled = true;
        }
        else if (linesList.value != 'all-lines' && duration.value != 'one-way') {
            invalidCombination.textContent = 'Билетите за време са в комбинация само с всички линии!';
            submitBtn.disabled = true;
        }
        else if ((linesList.value != 'all-lines' && duration.value == 'one-way') || (linesList.value == 'all-lines' && duration.value != 'one-way')) {
            invalidCombination.textContent = '';

            if (document.querySelector('.insufficient-funds').textContent.length == 0) {
                submitBtn.disabled = false;
            }
        }

        return submitBtn.disabled;
    }
}

async function checkForInsufficientFunds() {
    const payment = document.querySelector('.payment-method');

    if (payment != null) {
        const availableFunds = await getFunds(payment.value);
        const totalPrice = Number(document.querySelector('.final-hidden-price').value);
        const insufficientFunds = document.querySelector('.insufficient-funds');
        const submitBtn = document.querySelector('.submit-btn');

        if (availableFunds != undefined && availableFunds < totalPrice) {
            insufficientFunds.textContent = 'Нямате достатъчно средства!';
            submitBtn.disabled = true;
        }
        else if (availableFunds != undefined && availableFunds >= totalPrice) {
            insufficientFunds.textContent = '';

            if (!checkDurationLineCombination()) {
                submitBtn.disabled = false;
            }
        }
    }
}

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

    if (selectedOptionsValues.length != 0) {
        const url = new URL('https://localhost:7226/subscription/getTotalPrice');
        url.searchParams.append('subscriptionType', subscriptionType.value);
        url.searchParams.append('lines', selectedOptionsValues);
        url.searchParams.append('duration', chosenDuration.value);

        const response = await fetch(url);

        if (response.ok) {
            const resultObj = await response.json();

            if (!Number.isNaN(resultObj.totalPrice)) {
                totalPriceParagraph.textContent = `Обща сума: ${resultObj.totalPrice.toFixed(2)} лв.`;
                document.querySelector('.final-hidden-price').value = `${resultObj.totalPrice.toFixed(2)}`;
            }

            if (resultObj.cheaperOptionMsg != '') {
                document.querySelector('.cheaper-option').textContent = resultObj.cheaperOptionMsg;
            }
            else {
                document.querySelector('.cheaper-option').textContent = '';
            }
        }
    }
    else {
        totalPriceParagraph.textContent = `Обща сума: 0.00 лв.`;
        document.querySelector('.final-hidden-price').value = 0.00;
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

        return paymentFunds;
    }
}
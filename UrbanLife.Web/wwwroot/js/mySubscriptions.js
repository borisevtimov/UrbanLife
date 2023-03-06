addEventListener('load', function () {
    const linesList = this.document.getElementById('lines-filter');
    const previouslySelectedLine = this.document.getElementById('chosen-line');

    if (previouslySelectedLine.value != '') {
        linesList.value = previouslySelectedLine.value;
    }
});

document.querySelectorAll('.filter-input').forEach(input => input.addEventListener('change', function () {
    updateState();
}));

document.getElementById('purchase-filter').addEventListener('click', function (event) {
    const icon = event.target.children[0];

    if (icon.classList.contains('fa-arrow-down')) {
        icon.classList.replace('fa-arrow-down', 'fa-arrow-up');
    }
    else {
        icon.classList.replace('fa-arrow-up', 'fa-arrow-down');
    }

    updateState();
});

function updateState() {
    const lines = document.getElementById('lines-filter');
    const valid = document.getElementById('valid-filter');
    const purchaseDate = document.getElementById('purchase-filter');
    const currentPage = document.getElementById('current-page');

    let isPurchaseDateDesc = true;

    if (purchaseDate.children[0].classList.contains('fa-arrow-up')) {
        isPurchaseDateDesc = false;
    }

    const url = new URL('https://localhost:7226/user/account/subscriptions');
    url.searchParams.append('page', currentPage.value);
    url.searchParams.append('purchaseDateDesc', isPurchaseDateDesc);
    url.searchParams.append('isValid', valid.checked);
    url.searchParams.append('line', lines.value);

    window.location.assign(url);
}
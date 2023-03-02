document.querySelector('.main-title').addEventListener('click', async function (event) {
    const url = new URL('https://localhost:7226/user/account/subscriptions?page=8');
    url.searchParams.append('page', 8);
    await fetch(url);
});
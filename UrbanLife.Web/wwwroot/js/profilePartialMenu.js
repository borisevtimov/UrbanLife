const aside = document.getElementById('profile-menu');

document.addEventListener('click', function (event) {
    if (event.target.classList.contains('profile-picture') && aside.style.display == 'none') {
        aside.style.display = 'block';
    }
    else if (event.target.tagName != 'ASIDE' && !event.target.classList.contains('profile-item')) {
        aside.style.display = 'none';
    }
});

document.addEventListener('load', function () {
    aside.style.display = 'none';
});
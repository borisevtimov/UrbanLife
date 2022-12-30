document.querySelector('.card-number-input').addEventListener('keydown', (event) => {
    const input = event.target;

    if (input.value.length == 15 && input.value.split('-').length < 3) {
        const numberParts = input.value.match(/.{1,4}/g);

        input.value = numberParts.join('-');
    }
});

document.querySelector('.cvc-eye-icon').addEventListener('click', (event) => {
    const parentDiv = event.target.parentNode.parentNode;

    if (parentDiv.children[1].type == 'text') {
        parentDiv.children[1].type = 'password';

        event.target.classList.remove('fa-eye');
        event.target.classList.add('fa-eye-slash');
    }
    else {
        parentDiv.children[1].type = 'text';

        event.target.classList.remove('fa-eye-slash');
        event.target.classList.add('fa-eye');
    }
});
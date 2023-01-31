document.querySelector('.card-number-input').addEventListener('keydown', (event) => {
    const input = event.target;

    if (event.keyCode != 8) {
        if (input.value.length == 4 || input.value.length == 9 || input.value.length == 14) {
            input.value += '-';
        }
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
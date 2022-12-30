const form = document.getElementById('login-form');
const username = document.getElementById('Login_AccountName');
const password = document.getElementById('Login_Password');
const button = document.getElementById('submit-button');

if (username.value) {
	username.parentElement?.classList.add('mud-shrink');
}

if (password.value) {
	password.parentElement?.classList.add('mud-shrink');
}

button.addEventListener('click', () => form.requestSubmit());
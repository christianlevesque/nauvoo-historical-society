const menuToggler = document.getElementById('menu-toggler');
const mainNav = document.getElementById('menu-drawer');

menuToggler.addEventListener('click', () => {
	mainNav.classList.toggle('mud-drawer--closed');
	mainNav.classList.toggle('mud-drawer--open');
})
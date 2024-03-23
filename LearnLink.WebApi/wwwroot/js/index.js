"use strict"

init();

function redirect(page) {
    let uri = document.location.origin;
    window.location.href = `${uri}/${page}`;
}

function scrollToAnchor(elementId) {
    var element = document.getElementById(elementId);
    element.scrollIntoView({ behavior: 'smooth' });
}



function init() {
    setupDropdownEventListeners();

    document.addEventListener('click', (e) => {
        const element = e.target.closest('.dropdown');

        if(!element) {
            deselectAllDropdowns();
        }
     });
}

function setupDropdownEventListeners() {
    const dropdowns = document.querySelectorAll('.dropdown');

    if(dropdowns.length == 0) return;

    dropdowns.forEach((dropdown) => {
        deselectDropdown(dropdown);
        dropdown.querySelector('.dropdown__head').addEventListener('click', dropdownClick);
    });
}

function deselectAllDropdowns() {
    const dropdowns = document.querySelectorAll('.dropdown');
    dropdowns.forEach((dropdown) => deselectDropdown(dropdown));
}

function deselectDropdown(dropdown) {
    dropdown.querySelector('.dropdown__items').style.display = 'none';
}

function dropdownClick(e) {
    const element = e.target.closest('.dropdown');

    if(!element) return;

    element.querySelector('.dropdown__items').style.display = element.querySelector('.dropdown__items').style.display === 'none'? 'block' : 'none';
}



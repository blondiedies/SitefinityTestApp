/**Sidebar menu*/

document.addEventListener("DOMContentLoaded", function () {
    const toggler = document.querySelector(".toggler");
    const menu = document.querySelector(".menu");
    const wrapBox = document.querySelector(".wrapBox");

    // Toggle menu + overlay when checkbox changes
    toggler.addEventListener("change", function () {
        if (toggler.checked) {
            menu.classList.add("menu--active");
            wrapBox.classList.add("wrapBox--active");
        } else {
            menu.classList.remove("menu--active");
            wrapBox.classList.remove("wrapBox--active");
        }
    });

    // Close menu when overlay is clicked
    wrapBox.addEventListener("click", function () {
        toggler.checked = false; // uncheck the checkbox
        menu.classList.remove("menu--active");
        wrapBox.classList.remove("wrapBox--active");
    });
});

document.addEventListener("DOMContentLoaded", function () {

    const nav = document.getElementById('nav-bar'),
        bodypd = document.getElementById('body-pd'),
        headerpd = document.getElementById('header');

    // Validate that all variables exist
    if (nav && bodypd && headerpd) {
        // Show navbar
        nav.classList.add('show');
        // Add padding to body
        bodypd.classList.add('body-pd');
        // Add padding to header
        headerpd.classList.add('body-pd');
    }

    const linkColor = document.querySelectorAll('.nav_link');
    function colorLink() {
        if (linkColor) {
            linkColor.forEach(l => l.classList.remove('active'))
            this.classList.add('active')
        }
    }
    linkColor.forEach(l => l.addEventListener('click', colorLink));

    // Your other code for page functionality
});





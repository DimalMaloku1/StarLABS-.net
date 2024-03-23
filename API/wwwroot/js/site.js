// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/*
let menuClose = document.getElementById('menu-btn');

menuClose.addEventListener('click', () => {

    let aside = document.getElementById('aside');
    let gridContainer = document.querySelector('.grid-container');
    let gridRight = document.querySelector('.grid-right');

    gridRight.classList.toggle('collapsed');
    gridContainer.classList.toggle('collapsed');
    aside.classList.toggle('remove');
})
*/
// for details on configuring this project to bundle and minify static web 

$(document).ready(function () {
    let table = $('.datatable').DataTable({
        responsive: true,
        columnDefs: [
            { type: 'string', targets: [0, 1] },  // String sorting for columns 0 and 1
            { type: 'date', targets: [3] },       // Date sorting for column 3
            { type: 'numeric', targets: [2] }     // Numeric sorting for column 2 (boolean)
        ]
    });
});
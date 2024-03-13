// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function sidebarData() {


    let sidebarObject = [
        {
            name: 'Home',
            controller: 'home',
            symbol: 'home'
        },
        {
            name: 'Bookings',
            controller: 'Bookings',
            symbol: 'bookmark'
        },
        {
            name: 'Feedback',
            controller: 'Feedback',
            symbol: 'reviews'
        },
        {
            name: 'Room',
            controller: 'Room',
            symbol: 'door_front'
        },
        {
            name: 'Room Type',
            controller: 'RoomType',
            symbol: 'king_bed'
        },
        {
            name: 'Users',
            controller: 'Users',
            symbol: 'group'
        },
        {
            name: 'Employees',
            controller: 'Staff',
            symbol: 'badge'
        },
        {
            name: 'Staff Positions',
            controller: 'Position',
            symbol: 'engineering'
        },
        {
            name: 'Payments',
            controller: 'Payment',
            symbol: 'paid'
        },
        {
            name: 'Bills',
            controller: 'Bill',
            symbol: 'card_membership'
        },

        //If you need to add a default symbol 'Help' ex. symbol: 'Help'

        {
            name: 'Rest',
            controller: 'Restarutantqqwewqed',
            symbol: 'help'
        },
    ]

    let asideList = document.getElementById('sidebar-list');
    let asideData = '';

    sidebarObject.forEach(item => {
        asideData += `
        <li>
           <a class="nav-link text-dark" href="/${item.controller}">
              <div class="content">
                  <div class="left-side-content">
                      <span class="material-icons md-60">${item.symbol}</span>
                      <p>${item.name}</p>
                  </div>
              </div>
            </a>
       </li>
    `;
    })

    asideList.innerHTML = asideData;
}

let menuClose = document.getElementById('menu-btn');

menuClose.addEventListener('click', () => {

    let aside = document.getElementById('aside');
    let gridContainer = document.querySelector('.grid-container');
    let gridRight = document.querySelector('.grid-right');

    gridRight.classList.toggle('collapsed');
    gridContainer.classList.toggle('collapsed');
    aside.classList.toggle('remove');
})
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
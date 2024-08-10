document.addEventListener('DOMContentLoaded', function () {
    // Lấy URL của trang hiện tại
    var currentPath = window.location.pathname;

    // Xóa lớp active-menu khỏi tất cả các mục menu
    document.querySelectorAll('.main-menu li').forEach(function (menuItem) {
        menuItem.classList.remove('active-menu');
    });

    // Thêm lớp active-menu vào mục menu tương ứng với URL hiện tại
    if (currentPath.includes('/home/index')) {
        document.getElementById('home-menu').classList.add('active-menu');
    } else if (currentPath.includes('/home/product')) {
        document.getElementById('shop-menu').classList.add('active-menu');
    } else if (currentPath.includes('/home/about')) {
        document.getElementById('about-menu').classList.add('active-menu');
    } else if (currentPath.includes('/home/contact')) {
        document.getElementById('contact-menu').classList.add('active-menu');
    }
});

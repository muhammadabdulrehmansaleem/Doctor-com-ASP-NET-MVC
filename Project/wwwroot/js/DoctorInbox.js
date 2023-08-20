//$(function () {
//    if ($('#ms-menu-trigger')[0]) {
//        $('body').on('click', '#ms-menu-trigger', function () {
//            $('.ms-menu').toggleClass('toggled');
//        });
//    }
//});
const chatContent = document.getElementById("chat-content");
const publisher = document.querySelector(".publisher");

chatContent.addEventListener("scroll", () => {
    if (chatContent.scrollTop + chatContent.clientHeight >= chatContent.scrollHeight) {
        // User has scrolled to the bottom
        publisher.classList.remove("fixed-bottom");
    } else {
        // User has scrolled up
        publisher.classList.add("fixed-bottom");
    }
});


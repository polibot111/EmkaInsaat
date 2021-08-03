$(document).ready(function () {
    $(window).scroll(function () {
        if (this.scrollY > 20) {
            $('.navbarr').addClass("sticky");
        } else {
            $('.navbarr').removeClass("sticky");
        }       
    });
    //toggle menu/navbar script
    $('.menu-btn').click(function () {
        $('.navbarr .menu').toggleClass("active");
        $('.menu-btn i').toggleClass("active");
    });


    var typed = new Typed(".typing_", {

        strings: ["Mimar", "Mühendis", "Müteahhit", "İnşaat", "Yapı"],      
        typeSpeed: 100,
        backSpeed: 60,
        loop: true,
       
    });


    $('#autoWidth').lightSlider({
        autoWidth: true,
        loop: true,
        onSliderLoad: function () {
            $('#autoWidth').removeClass('cS-hidden');
        }
    });
    $('#imageGallery').lightSlider({
        gallery: true,
        item: 1,
        loop: true,
        thumbItem: 9,
        slideMargin: 0,
        enableDrag: false,
        currentPagerPosition: 'left',
        onSliderLoad: function (el) {
            el.lightGallery({
                selector: '#imageGallery .lslide'
            });
        }
    });  

    
})
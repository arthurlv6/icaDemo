$(function () {
    $('#searchInput').bind('keypress', function (e) {
        if (e.keyCode == 13) {
            Search();
        }
    });
})

function Search() {
    var searchFor = $("#searchInput").val();
    window.location.href = '/home/search?input=' + searchFor;
}

$(function () {
    $('#status').fadeOut(); // will first fade out the loading animation
    $('#preloader').delay(350).fadeOut('slow'); // will fade out the white DIV that covers the website.
    $('body').delay(350).css({ 'overflow': 'visible' });

    new WOW().init();
    $("[data-header]").on("click", function (e) {
        e.preventDefault();
        var orderValue = $(this).attr("data-header-val");
        var orderDirection = $("#OrderDirection").val();
        if (orderDirection == "asc") {
            $("#OrderDirection").val("desc");
        } else {
            $("#OrderDirection").val("asc");
        }
        $("#ChangeOrderDirection").val(true)
        $("#orderBy").val(orderValue);
        var form = $("#searchform");
        //alert(orderValue);
        form.submit();
    });
});
function showTableDetail(e) {
    $(e).parent().parent().next().slideToggle('fast');
}

function resetSortDirectionAndSubmit() {
    $("#ChangeOrderDirection").val(false)
    $("#searchform").submit();
}

function OnSuccessReturn(data, status, xhr) {
    if (data == "fail") {
        //$("#messageModal").modal("show");
        showMessage("Failed", "Please check your input data and contact your administrator.");
    } else {
        //$("#messageModal").modal("show");
        //alert(data);
        showMessage("Normal", "Successfully save all the data.");
    }
}
function OnFailure(ajaxContext) {
    alert('Failure, please try later.');
}
$.ajaxSetup({ cache: false });

function showMessage(status, input) {
    var message = $("#message");
    $("#message span").html(input);
    message.css("margin-left", -parseInt(message.css("width")) / 2);

    if (status == "Normal") {
        if (!message.hasClass("alert-info")) {
            message.addClass("alert-info");
        }
        if (message.hasClass("alert-danger")) {
            message.removeClass("alert-danger");
        }
        $("#message b").html("Message");
        message.css("display", "block").delay(3000).fadeOut();
    } else {
        if (message.hasClass("alert-info")) {
            message.removeClass("alert-info");
        }
        if (!message.hasClass("alert-danger")) {
            message.addClass("alert-danger");
        }
        $("#message b").html("Alert!");
        message.css("display", "block");
    }
    return false;
}
function hideMessage() {
    var message = $("#message");
    message.css("display", "none");
}
$(function () {

    $('[data-toggle="tooltip"]').tooltip();

    function iframe_width(width) {
        $("#preview-iframe").animate({ width: width }, 500);
    }

    $("#display-full").click(function (e) {
        e.preventDefault();
        iframe_width("100%");
    });

    $("#display-940").click(function (e) {
        e.preventDefault();
        iframe_width("940px");
    });

    $("#display-480").click(function (e) {
        e.preventDefault();
        iframe_width("480px");
    });

    $("#remove-frame").click(function (e) {
        e.preventDefault();
        window.location.href = "http://almsaeedstudio.com/AdminLTE";
    });

    $(".ad").hide().delay(3000).show(500);

    $("#close-ad").click(function (e) {
        e.preventDefault();
        $(".ad").remove();
    });

});

//$(function () {
//    $('.navbar .dropdown').hover(function () {
//        $(this).find('.dropdown-menu').first().stop(true, true).slideDown();
//        $(this).addClass("active");
//    }, function () {
//        $(this).find('.dropdown-menu').first().stop(true, true).slideUp();
//        $(this).removeClass("active");
//    });
//})

// ADD SLIDEDOWN ANIMATION TO DROPDOWN //
$(function () {
    $('.dropdown').on('show.bs.dropdown', function (e) {
        $(this).find('.dropdown-menu').first().stop(true, true).slideDown();
    });

    // ADD SLIDEUP ANIMATION TO DROPDOWN //
    $('.dropdown').on('hide.bs.dropdown', function (e) {
        $(this).find('.dropdown-menu').first().stop(true, true).slideUp();
    });
})
function openModal() {
    $("body").css("overflow", "hidden");
    $("#fullModal").modal('show');
    $("#fullModal").css("padding-left", "0px");
}
function dismiss() {
    $("body").css("overflow", "visible");
}
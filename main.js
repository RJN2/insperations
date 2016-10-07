// JavaScript source code
$(function () {
    $("body").on("input propertychange", ".floating-label-form-group", function (e) {
        $(this).toggleClass("floating-label-form-group-with-value", !!$(e.target).val());
    }).on("focus", ".floating-label-form-group", function () {
        $(this).addClass("floating-label-form-group-with-focus");
    }).on("blur", ".floating-label-form-group", function () {
        $(this).removeClass("floating-label-form-group-with-focus");
    });
});

$('.pull-down').each(function () {
    var $this = $(this);
    var marginBottom = parseInt($this.css('margin-bottom'));
    console.log(marginBottom);
    console.log($this.parent().height() - $this.height() - marginBottom);
    $this.css('margin-top', $this.parent().height() - $this.height() - marginBottom);
});

$('textarea').keyup(updateCount);
$('textarea').keydown(updateCount);

function updateCount() {
    var cs = $(this).val().length;
    $('#counter').text(cs);
}
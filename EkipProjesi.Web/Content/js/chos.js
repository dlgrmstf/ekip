(function () {
    if ($("[class*='form-validation']")[0]) {
        $("[class*='form-validation']").validationEngine();
        console.log("form - validation");
        //Clear Prompt
        $('body').on('click', '.validation-clear', function (e) {
            e.preventDefault();
            $(this).closest('form').validationEngine('hide');
        });
    }
})();

(function (document, window, $) {
    'use strict';
    var Site = window.Site;
    $(document).ready(function () {
        Site.run();
    });
})(document, window, jQuery);
function basla() {
    $(".page").LoadingOverlay("show", {
        image: "",
        fontawesome: "fa fa-spinner fa-spin"
    });
}
function durdur() {
    $(".page").LoadingOverlay("hide");
}
$(".post").click(function () {
    $.LoadingOverlay("show", {
        image: "",
        fontawesome: "fa fa-spinner fa-spin"
    });
});
function tckontrol(field, rules, i, options) {
    value = field.val().toString();
    var isEleven = /^[0-9]{11}$/.test(value);
    var totalX = 0;
    for (var i = 0; i < 10; i++) {
        totalX += Number(value.substr(i, 1));
    }
    var isRuleX = totalX % 10 == value.substr(10, 1);
    var totalY1 = 0;
    var totalY2 = 0;
    for (var i = 0; i < 10; i += 2) {
        totalY1 += Number(value.substr(i, 1));
    }
    for (var i = 1; i < 10; i += 2) {
        totalY2 += Number(value.substr(i, 1));
    }
    var isRuleY = ((totalY1 * 7) - totalY2) % 10 == value.substr(9, 0);
    if (!isEleven || !isRuleX || !isRuleY) {
        return "* T.C. Kimlik Numarasını Kontrol Ediniz";
    }
}
$(document).ready(function () {
    //$(".post").click(function () {
    //    if ($(".form-validation").validationEngine('validate')) {
    //        $.LoadingOverlay("show", {
    //            image: "",
    //            fontawesome: "fa fa-spinner fa-spin"
    //        });
    //    }
    //});
    /* --------------------------------------------------------
Form Validation
-----------------------------------------------------------*/

});
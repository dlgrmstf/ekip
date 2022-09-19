(function () {
    //console.log(language);
    //$.extend(true, $.fn.dataTable.defaults, {
    //    "language": {
    //        "url": baseUrl + "Content/datatables-net/Turkish.lang.json",
    //    },
    //    //searching: false,
    //    //ordering: false
    //});
    //var letters = { "İ": "[İi]", "I": "[Iı]", "Ş": "[Şş]", "Ğ": "[Ğğ]", "Ü": "[Üü]", "Ö": "[Öö]", "Ç": "[Çç]", "i": "[İi]", "ı": "[Iı]", "ş": "[Şş]", "ğ": "[Ğğ]", "ü": "[Üü]", "ö": "[Öö]", "ç": "[Çç]" };
    jQuery.fn.DataTable.ext.type.search.string = function (data) {
        
        var testd = !data ?
            '' :
            typeof data === 'string' ?
                data
                    .replace(/I/g, 'ı')
                    .replace(/İ/g, 'i')
                    .replace(/üı/g, 'ÜI')
                    //.replace(/i/g, 'I')
                    :
                data;
                //data.replace(/(([İIŞĞÜÇÖiışğüçö]))/g, function (letter) { return letters[letter]; }) : data;
        return testd;
    };
    $(document).on('keyup', ".dataTables_filter input[type='search']", function () {
        //$(this).val($(this).val().toLowerCase());
        var id = $(this).closest('div').attr("id");
        id = id.replace("_filter", "");
        //console.log(id);
        var xtable = $("#" + id).DataTable();
        xtable.search(jQuery.fn.DataTable.ext.type.search.string(this.value)).draw();
    });
    $.fn.dataTable.Api.register('column().title()', function () {
        var colheader = this.header();
        return $(colheader).text().trim();
    });
}());
function datatableFooterSearch(selector) {//tfoot eklenmesi ve initComplete: function () {datatableFooterSearch(this);} içinde çağırılması gerekmektedir.
    selector.api().columns().every(function (i) {
        var column = this;
        var text = column.column(i).title();
        if (text != "") {
            //console.log(text);
            var select = $('<input class="datatable-input-sm" type="text" title="' + Lang.Ara + ' ' + text + '" placeholder="' + Lang.Ara + ' ' + text + '" data-index="' + i + '"/>').appendTo($(column.footer()).empty()).on('keyup', function () {
                var val = $.fn.dataTable.util.escapeRegex(
                    $(this).val()
                );
                column.search(val).draw();
            });
            var div = $('<div class="form-item">' + select + '</div>');
            //column.data().unique().sort().each(function (d, j) {
            //    select.append('<option value="' + d + '">' + d + '</option>');
            //});
        }
    });
}
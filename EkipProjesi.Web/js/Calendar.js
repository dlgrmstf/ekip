var events = [];
var selectedEvent = null;
FetchEventAndRenderCalendar();
function FetchEventAndRenderCalendar() {
    events = [];
    $.ajax({
        type: "GET",
        url: "@Url.Action("RandevuGetir","ArindirmaMerkezleri")",
        success: function (data) {
            $.each(data, function (i, v) {
                events.push({
                    eventID: v.ID,
                    description: v.Aciklama,
                    start: moment(v.RandevuBaslangicTarihi),
                    end: v.RandevuBitisTarihi != null ? moment(v.RandevuBitisTarihi) : null,
                    color: v.Renk,
                    allDay: v.TumGun,
                    phone: v.Telefon,
                    name: v.HastaAdi,
                    surname: v.HastaSoyadi,
                    tcno: v.HastaTCKimlikNo,
                    kurum: v.KurumID,
                    poliklinik: v.PoliklinikTuruID,
                    kurumid: v.KurumID,
                    polid: v.PoliklinikTuruID
                });
            })

            GenerateCalender(events);
        },
        error: function (error) {
            alert('Veriler getirilirken hata oluştu, lütfen tekrar deneyiniz.');
        }
    })
}

function GenerateCalender(events) {
    $('#calender').fullCalendar('destroy');
    $('#calender').fullCalendar({
        contentHeight: 400,
        defaultDate: new Date(),
        timeFormat: 'h(:mm)',
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,basicWeek,basicDay,agenda'
        },
        eventLimit: true,
        eventColor: '#378006',
        events: events,
        eventClick: function (calEvent, jsEvent, view) {
            selectedEvent = calEvent;
            $('#myModal #eventTitle').text(calEvent.RandevuBaslangicTarihi);
            var $description = $('<div/>');
            $description.append($('<p/>').html('<b>Randevu Başlangıç Tarihi: </b>' + calEvent.start.format("DD-MMM-YYYY HH:mm")));
            if (calEvent.end != null) {
                $description.append($('<p/>').html('<b>Randevu Bitiş Tarihi: </b>' + calEvent.end.format("DD-MMM-YYYY HH:mm")));
            }
            $description.append($('<p/>').html('<b>Açıklama: </b>' + calEvent.description));
            $description.append($('<p/>').html('<b>Hasta Adı: </b>' + calEvent.name));
            $description.append($('<p/>').html('<b>Hasta Soyadı: </b>' + calEvent.surname));
            $description.append($('<p/>').html('<b>Hasta TC No: </b>' + calEvent.tcno));
            $description.append($('<p/>').html('<b>Telefon: </b>' + calEvent.phone));
            $description.append($('<p/>').html('<b>Kurum ID: </b>' + calEvent.kurum));
            $description.append($('<p/>').html('<b>Poliklinik ID: </b>' + calEvent.poliklinik));

            $('#myModal #pDetails').empty().html($description);

            $('#myModal').modal();
        },
        selectable: true,
        select: function (start, end) {
            selectedEvent = {
                eventID: 0,
                description: '',
                start: start,
                end: end,
                allDay: false,
                color: '',
                name: '',
                surname: '',
                phone: '',
                tcno: '',
                kurum: 0,
                poliklinik: 0
            };
            openAddEditForm();
            $('#calendar').fullCalendar('unselect');
        },
        editable: true,
        eventDrop: function (event) {
            var data = {
                ID: event.eventID,
                RandevuBaslangicTarihi: event.start.format('DD/MM/YYYY HH:mm'),
                RandevuBitisTarihi: event.end != null ? event.end.format('DD/MM/YYYY HH:mm') : null,
                Aciklama: event.description,
                Renk: event.color,
                TumGun: event.allDay,
                HastaAdi: event.name,
                HastaSoyadi: event.surname,
                HastaTCKimlikNo: evet.tcno,
                Telefon: event.phone,
                KurumID: event.kurum,
                PoliklinikTuruID: event.poliklinik
            };
            SaveEvent(data);
        }
    })
}

$('#btnEdit').click(function () {
    //Open modal dialog for edit event
    openAddEditForm();
})
$('#btnDelete').click(function () {
    if (selectedEvent != null && confirm('Randevuyu silmek istediğinize emin misiniz?')) {
        $.ajax({
            type: "POST",
            url: '@Url.Action("RandevuSil", "ArindirmaMerkezleri")',
            data: { 'eventID': selectedEvent.eventID },
            success: function (data) {
                if (data.status) {
                    //Refresh the calender
                    FetchEventAndRenderCalendar();
                    $('#myModal').modal('hide');
                }
            },
            error: function () {
                alert('Failed');
            }
        })
    }
})
$('#dtp1,#dtp2').datetimepicker({
    format: 'DD/MM/YYYY HH:mm'
});
$('#chkIsFullDay').change(function () {
    if ($(this).is(':checked')) {
        $('#divEndDate').hide();
    }
    else {
        $('#divEndDate').show();
    }
});
function openAddEditForm() {
    if (selectedEvent != null) {
        $('#hdEventID').val(selectedEvent.eventID);
        $('#txtStart').val(selectedEvent.start.format('DD/MM/YYYY HH:mm'));
        $('#chkIsFullDay').prop("checked", selectedEvent.allDay || false);
        $('#chkIsFullDay').change();
        $('#txtEnd').val(selectedEvent.start != null ? selectedEvent.start.format('DD/MM/YYYY HH:mm') : '');
        $('#txtDescription').val(selectedEvent.description);
        $('#ddThemeColor').val(selectedEvent.color);
        $('#txtName').val(selectedEvent.name);
        $('#txtSurname').val(selectedEvent.surname);
        $('#txtTCNo').val(selectedEvent.tcno);
        $('#txtPhone').val(selectedEvent.phone);
        $('#txtKurum').val(selectedEvent.kurum);
        $('#txtPoliklinik').val(selectedEvent.poliklinik);
    }
    $('#myModal').modal('hide');
    $('#myModalSave').modal();
}
$('#btnSave').click(function () {
    //Validation/
    if ($('#txtName').val().trim() == "") {
        alert('Lütfen Hasta Adı Giriniz.');
        return;
    }
    if ($('#txtSurname').val().trim() == "") {
        alert('Lütfen Hasta Soyadı Giriniz.');
        return;
    }
    if ($('#txtPhone').val().trim() == "") {
        alert('Lütfen Hasta Telefonu Giriniz.');
        return;
    }
    if ($('#txtKurum').val().trim() == "") {
        alert('Lütfen Kurum ID Giriniz.');
        return;
    }
    if ($('#txtPoliklinik').val().trim() == "") {
        alert('Lütfen Poliklinik ID Giriniz.');
        return;
    }
    if ($('#txtStart').val().trim() == "") {
        alert('lütfen Randevu Başlangıç Saatini Giriniz.');
        return;
    }
    if ($('#chkIsFullDay').is(':checked') == false && $('#txtEnd').val().trim() == "") {
        alert('Lütfen Randevu Bitiş Saatini Giriniz.');
        return;
    }
    else {
        var startDate = moment($('#txtStart').val(), "DD/MM/YYYY HH:mm").toDate();
        var endDate = moment($('#txtEnd').val(), "DD/MM/YYYY HH:mm").toDate();
        if (startDate > endDate) {
            alert('Geçersiz Bitiş Tarihi');
            return;
        }
    }

    var data = {
        ID: $('#hdEventID').val(),
        RandevuBaslangicTarihi: $('#txtStart').val().trim(),
        RandevuBitisTarihi: $('#chkIsFullDay').is(':checked') ? 0 : $('#txtEnd').val().trim(),
        Aciklama: $('#txtDescription').val(),
        Renk: $('#ddThemeColor').val(),
        TumGun: $('#chkIsFullDay').is(':checked'),
        HastaAdi: $('#txtName').val().trim(),
        HastaSoyadi: $('#txtSurname').val().trim(),
        HastaTCKimlikNo: $('#txtTCNo').val().trim(),
        Telefon: $('#txtPhone').val(),
        KurumID: $('#txtKurum').val(),
        PoliklinikTuruID: $('#txtPoliklinik').val()
    }
    SaveEvent(data);
    // call function for submit data to the server
})
function SaveEvent(data) {
    $.ajax({
        type: "POST",
        url: '@Url.Action("RandevuKaydet", "ArindirmaMerkezleri")',
        data: data,
        success: function (data) {
            if (data.status) {
                //Refresh the calender
                FetchEventAndRenderCalendar();
                $('#myModalSave').modal('hide');
            }
        },
        error: function () {
            alert('Kayıt sırasında hata oluştu.Lütfen tekrar deneyiniz.');
        }
    })
}
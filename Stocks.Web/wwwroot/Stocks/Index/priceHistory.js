connection.on('drawPriceHistory', (priceHistory, ticker) => {
    var array = new Array();
    for (var i = 0; i < priceHistory.length; i++) {
        array.push([
            priceHistory[i]['date'],
            priceHistory[i]['low'],
            priceHistory[i]['open'],
            priceHistory[i]['close'],
            priceHistory[i]['high']
        ]);
    }

    if (array.length > 0) {
        $('#symbol').text(ticker['symbol']);
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(function () {
            var data = new google.visualization.arrayToDataTable(array, true);
            var chart = new google.visualization.CandlestickChart(document.getElementById('priceHistory'));
            chart.draw(data);
        });
    }

    else {
        // no data
    }

    $('#priceHistoryDatePicker').prop('disabled', false);
    $('#priceHistoryDatePickerStartDate').prop('disabled', false);
    $('#priceHistoryDatePickerEndDate').prop('disabled', false)
});

$('#priceHistoryDatePicker').datepicker({
    autoclose: true,
    endDate: new Date()
});

$('#priceHistoryDatePicker').on('change', () => {
    var priceHistoryDateRange = {
        start: $('#priceHistoryDatePickerStartDate').val(),
        end: $('#priceHistoryDatePickerEndDate').val()
    };

    var tickerTableSelectedRowData = $('#ticker').DataTable().row({ selected: true }).data();
    if (priceHistoryDateRange.start !== '' && priceHistoryDateRange.end !== '' && priceHistoryDateRange.start !== priceHistoryDateRange.end) {
        connection.invoke('GetPriceHistoryAsync', tickerTableSelectedRowData, priceHistoryDateRange);
    }
});

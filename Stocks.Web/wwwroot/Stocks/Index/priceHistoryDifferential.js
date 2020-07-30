connection.on('drawPriceHistoryDifferential', (priceHistoryDifferential) => {
    if ($.fn.dataTable.isDataTable($('#priceHistoryDifferential'))) {
        $('#priceHistoryDifferential').DataTable().destroy();
        $('#priceHistoryDifferential').empty();
    }

    $('#priceHistoryDifferential').DataTable({
        data: JSON.parse(priceHistoryDifferential),
        columns: [
            { title: 'Symbol', data: 'Symbol' },
            { title: 'Last Price (Close)', data: 'LastPriceClose' },
            { title: 'High Price (Average)', data: 'AveragePriceHigh' },
            { title: 'Low Price (Average)', data: 'AveragePriceLow' },
            { title: 'Price Differential', data: 'Differential' },
            { title: 'Days Lapsed (Last-High/Low)', data: 'DaysLapsedLast' },
            { title: 'Days Lapsed (High-Low)', data: 'DaysLapsedHighLow' }
        ],
        colReorder: {
            fixedColumnsLeft: 1
        },
        order: [
            [4, 'asc']
        ]
    });

    $('#priceHistoryDifferentialDatePicker').prop('disabled', false);
    $('#priceHistoryDifferentialDatePickerStartDate').prop('disabled', false);
    $('#priceHistoryDifferentialDatePickerEndDate').prop('disabled', false);
});

$('#priceHistoryDifferentialDatePicker').datepicker({
    autoclose: true,
    endDate: new Date()
});

var priceHistoryDifferentialDateRange = {
    start: $('#priceHistoryDifferentialDatePickerStartDate').val(),
    end: $('#priceHistoryDifferentialDatePickerEndDate').val()
};

$('#priceHistoryDifferentialDatePicker').on('change', () => {
    priceHistoryDifferentialDateRange.start = $('#priceHistoryDifferentialDatePickerStartDate').val();
    priceHistoryDifferentialDateRange.end = $('#priceHistoryDifferentialDatePickerEndDate').val();
    if (priceHistoryDifferentialDateRange.start !== '' && priceHistoryDifferentialDateRange.end !== '' && priceHistoryDifferentialDateRange.start !== priceHistoryDifferentialDateRange.end) {
        connection.invoke('GetPriceHistoryDifferentialAsync', priceHistoryDifferentialDateRange);
    }
});

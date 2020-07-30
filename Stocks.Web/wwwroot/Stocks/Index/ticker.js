var tickerTable = $('#ticker').DataTable({
    data: ticker,
    columns: [
        { title: 'Symbol', data: 'Symbol' },
        { title: 'Name', data: 'Name' },
        { title: 'Sector', data: 'Sector' },
        { title: 'Industry', data: 'Industry' },
        { title: 'IpoYear', data: 'IpoYear' },
        { title: 'MarketCap', data: 'MarketCap' },
        //{ title: 'LastSale', data: 'LastSale' },
        { title: 'Updated', data: 'Updated' }
    ],
    order: [
        [0, 'asc']
    ],
    colReorder: {
        fixedColumnsLeft: 1
    },
    select: {
        single: true,
        toggleable: false
    },
    autoWidth: false
});

tickerTable.on('select', (e, dt, type, indices) => {
    var priceHistoryDateRange = {
        start: $('#priceHistoryDatePickerStartDate').val(),
        end: $('#priceHistoryDatePickerEndDate').val()
    };

    var tickerTableSelectedRowData = tickerTable.row(indices[0]).data();
    if (priceHistoryDateRange.start !== '' && priceHistoryDateRange.end !== '' && priceHistoryDateRange.start !== priceHistoryDateRange.end) {
        connection.invoke('GetPriceHistoryAsync', tickerTableSelectedRowData, priceHistoryDateRange);
    }

    console.log('invoke: GetPriceHistoryAsync');
});

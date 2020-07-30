const connection = new signalR.HubConnectionBuilder()
    .withUrl(hubConnectionSettings.hubUrl)
    .withAutomaticReconnect()
    .configureLogging(hubConnectionSettings.logLevel)
    .build();

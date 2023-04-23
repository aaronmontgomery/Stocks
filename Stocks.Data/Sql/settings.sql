USE Stocks;

-- StocksApi
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('StocksApi', 'BaseUrl', 'https://localhost:44328');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('StocksApi', 'BeginDateTime', DATEFROMPARTS(2022, 1, 1));

-- TdAmeritradeApi
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'ApiKey', '');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'AuthenticationUrl', 'https://api.tdameritrade.com/v1/oauth2/token');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'AccountsUrl', 'https://api.tdameritrade.com/v1/accounts');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'MarketDataUrl', 'https://api.tdameritrade.com/v1/marketdata');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'OrdersUrl', 'https://api.tdameritrade.com/v1/orders');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'InstrumentsUrl', 'https://api.tdameritrade.com/v1/instruments');

-- TDAmeritrade Auth
--https://auth.tdameritrade.com/oauth?client_id= %40AMER.OAUTHAP&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%3A44328%2Fapi%2Fauthorization%2Fpostaccesstoken&lang=en-us
--https://localhost:44328/api/authorization/postaccesstoken

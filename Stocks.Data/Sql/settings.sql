USE Stocks;

-- TdAmeritradeApi
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'ApiKey', '');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'AuthenticationUri', 'https://api.tdameritrade.com/v1/oauth2/token');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'AccountsUri', 'https://api.tdameritrade.com/v1/accounts');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'MarketDataUri', 'https://api.tdameritrade.com/v1/marketdata');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('TdAmeritradeApi', 'OrdersUri', 'https://api.tdameritrade.com/v1/orders');

-- Exchanges
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('Exchange', 'Nasdaq', 'https://old.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nasdaq&render=download');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('Exchange', 'NYSE', 'https://old.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nyse&render=download');
INSERT INTO Setting ([Type], [Key], [Value]) VALUES ('Exchange', 'AMEX', 'https://old.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=amex&render=download');

-- TDAmeritrade Auth
--https://auth.tdameritrade.com/oauth?client_id= %40AMER.OAUTHAP&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%3A44397%2Fapi%2Fauthorization%2Fpostaccesstoken&lang=en-us

-- Symbol Lists
--ftp://ftp.nasdaqtrader.com/symboldirectory/nasdaqtraded.txt
--http://investexcel.net/all-yahoo-finance-stock-tickers/

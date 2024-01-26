

CREATE TABLE [User](
	[UserId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] VARCHAR(250) NOT NULL,
    [Age] INT NULL
);

CREATE TABLE [ExternalClientMessage](
	[ExternalClientMessageId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UserId] INT NOT NULL,
    [StatusCode] INT NOT NULL,
    [CreateDate] DATETIME NOT NULL
);

select * from [User] order by UserId desc

select * from [ExternalClientMessage] order by ExternalClientMessageId desc

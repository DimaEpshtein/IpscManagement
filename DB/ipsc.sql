CREATE TABLE Member (
    Id int Identity (1,1) not null,
    [Identity] int not null,
    FirstName nvarchar(255),
	LastName nvarchar(255),
	Email varchar(255),
	Phone varchar(15),
	MobilePhone varchar(15),
    Address nvarchar(255),
    City nvarchar(255),
	Active bit not null,
	Information nvarchar(max),
	ShooterIdentity int
);

CREATE TABLE BulletsStock (
    Id int Identity (1,1) not null,
    MemberId int not null,
	MemberIdentity int not null,
	Amount int not null DEFAULT(0)
);

CREATE TABLE BulletsStockHistory (
    Id int Identity (1,1) not null,
    MemberId int not null,
	MemberIdentity int not null,
	Amount int not null,
	ActionType int not null
);


ALTER TABLE Member add CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED([Id] ASC)
ALTER TABLE Member add CONSTRAINT [AK_Member] UNIQUE([Identity])

ALTER TABLE BulletsStock add CONSTRAINT [PK_BulletsStock] PRIMARY KEY CLUSTERED([Id] ASC)
ALTER TABLE BulletsStockHistory add CONSTRAINT [PK_BulletsStockHistory] PRIMARY KEY CLUSTERED([Id] ASC)

ALTER TABLE BulletsStock ADD FOREIGN KEY (MemberId) REFERENCES Member(Id);
ALTER TABLE BulletsStock ADD FOREIGN KEY (MemberIdentity) REFERENCES Member([Identity]);

ALTER TABLE [BulletsStockHistory] ADD [DateTime] DateTime not null
ALTER TABLE [Member] ADD [DateTime] DateTime not null
ALTER TABLE [BulletsStock] ADD [DateTime] DateTime not null
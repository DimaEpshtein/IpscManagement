
CREATE TABLE [dbo].[AmmoType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Identity] int NULL,
	[Name] nvarchar(50)  NOT NULL,
	[Remarks] nvarchar(256) NULL
	 CONSTRAINT [PK_AmmoType_Id] PRIMARY KEY CLUSTERED (Id),
	 CONSTRAINT UC_AmmoType_Identity UNIQUE ([Identity])
	)

ALTER TABLE [dbo].[BulletsStock]
ADD AmmoType int
CONSTRAINT [FK_BulletsStock_AmmoType] FOREIGN KEY (AmmoType) REFERENCES AmmoType([Id])

ALTER TABLE [dbo].[BulletsStockHistory]
ADD AmmoType int, 
Modifier nvarchar(128)


CREATE TABLE [dbo].[Warehouse](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Phone] [varchar](15) NULL,
	[Address] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[Active] [bit] NOT NULL,
	[Information] [nvarchar](max) NULL,
	[DateTime] [datetime] NOT NULL
	CONSTRAINT [PK_Warehouse_Id] PRIMARY KEY CLUSTERED (Id),
	
	)

	CREATE TABLE [dbo].[WarehouseBulletsStock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WarehouseId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[AmmoType] int not null DEFAULT ((0)),
	[DateTime] [datetime] NOT NULL
	CONSTRAINT [PK_WarehouseBulletsStock_Id] PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT [FK_WarehouseBulletsStock_WarehouseId] FOREIGN KEY (WarehouseId) REFERENCES Warehouse([Id]),
	CONSTRAINT [FK_WarehouseBulletsStock_AmmoType] FOREIGN KEY (AmmoType) REFERENCES AmmoType([Id])
	)

	CREATE TABLE [dbo].[WarehouseBulletsStockHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WarehouseId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[AmmoType] int not null DEFAULT ((0)),
	[ActionType] [int] NOT NULL,
	MemberId int null,
	Modifier nvarchar(128) null,
	[Remarks] [nvarchar](max) NULL,
	[DateTime] [datetime] NOT NULL,
	
 CONSTRAINT [PK_WarehouseBulletsStockHistory_Id] PRIMARY KEY CLUSTERED (Id)

 )
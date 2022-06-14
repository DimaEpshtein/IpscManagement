alter table [dbo].[Member]
  add Gender smallint not null DEFAULT(0),
  DateofBirth date not null default CURRENT_TIMESTAMP,
  FatherName nvarchar(256) null,
  ArmyId  int null,
  Zip varchar(128) null
  
  alter table [dbo].[WarehouseBulletsStockHistory]
  add PreviousAmmount int not null default(0),
  NewAmmount int not null default(0)
  
  alter table [dbo].[BulletsStockHistory]
  add PreviousAmmount int not null default(0),
  NewAmmount int not null default(0)
Use eBillingConfigurations
Create table WSCredentials
( ID int not null primary key,
  Username nvarchar(100) not null,
  Password nvarchar(255) not null,
  Domain nvarchar(255)
)
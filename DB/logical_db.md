@startuml


entity user{
* userID: integer <<generated>> <<pk>>
--
* firstName: text
* lastName: text
* email: text
* address: text
* number: integer
* city: text
* province: text
* country: text
complement: text
* postalCode: varchar(16)
description: text
}

entity station{
* StationID integer <<generated>> <<pk>>
* StationName text
* City text
* Province text
* Country text
}

entity payment{
* paymentID: integer <<generated>> <<pk>>
--
* method: varchar(8)
* amount: decimals(10,2)
* datetime: datetime <<default now>>
}

entity ticket{
* ticketID: integer <<generated>> <<pk>>
--
* category: varchar(18)
row: varchar(1)
seat: varchar(2)
description: text
last_updated: timestamp <<default now>>
}

entity trip{
* tripID integer <<generated>> <<pk>>
-- 
* StationID INT
* DepartureTime DATETIME
* ArrivalTime DATETIME
}

user " 1"  - "*" ticket

ticket "*" - "1" trip

trip "1" - "*" station

payment "1 " -- " * " ticket


@enduml
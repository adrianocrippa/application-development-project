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
--
* userID: <<fk>> user
}

entity ticket{
* ticketID: integer <<generated>> <<pk>>
--
* category: varchar(18)
row: varchar(1)
seat: varchar(2)
description: text
last_updated: timestamp <<default now>>
--
* userID: <<fk>> user
* paymentID: <<fk>> payment
* tripID: <<fk>> trip
}

entity trip{
* tripID integer <<generated>> <<pk>>
--
* StationID integer
* DepartureTime datetime
* ArrivalTime datetime
--
* stationID <<fk>> station
}

entity wallet{
* walletID: integer <<generated>> <<pk>>
--
* user_id: <<fk>> user
}

user "1 " -- " 1 " wallet

user "1" - " * " payment

user " 1"  - "*" ticket

ticket "  *" -- "1" trip

ticket "*" - "1" wallet

trip "1" - "*" station

payment "1 " -- " * " ticket


@enduml
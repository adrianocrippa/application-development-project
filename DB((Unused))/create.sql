drop schema if exists ticketAPP cascade;

create schema ticketAPP;
set search_path to ticketAPP;

create table "user"
(
    userID  integer generated always as identity
        primary key,
    first_name varchar(32) not null
        constraint firstname_check
            check (length((first_name)::text) >= 3),
    last_name varchar(32) not null
        constraint lastname_check
            check (length((last_name)::text) >= 1),
    email    text        not null
        unique
        constraint user_email_check
            check ((length(email) >= 6) AND (email ~~* '%@%.%'::text)),
    address text    not null,
    number  integer not null,
    complement text,
    city    text    not null,
    province    text    not null,
    postal_code varchar(16),
    country text    not null
);

create table "payment"
(
    paymentID  integer generated always as identity
        primary key,
    method varchar(8),
    amount decimal(10,2),
    datetime timestamp default now(),
    user_id     integer
        references "user"
);

create table "wallet"
(
    wallet_id  integer generated always as identity
        primary key,
    user_id     integer
        references "user"
);

create table "ticket"
(
    ticketID  integer generated always as identity
        primary key,
    category varchar(18) not null,
    row varchar(2),
    seat varchar(2),
    description text,
    last_updated timestamp default now(),
    paymentID     integer
        references "payment",
    userID     integer
        references "user",
    walletID integer
references wallet
);

--AT THIS POINT WE NEED TO IMPORT FORM CSV FILE.. DID ON DATA GRIP AND WAS VERY EASY
create table "station"
(
    stationID integer primary key,
    station_name text not null,
    station_city text not null,
    station_province text not null,
    station_country text not null,
station_code varchar(3) not null
);

create table "trip"
(
    tripID integer generated always as identity
        primary key,
    departure_date date,
    departure_time time,
    arrival_date date,
    arrival_time time,
    stationID integer references station,
    ticketID integer references ticket
);

--

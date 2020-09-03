-- create database hlsun
CREATE DATABASE hlsun;

-- create table student
-- 1: male  ,  2: famale  ,  3: unknow
create table student(
    id int auto_increment primary key not null,
    classid int not null default -1,
    gradeid int not null default -1,
    stunum varchar(15) not null unique,
    cardid varchar(18) not null unique,
    name varchar(20) not null,
    age int not null default 18,
    gender int not null default 1, 
    phonenum varchar(20) not null default "unknow",
    email varchar(50) not null default "unknow@emil.com",
    address varchar(50) not null default "unknow"
);

-- add column 
alter table student add column isdelete bit not null default 0 after address;

-- create table class
create table class(
    id int auto_increment primary key not null,
    classid int not null default 0,
    gradeid int not null default 0
); 

-- create table grade
create table grade(
    id int auto_increment primary key not null,
    schoolid varchar(8) not null unique,
    schoolname varchar(50) not null,
    gradeid int not null unique default 0
);

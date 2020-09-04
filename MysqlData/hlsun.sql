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
    address varchar(50) not null default "unknow",
	isdelete bit not null default 0
);

-- add column 
alter table student add column isdelete bit not null default 0 after address;

-- insert student data
insert into student(classid, gradeid, stunum, cardid, name, age, gender, phonenum, email, address, isdelete) values(1, 1, '100102020110001', '342201200201204892', 'zhansan', 18, 0, '1587920110', 'unknow@emil', '', 0);

-- create table class
create table class(
    id int auto_increment primary key not null,
    classid int not null default 0,
    gradeid int not null default 0
); 

-- insert class data
insert into class(classid, gradeid) values();

-- create table grade
create table grade(
    id int auto_increment primary key not null,
    schoolid varchar(8) not null unique,
    schoolname varchar(50) not null,
    gradeid int not null unique default 0
);

-- insert grade data
insert into grade(schoolid, schoolname, gradeid) values('10010001', 'an hui wang cai yuan', 1);


-- 添加外键约束
alter table class add foreign key (gradeid) references grade(gradeid);

alter table student add foreign key (gradeid) references grade(gradeid);
alter table student add foreign key (classid) references class(classid);

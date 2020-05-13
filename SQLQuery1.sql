--CREATE TABLE room_type(
--[type] varchar(6) primary key,
--price float
--)

--CREATE TABLE room(
--room_id varchar(4) primary key,
--[type] varchar(6) not null,
--remark varchar(100),
--FOREIGN KEY([type]) REFERENCES room_type([type]) on update cascade
--)

--CREATE TABLE customer(
--id varchar(18) primary key,
--name varchar(10) not null,
--sex varchar not null,
--tel varchar(15) not null,
--constraint c_sex check(sex in ('F','M'))
--)

--CREATE TABLE account(
--customer_id varchar(50) primary key,
--[password] varchar(50) not null,
--[user_name] varchar(50) not null,
--constraint psd_length check(len([password])>=6)
--)

--CREATE TABLE account_customer(
--customer_id varchar(50),
--id varchar(18),
--FOREIGN KEY(customer_id) REFERENCES account(customer_id) on delete cascade,
--FOREIGN KEY(id) REFERENCES customer(id) on delete cascade
--)

--CREATE TABLE [order](
--order_id varchar(12) primary key,
--[type] varchar(10) check([type] in ('will','ing','ed','canceled')),
--order_time datetime,
--price float,
--customer_id varchar(50) not null,
--room_id varchar(4) not null,
--in_date varchar(8),
--out_date varchar(8),
--id1 varchar(18) not null,
--id2 varchar(18),
--id3 varchar(18)
--FOREIGN KEY(customer_id) REFERENCES account(customer_id),
--FOREIGN KEY(room_id) REFERENCES room(room_id),
--FOREIGN KEY(id1) REFERENCES customer(id),
--FOREIGN KEY(id2) REFERENCES customer(id),
--FOREIGN KEY(id3) REFERENCES customer(id)
--)

--CREATE TABLE [stuff](
--recept_id varchar(50) primary key,
--name varchar(50) not null,
--[password] varchar(50) not null,
--position varchar(10) not null
--)

--CREATE TABLE order_num(
--order_date varchar(10),
--number int,
--)

--CREATE VIEW customer_view(room_id,price,in_time,out_time)
--AS
--SELECT room.room_id,room_type.price,in_date,out_date 
--FROM room,room_type,[order] WHERE room.[type]=room_type.[type] and room.room_id=[order].room_id
--WITH CHECK OPTION

--CREATE VIEW reception_view(order_id,room_id,in_date,out_date,single_price)
--AS
--SELECT order_id,room.room_id,in_date,out_date,room_type.price FROM room,room_type,[order]
--WHERE room.[type]=room_type.[type] and room.room_id=[order].room_id

--CREATE FUNCTION MD5
--    (
--      @src varchar(255) ,    -- 源字符串
--      @type INT = 32        -- 加密类型(16/32)，默认值32
--    )
--RETURNS varchar(255)
--    WITH EXECUTE AS CALLER
--AS
--    BEGIN
--        -- 存放md5加密串(ox)
--        DECLARE @smd5 varchar(34)
--        -- 加密字符串此处用MD5加密
--        SET @smd5 = sys.fn_VarBinToHexStr(HASHBYTES('MD5', @src));
--        IF @type = 16
--            SELECT  @smd5 = SUBSTRING(@smd5, 11, 16)   --16位
--        ELSE
--            SELECT  @smd5 = SUBSTRING(@smd5, 3, 32)    --32位
--        -- 返回加密串，转大写
--        RETURN UPPER(@smd5)
 
--    END
    
--CREATE TRIGGER trg_EncryptPwd ON account
--    AFTER INSERT, UPDATE
-- AS
--    BEGIN
--        IF ( UPDATE([password]) )
--            BEGIN
--                DECLARE @uId varchar(15)
--                DECLARE @uPassword varchar(32)
--                -- 获取用户ID和密码
--                SELECT  @uId = customer_id ,
--                        @uPassword = [password]
--                FROM    inserted

--                -- 更新密码
--                UPDATE  account
--                SET     [password] = dbo.MD5(@uPassword, 32)
--                WHERE   customer_id = @uId
--            END
--    END

--CREATE FUNCTION toOID
--    (     
--    )
--RETURNS varchar(255)
--    WITH EXECUTE AS CALLER
--AS
--    BEGIN
--        DECLARE @date_str varchar(8)
--        DECLARE @num varchar(3)
--        SET @date_str = (select CONVERT(varchar(8),GETDATE(),112))
--        set @num= (select COUNT(*) from order_num where order_date=(Select CONVERT(varchar(100), GETDATE(), 112)))
--        DECLARE @oid varchar(11)
--        if(@num>99)
--		SET @oid = @date_str+(SELECT CONVERT(varchar(3),@num))
--		else if(@num>9)
--		SET @oid = @date_str+'0'+(SELECT CONVERT(varchar(3),@num))
--		else
--		SET @oid = @date_str+'00'+(SELECT CONVERT(varchar(3),@num))
--		RETURN @oid
--    END

--CREATE TRIGGER trg_setoid ON [order]
--    AFTER INSERT
-- AS
--    BEGIN
--        INSERT order_num 
--        VALUES((Select CONVERT(varchar(100), GETDATE(), 112)),(select COUNT(*) from order_num where order_date=(Select CONVERT(varchar(100), GETDATE(), 112))))
--    END

--CREATE INDEX RNO_IDX
--ON room(room_id)

--CREATE INDEX OID_IDX
--ON [order](order_id)

--CREATE USER HOTEL_STUFF
--GRANT UPDATE,INSERT,DELETE,SELECT ON account
--TO HOTEL_STUFF
--GRANT UPDATE,INSERT,DELETE,SELECT ON [order]
--TO HOTEL_STUFF
--GRANT UPDATE,INSERT,DELETE,SELECT ON room
--TO HOTEL_STUFF
--GRANT UPDATE,INSERT,DELETE,SELECT ON room_type
--TO HOTEL_STUFF
--GRANT UPDATE,INSERT,DELETE,SELECT ON [stuff]
--TO HOTEL_STUFF

--CREATE USER CUSTOMER
--GRANT SELECT ON account
--TO CUSTOMER
--GRANT INSERT,DELETE,SELECT ON customer
--TO CUSTOMER
--GRANT INSERT,DELETE,SELECT ON customer
--TO CUSTOMER

--CREATE TRIGGER trg_EncryptStuffPwd ON [stuff]
--    AFTER INSERT, UPDATE
-- AS
--    BEGIN
--        IF ( UPDATE([password]) )
--            BEGIN
--                DECLARE @uId varchar(15)
--                DECLARE @uPassword varchar(32)
--                -- 获取用户ID和密码
--                SELECT  @uId = recept_id ,
--                        @uPassword = [password]
--                FROM    inserted

--                -- 更新密码
--                UPDATE  [stuff]
--                SET     [password] = dbo.MD5(@uPassword, 32)
--                WHERE   recept_id = @uId
--            END
--    END
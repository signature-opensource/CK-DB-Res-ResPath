--[beginscript]

create table CK.tResPath
(
	ResId int not null,
	ResPath varchar(128) collate Latin1_General_100_BIN2 not null,
	constraint PK_CK_ResPath primary key nonclustered (ResId),
	constraint FK_CK_ResPath_ResId foreign key (ResId) references CK.tRes( ResId )
);

create unique clustered index IX_CK_ResPath_ResPath on CK.tResPath( ResPath );

insert into CK.tResPath( ResId, ResPath ) values( 0, '' );
insert into CK.tResPath( ResId, ResPath ) values( 1, 'System' );

--[endscript]

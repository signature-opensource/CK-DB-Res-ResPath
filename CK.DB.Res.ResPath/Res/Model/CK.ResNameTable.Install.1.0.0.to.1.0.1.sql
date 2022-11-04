--[beginscript]

drop index IX_CK_ResPath_ResPath on CK.tResPath;

alter table CK.tResPath alter column ResPath varchar(128) collate Latin1_General_100_BIN2 not null;

create unique clustered index IX_CK_ResPath_ResPath on CK.tResPath( ResPath );

--[endscript]
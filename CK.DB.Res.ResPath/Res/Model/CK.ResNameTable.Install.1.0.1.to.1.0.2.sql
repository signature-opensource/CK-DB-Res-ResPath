--[beginscript]

alter table CK.tResPath drop constraint CK_FK_ResPath_ResId;

alter table CK.tResPath add constraint FK_CK_ResPath_ResId foreign key (ResId) references CK.tRes( ResId );

--[endscript]
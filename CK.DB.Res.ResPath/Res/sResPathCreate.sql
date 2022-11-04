-- SetupConfig:{}
--
-- Associates a name to an existing resource identifier.
--
create procedure CK.sResPathCreate
(
	@ResId int,
	@ResPath varchar(128)
)
as 
begin
	set @ResPath = RTrim( LTrim(@ResPath) );

	--[beginsp]

	--<PreCreate revert />

	insert into CK.tResPath( ResId, ResPath ) values( @ResId, @ResPath );
	
	--<PostCreate />	
	
	--[endsp]
end
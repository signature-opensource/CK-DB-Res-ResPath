-- SetupConfig: {}
--
-- Renames a resource by its resource identifier.
--
create procedure CK.sResPathRename
(
    @ResId int,
    @NewName varchar(128),
	@WithChildren bit = 1
)
as 
begin
	if @ResId <= 1 throw 50000, 'Res.NoRename', 1;
	
	declare @OldName varchar(128);
	select @OldName = ResPath from CK.tResPath where ResId = @ResId;

	if @OldName is not null 
	begin
		exec CK.sResPathRenameResPath @OldName, @NewName, @WithChildren;
	end
end

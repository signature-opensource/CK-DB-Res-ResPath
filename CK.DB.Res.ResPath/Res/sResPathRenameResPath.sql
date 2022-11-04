-- SetupConfig: {}
--
-- Renames a resource by its resource name.
--
create procedure CK.sResPathRenameResPath
(
    @OldName varchar(128),
    @NewName varchar(128),
	@WithChildren bit = 1
)
as 
begin
	declare @LenPrefix int;
	set @NewName = RTrim( LTrim(@NewName) );
	set @LenPrefix = len(@OldName)+1;
	if @LenPrefix is null or @LenPrefix = 0 throw 50000, 'Res.RootRename', 1;

	--[beginsp]

	--<PreRename revert />

	if @WithChildren = 1
	begin
		-- Updates child names first.
		update CK.tResPath set ResPath = @NewName + substring( ResPath, @LenPrefix, 128 )
			where ResPath like @OldName + '.%';
	end
	-- Updates the resource itself.
	update CK.tResPath set ResPath = @NewName where ResPath = @OldName;

	--<PostRename />
	
	--[endsp]
end

-- SetupConfig: {}
--
-- Destroys the resource name if it exists.
--
create procedure CK.sResPathDestroy
(
	@ResId int
)
as 
begin
	if @ResId <= 1 throw 50000, 'Res.Undestroyable', 1;

	--[beginsp]

	--<PreDestroy revert />

	delete from CK.tResPath where ResId = @ResId;
	
	--<PostDestroy />	
	
	--[endsp]
end
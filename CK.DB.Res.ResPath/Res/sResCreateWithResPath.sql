-- SetupConfig: { "Requires": [ "CK.sResPathCreate" ] }
--
-- Creates a resource with an initial ResPath.
--
create procedure CK.sResCreateWithResPath
(
	@ResPath varchar(128),
	@ResId int output
)
as 
begin
	--[beginsp]

	exec CK.sResCreate @ResId output;
	exec CK.sResPathCreate @ResId, @ResPath;
	
	--[endsp]
end
-- SetupConfig: { "Requires": [ "CK.sResDestroy", "CK.sResPathDestroy" ] }
--
-- Destroys a root resource and/or its children thanks to its name.
-- Note that if @WithRoot and @WithChildren are both 0, nothing is done.
-- If the root name doesn't exist, its children can nevertheless be destroyed.
-- Setting @ResPathOnly to 0 will call CK.sResDestroy, destroying the ResId and all its resources.
-- By default, only the resource name is destroyed (this is the safest way).
-- 
create procedure CK.sResDestroyByResPath
(
	@RootResPath varchar(128),
	@WithRoot bit = 1,
	@WithChildren bit = 1,
	@ResPathOnly bit = 1
)
as begin
    --[beginsp]

    --<PresResDestroyByResPath revert />
    declare @RootResId int;

    if @WithRoot = 1 
    begin
	    select @RootResId = r.ResId 
		    from CK.tResPath r
		    where r.ResPath = @RootResPath;
		if @@RowCount <> 0
		begin
			if @ResPathOnly = 1 exec CK.sResPathDestroy @RootResId;
			else exec CK.sResDestroy @RootResId;
		end
    end
    if @WithChildren = 1
	begin
        declare @ChildResId int;
	    declare @CRes cursor;
	    set @CRes = cursor local fast_forward for 
		    select r.ResId 
			    from CK.tResPath r
			    where r.ResPath like @RootResPath + '/%';
	    open @CRes;
	    fetch from @CRes into @ChildResId;
	    while @@FETCH_STATUS = 0
	    begin
		    if @ResPathOnly = 1 exec CK.sResPathDestroy @ChildResId;
		    else exec CK.sResDestroy @ChildResId;
		    fetch next from @CRes into @ChildResId;
	    end
	    deallocate @CRes;
    end

	--<PostsResDestroyByResPath />

    --[endsp]
end


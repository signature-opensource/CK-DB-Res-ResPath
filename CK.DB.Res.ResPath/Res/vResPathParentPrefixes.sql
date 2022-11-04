-- SetupConfig: { "Requires": [ "CK.fResPathPrefixes" ] }
create view CK.vResPathParentPrefixes 
as 
	select  r.ResId,
			r.ResPath,
			-- Null if the ParentPrefix is not an existing resource.
			ParentResId = pExist.ResId,
			-- Parent prefix that may exist as an actual resource or not.
			p.ParentPrefix,
			-- Level of the ParentPrefix. First parent has level 1.
			p.ParentLevel
		from CK.tResPath r
		cross apply CK.fResPathPrefixes( r.ResPath ) p
		left outer join CK.tResPath pExist on pExist.ResPath = p.ParentPrefix;

-- SetupConfig: {}
create view CK.vResPathDirectChildren
as
select  ResId = r.ResId,
		ResPath = r.ResPath,
		c.ChildId,
		c.ChildName
	from CK.tResPath r
	cross apply (select ChildId = ResId, 
						ChildName = ResPath
					from CK.tResPath 
					where ResPath like r.ResPath + '/%' and ResPath not like r.ResPath + '/%/%' ) c;


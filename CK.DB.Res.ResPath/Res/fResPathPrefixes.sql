-- SetupConfig:{}
-- Inline Table Value Function (ITVF) version to get the parent ResPaths from a ResPath.
-- This is the fastest implementation I can achieve.
create function CK.fResPathPrefixes( @ResPath varchar(128) )
returns table -- with schemabinding
 return
		with E1(n) as ( select 1 union all select 1 union all select 1 union all select 1 union all
						select 1 union all select 1 union all select 1 union all select 1 union all 
						select 1 union all select 1 union all select 1 union all select 1
					 ),                          -- 12 rows
		    E2(n) as (select 1 from E1 a, E1 b), -- 12*12  = 144 rows
			T(n) as ( 
						select top(len(@ResPath)) n = row_number() over (order by  (select null) desc)-1 from E2
					) 
		select	ParentLevel = row_number() over (order by  (select null) desc), 
				ParentPrefix = SUBSTRING(@ResPath, 0, len(@ResPath)-T.n ) collate Latin1_General_100_BIN2
			from T
			where len(@ResPath) > 1 and SUBSTRING(@ResPath, len(@ResPath)-T.n, 1 ) = '/';

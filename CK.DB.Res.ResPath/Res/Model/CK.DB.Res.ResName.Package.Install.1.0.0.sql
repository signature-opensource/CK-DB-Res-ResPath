--[beginscript]

if OBJECT_ID('CK.sResDestroyByResPathPrefix') is not null drop procedure CK.sResDestroyByResPathPrefix;
if OBJECT_ID('CK.sResDestroyResPathChildren') is not null drop procedure CK.sResDestroyResPathChildren;
if OBJECT_ID('CK.sResDestroyWithResPathChildren') is not null drop procedure CK.sResDestroyWithResPathChildren;

--[endscript]
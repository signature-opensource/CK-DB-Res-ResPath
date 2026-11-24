Implements the `CK.tResPath` table: a unique path-based name for the resources of CK.DB.Res, with the functions and
views that navigate the resulting hierarchy.

`ResPathTable` creates, renames and destroys the resources by their path (`ResPathTable.CreateWithResPathAsync`,
`ResPathTable.CreateResPathAsync`, `ResPathTable.RenameAsync`, `ResPathTable.DestroyByResPathAsync`); the
`CK.vResPathAllChildren`, `CK.vResPathDirectChildren` and `CK.vResPathParentPrefixes` views, the `CK.fResPathPrefixes`
function and the `ResPath` column added to `CK.vRes` read it.

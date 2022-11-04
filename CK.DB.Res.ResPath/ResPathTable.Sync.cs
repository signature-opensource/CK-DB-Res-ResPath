using CK.Core;
using CK.SqlServer;

namespace CK.DB.Res.ResPath
{
    public abstract partial class ResPathTable : SqlTable
    {
        /// <summary>
        /// Renames a resource, by default renaming also its children.
        /// Nothing is done if the resource does not exist or has no associated ResPath.
        /// </summary>
        /// <param name="ctx">The call context.</param>
        /// <param name="resId">The resource identifier to rename.</param>
        /// <param name="newName">The new resource name.</param>
        /// <param name="withChildren">
        /// False to rename only this resource and not its children: children
        /// names are left as-is as "orphans".
        /// </param>
        [SqlProcedure( "sResPathRename" )]
        public abstract void Rename( ISqlCallContext ctx, int resId, string newName, bool withChildren = true );

        /// <summary>
        /// Renames a resource, by default renaming also its children.
        /// Nothing is done if the resource does not exist or has no associated ResPath.
        /// </summary>
        /// <param name="ctx">The call context.</param>
        /// <param name="oldName">The resource name to rename.</param>
        /// <param name="newName">The new resource name.</param>
        /// <param name="withChildren">
        /// False to rename only this resource and not its children: children
        /// names are left as-is as "orphans".
        /// </param>
        [SqlProcedure( "sResPathRenameResPath" )]
        public abstract void Rename( ISqlCallContext ctx, string oldName, string newName, bool withChildren = true );


        /// <summary>
        /// Creates a new resource name for an existing resource identifier.
        /// There must not be an existing resource name.
        /// </summary>
        /// <param name="ctx">The call context.</param>
        /// <param name="resId">The resource identifier that must exist.</param>
        /// <param name="ResPath">The resource name to create.</param>
        [SqlProcedure( "sResPathCreate" )]
        public abstract void CreateResPath( ISqlCallContext ctx, int resId, string ResPath );

        /// <summary>
        /// Creates a new resource with a name.
        /// </summary>
        /// <param name="ctx">The call context.</param>
        /// <param name="ResPath">The resource name to create.</param>
        /// <returns>The new resource identifier.</returns>
        [SqlProcedure( "sResCreateWithResPath" )]
        public abstract int CreateWithResPath( ISqlCallContext ctx, string ResPath );

        /// <summary>
        /// Destroys the resource name associated to a resource if any.
        /// </summary>
        /// <param name="ctx">The call context.</param>
        /// <param name="resId">The resoource identifier.</param>
        [SqlProcedure( "sResPathDestroy" )]
        public abstract void DestroyResPath( ISqlCallContext ctx, int resId );

        /// <summary>
        /// Destroys a root resource and/or its children thanks to its name.
        /// Note that if <paramref name="withRoot"/> and <paramref name="withChildren"/> are both false, nothing is done.
        /// If the root name doesn't exist, its children can nevertheless be destroyed.
        /// Setting <paramref name="ResPathOnly"/> to false will call CK.sResDestroy, destroying the ResId 
        /// and all its resources. By default, only the resource name is destroyed (this is the safest way).
        /// /// </summary>
        /// <param name="ctx">The call context.</param>
        /// <param name="rootResPath">The root resource name to destroy.</param>
        /// <param name="withRoot">Whether the root itself must be destroyed.</param>
        /// <param name="withChildren">Whether the root's children must be destroyed.</param>
        /// <param name="ResPathOnly">
        /// Set it it false to call sResDestroy (destroying the whole resources) instead of sResPathDestroy.
        /// This has be set explicitely.
        /// </param>
        [SqlProcedure( "sResDestroyByResPath" )]
        public abstract void DestroyByResPath( ISqlCallContext ctx, string rootResPath, bool withRoot = true, bool withChildren = true, bool ResPathOnly = true );

    }
}

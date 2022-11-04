using CK.Core;
using CK.SqlServer;
using System.Threading.Tasks;

namespace CK.DB.Res.ResPath
{
    /// <summary>
    /// This table holds the resource name.
    /// Resource names implement a path-based hierarchy.
    /// </summary>
    [SqlTable( "tResPath", Package = typeof( Package ) )]
    [Versions( "1.0.0" )]
    [SqlObjectItem( "transform:vRes, transform:sResDestroy" )]
    [SqlObjectItem( "fResPathPrefixes" )]
    [SqlObjectItem( "vResPathAllChildren, vResPathDirectChildren, vResPathParentPrefixes" )]
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
        /// <returns>The awaitable.</returns>
        [SqlProcedure( "sResPathRename" )]
        public abstract Task RenameAsync( ISqlCallContext ctx, int resId, string newName, bool withChildren = true );

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
        /// <returns>The awaitable.</returns>
        [SqlProcedure( "sResPathRenameResPath" )]
        public abstract Task RenameAsync( ISqlCallContext ctx, string oldName, string newName, bool withChildren = true );

        /// <summary>
        /// Creates a new resource name for an existing resource identifier.
        /// There must not be an existing resource name.
        /// </summary>
        /// <param name="ctx">The call context.</param>
        /// <param name="resId">The resource identifier that must exist.</param>
        /// <param name="ResPath">The resource name to create.</param>
        /// <returns>The awaitable.</returns>
        [SqlProcedure( "sResPathCreate" )]
        public abstract Task CreateResPathAsync( ISqlCallContext ctx, int resId, string ResPath );

        /// <summary>
        /// Creates a new resource with a name.
        /// </summary>
        /// <param name="ctx">The call context.</param>
        /// <param name="ResPath">The resource name to create.</param>
        /// <returns>The new resource identifier.</returns>
        [SqlProcedure( "sResCreateWithResPath" )]
        public abstract Task<int> CreateWithResPathAsync( ISqlCallContext ctx, string ResPath );

        /// <summary>
        /// Destroys the resource name associated to a resource identifier if any.
        /// This doesn't destroy the resource itself, only the name.
        /// </summary>
        /// <param name="ctx">The call context.</param>
        /// <param name="resId">The resoource identifier.</param>
        /// <returns>The awaitable.</returns>
        [SqlProcedure( "sResPathDestroy" )]
        public abstract Task DestroyResPathAsync( ISqlCallContext ctx, int resId );

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
        /// <returns>The awaitable.</returns>
        [SqlProcedure( "sResDestroyByResPath" )]
        public abstract Task DestroyByResPathAsync( ISqlCallContext ctx, string rootResPath, bool withRoot = true, bool withChildren = true, bool ResPathOnly = true );

    }
}

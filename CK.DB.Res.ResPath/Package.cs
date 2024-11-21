using CK.Core;

namespace CK.DB.Res.ResPath;

/// <summary>
/// This package brings the resource name support (a path-based hierarchy).
/// </summary>
[SqlPackage( Schema = "CK", ResourcePath = "Res" )]
// The 1.0.0 version of the package removes the previous (and crappy) sResDestroyByResPathPrefix, sResDestroyResPathChildren 
// and sResDestroyWithResPathChildren procedures.
[Versions( "1.0.0" )]
public class Package : SqlPackage
{
    void StObjConstruct( Res.Package resource )
    {
    }

    /// <summary>
    /// Gets the resource table.
    /// </summary>
    [InjectObject]
    public ResTable ResTable { get; protected set; }

    /// <summary>
    /// Gets the CK.tResPath table.
    /// </summary>
    [InjectObject]
    public ResPathTable ResPathTable { get; protected set; }
}

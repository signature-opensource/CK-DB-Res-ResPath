using CK.Core;
using CK.SqlServer;
using CK.Testing;
using Shouldly;
using NUnit.Framework;
using System;
using static CK.Testing.MonitorTestHelper;

namespace CK.DB.Res.ResPath.Tests;

[TestFixture]
public class ResPathTests
{
    [Test]
    public void resource_0_and_1_are_empty_and_System()
    {
        var r = SharedEngine.Map.StObjs.Obtain<ResPathTable>();
        r.Database.ExecuteScalar( "select ResPath from CK.vRes where ResId = 0" )
            .ShouldBe( "" );
        r.Database.ExecuteScalar( "select ResPath from CK.vRes where ResId = 1" )
            .ShouldBe( "System" );
    }


    [Test]
    public void resource_0_and_1_can_not_be_destroyed()
    {
        var p = SharedEngine.Map.StObjs.Obtain<Package>();
        using( var ctx = new SqlStandardCallContext( TestHelper.Monitor ) )
        {
            Util.Invokable( () => p.ResTable.Destroy( ctx, 0 ) ).ShouldThrow<SqlDetailedException>();
            Util.Invokable( () => p.ResTable.Destroy( ctx, 1 ) ).ShouldThrow<SqlDetailedException>();
        }
    }

    [Test]
    public void CreateResPath_raises_an_exception_if_the_resource_is_already_associated_to_a_path_or_the_path_already_exists()
    {
        var p = SharedEngine.Map.StObjs.Obtain<Package>();
        using( var ctx = new SqlStandardCallContext( TestHelper.Monitor ) )
        {
            int resId = p.ResTable.Create( ctx );
            string ResPath = Guid.NewGuid().ToString();
            string ResPath2 = Guid.NewGuid().ToString();
            p.ResPathTable.CreateResPath( ctx, resId, ResPath );
            // Creates where a name already exists.
            Util.Invokable( () => p.ResPathTable.CreateResPath( ctx, resId, ResPath2 ) ).ShouldThrow<SqlDetailedException>();
            // Creates with an already existing name.
            int resId2 = p.ResTable.Create( ctx );
            Util.Invokable( () => p.ResPathTable.CreateResPath( ctx, resId2, ResPath ) ).ShouldThrow<SqlDetailedException>();
            p.ResTable.Destroy( ctx, resId );
            p.ResTable.Destroy( ctx, resId2 );
        }
    }

    [Test]
    public void renaming_a_resource_can_be_done_WithChildren_or_only_for_the_resource_itself_by_resId()
    {
        var p = SharedEngine.Map.StObjs.Obtain<Package>();
        using( var ctx = new SqlStandardCallContext( TestHelper.Monitor ) )
        {
            p.ResPathTable.DestroyByResPath( ctx, "Test", ResPathOnly: false );

            int n1 = p.ResPathTable.CreateWithResPath( ctx, "Test/Root" );
            int n2 = p.ResPathTable.CreateWithResPath( ctx, "Test/Root/1" );
            int n3 = p.ResPathTable.CreateWithResPath( ctx, "Test/Root/1/1" );

            p.ResPathTable.Rename( ctx, n1, "Test/-Root-" );
            p.Database.ExecuteReader( "select * from CK.tResPath where ResPath like 'Test/Root%'" )
                .Rows.ShouldBeEmpty();
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/-Root-'" )
                .ShouldBe( n1 );
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/-Root-/1'" )
                .ShouldBe( n2 );
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/-Root-/1/1'" )
                .ShouldBe( n3 );

            p.ResPathTable.Rename( ctx, n1, "Test/MovedTheRootOnly", withChildren: false );
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/MovedTheRootOnly'" )
                    .ShouldBe( n1 );
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/-Root-/1'" )
                    .ShouldBe( n2 );
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/-Root-/1/1'" )
                .ShouldBe( n3 );
        }
    }

    [Test]
    public void renaming_a_resource_can_be_done_WithChildren_or_only_for_the_resource_itself_by_ResPath()
    {
        var p = SharedEngine.Map.StObjs.Obtain<Package>();
        using( var ctx = new SqlStandardCallContext( TestHelper.Monitor ) )
        {
            p.ResPathTable.DestroyByResPath( ctx, "Test", ResPathOnly: false );

            int n1 = p.ResPathTable.CreateWithResPath( ctx, "Test/Root" );
            int n2 = p.ResPathTable.CreateWithResPath( ctx, "Test/Root/1" );
            int n3 = p.ResPathTable.CreateWithResPath( ctx, "Test/Root/1/1" );

            p.ResPathTable.Rename( ctx, "Test/Root", "Test/-Root-" );
            p.Database.ExecuteReader( "select * from CK.tResPath where ResPath like 'Test/Root%'" )
                .Rows.ShouldBeEmpty();
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/-Root-'" )
                .ShouldBe( n1 );
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/-Root-/1'" )
                .ShouldBe( n2 );
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/-Root-/1/1'" )
                .ShouldBe( n3 );

            p.ResPathTable.Rename( ctx, "Test/-Root-", "Test/MovedTheRootOnly", withChildren: false );
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/MovedTheRootOnly'" )
                    .ShouldBe( n1 );
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/-Root-/1'" )
                    .ShouldBe( n2 );
            p.Database.ExecuteScalar( "select ResId from CK.tResPath where ResPath='Test/-Root-/1/1'" )
                .ShouldBe( n3 );
        }
    }

    [Test]
    public void using_DestroyByPrefix_enables_destruction_without_an_existing_parent()
    {
        var p = SharedEngine.Map.StObjs.Obtain<Package>();
        using( var ctx = new SqlStandardCallContext( TestHelper.Monitor ) )
        {
            var nameRoot = Guid.NewGuid().ToString();

            int n1 = p.ResPathTable.CreateWithResPath( ctx, nameRoot + "/Test/Root" );
            int n2 = p.ResPathTable.CreateWithResPath( ctx, nameRoot + "/Test/Root.1" );
            int n3 = p.ResPathTable.CreateWithResPath( ctx, nameRoot + "/Test/Root/1/1" );
            p.Database.ExecuteScalar( "select count(*) from CK.tResPath where ResPath like @0+'%'", nameRoot ).ShouldBe( 3 );

            p.ResPathTable.DestroyByResPath( ctx, nameRoot, withRoot: true, withChildren: true, ResPathOnly: false );
            p.Database.ExecuteScalar( "select count(*) from CK.tResPath where ResPath like @0 + '%'", nameRoot ).ShouldBe( 0 );
            p.Database.ExecuteScalar( "select count(*) from CK.tRes where ResId in (@0, @1, @2)", n1, n2, n3 ).ShouldBe( 0 );

            // Destroys root, keep children.
            n1 = p.ResPathTable.CreateWithResPath( ctx, nameRoot + "/Test/Root" );
            n2 = p.ResPathTable.CreateWithResPath( ctx, nameRoot + "/Test/Root/1" );
            n3 = p.ResPathTable.CreateWithResPath( ctx, nameRoot + "/Test/Root/1/1" );
            p.Database.ExecuteScalar( "select count(*) from CK.tResPath where ResPath like @0+'%'", nameRoot ).ShouldBe( 3 );

            p.ResPathTable.DestroyByResPath( ctx, nameRoot + "/Test/Root", withRoot: true, withChildren: false, ResPathOnly: false );
            p.Database.ExecuteScalar( "select count(*) from CK.tResPath where ResPath like @0+'%'", nameRoot ).ShouldBe( 2 );

            p.ResPathTable.DestroyByResPath( ctx, nameRoot + "/Test/Root", withRoot: true, withChildren: true, ResPathOnly: false );
            p.Database.ExecuteScalar( "select count(*) from CK.tResPath where ResPath like @0+'%'", nameRoot ).ShouldBe( 0 );

            // Destroys children but keep the root.
            n1 = p.ResPathTable.CreateWithResPath( ctx, nameRoot + "/Test/Root" );
            n2 = p.ResPathTable.CreateWithResPath( ctx, nameRoot + "/Test/Root/1" );
            n3 = p.ResPathTable.CreateWithResPath( ctx, nameRoot + "/Test/Root/1/1" );
            p.Database.ExecuteScalar( "select count(*) from CK.tResPath where ResPath like @0+'%'", nameRoot ).ShouldBe( 3 );

            p.ResPathTable.DestroyByResPath( ctx, nameRoot + "/Test/Root", withRoot: false, withChildren: true, ResPathOnly: false );
            p.Database.ExecuteScalar( "select count(*) from CK.tResPath where ResPath like @0 + '%'", nameRoot ).ShouldBe( 1 );

            p.ResPathTable.DestroyByResPath( ctx, nameRoot + "/Test/Root", withRoot: true, withChildren: false, ResPathOnly: false );
            p.Database.ExecuteScalar( "select count(*) from CK.tResPath where ResPath like @0 + '%'", nameRoot ).ShouldBe( 0 );

        }
    }
}

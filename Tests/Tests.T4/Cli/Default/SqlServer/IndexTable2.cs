// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.SqlServer
{
	[Table("IndexTable2")]
	public class IndexTable2
	{
		[Column("PKField1", IsPrimaryKey = true, PrimaryKeyOrder = 1)] public int PkField1 { get; set; } // int
		[Column("PKField2", IsPrimaryKey = true, PrimaryKeyOrder = 0)] public int PkField2 { get; set; } // int

		#region Associations
		/// <summary>
		/// FK_Patient2_IndexTable
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(PkField2) + "," + nameof(PkField2), OtherKey = nameof(IndexTable.PkField2) + "," + nameof(PkField2))]
		public IndexTable Patient2IndexTable { get; set; } = null!;
		#endregion
	}
}

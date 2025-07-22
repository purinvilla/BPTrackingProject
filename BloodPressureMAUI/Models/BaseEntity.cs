using System;
using SQLite;

namespace BloodPressureMAUI.Models;

public abstract class BaseEntity {

	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

}

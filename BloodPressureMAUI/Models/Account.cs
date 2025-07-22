using SQLite;

namespace BloodPressureMAUI.Models;

[Table("users")]
public class Account : BaseEntity {

	[Unique]
	public string Nickname { get; set; }
	[Unique]
	public string Email { get; set; }
	public string Password { get; set; }

}

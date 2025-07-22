using Newtonsoft.Json;
using System;

namespace BloodPressureMAUI.Models;

public class BPData : BaseEntity {

    // Properties
    public DateTime Posttime { get; set; }
    public int Systolic { get; set; }
	public int Diastolic { get; set; }
	public int Heartrate { get; set; }

    // Constructor
    public BPData(DateTime Posttime,int Systolic, int Diastolic, int Heartrate) {
        this.Posttime = Posttime;
        this.Systolic = Systolic;
        this.Diastolic = Diastolic;
        this.Heartrate = Heartrate;
    }

}

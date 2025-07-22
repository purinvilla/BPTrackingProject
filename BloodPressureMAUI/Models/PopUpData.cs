using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodPressureMAUI.Models;

public class PopUpData {

    // Properties
    public string Title { get; set; }
    public string Description { get; set; }
    public string ButtonText { get; set; }
    public String IconName { get; set; }

    // Constructor
    public PopUpData(string title, string description, string buttonText, string iconName) {
        Title = title;
        Description = description;
        ButtonText = buttonText;
        IconName = iconName;
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLibrary.Models
{
    public class SettingsDTO
    {
        public bool Topmost { get; set; } = false;
        public bool HideCompleted { get; set; } = false;
        public SettingsDTO() { }

        public SettingsDTO(bool topmost, bool hideCompleted)
        {
            Topmost = topmost;
            HideCompleted = hideCompleted;
        }
    }
}

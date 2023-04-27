using DigitalPlatform.CirculationClient;

namespace dp2billing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ClientInfo.ProgramName = "dp2billing";
            ClientInfo.MainForm = this;

            InitializeComponent();
        }
    }
}
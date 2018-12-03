using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class BlackInvader : UserControl
    {
        public BlackInvader()
        {
            InitializeComponent();
        }
        public int Speed { get { return 20; } }
        public int Move { get { return 30; } }
    }
}

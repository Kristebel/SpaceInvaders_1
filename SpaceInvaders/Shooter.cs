using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class Shooter: UserControl
    {
        private const int _speed = 30; 
        public Shooter()
        {
            InitializeComponent();
        }
        public int Speed { get { return _speed; } }
    }
}

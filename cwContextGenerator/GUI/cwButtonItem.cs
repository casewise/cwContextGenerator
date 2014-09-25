using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Casewise.GraphAPI.API;
using System.Drawing;
using System.Reflection;
using Casewise.GraphAPI.PSF;

namespace cwContextGenerator.GUI
{
    class cwButtonItem : Panel
    {
        Launcher mainGUI = null;
        private cwLightObject _sourceObject = null;
        public PictureBox pb = new PictureBox();

        public cwButtonItem(Launcher mainGUI, cwLightObject sourceObject)
        {
            this.mainGUI = mainGUI;
            _sourceObject = sourceObject;
            this.Font = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "button" + sourceObject.ID.ToString();
            this.Size = new System.Drawing.Size(136, 116);

            this.MouseEnter += new System.EventHandler(this.cwButtonItem_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.cwButtonItem_MouseLeave);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwButtonItem_Click);

            Label l = new Label();
            l.Text = sourceObject.ToString();
            l.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l.Size = new System.Drawing.Size(136, 40);
            l.Location = new System.Drawing.Point(0, 76);
            l.Dock = DockStyle.Bottom;
            l.ForeColor = Color.Black;
            l.TextAlign = ContentAlignment.TopCenter;
            l.MouseEnter += new System.EventHandler(this.cwButtonItem_MouseEnter);
            l.MouseLeave += new System.EventHandler(this.cwButtonItem_MouseLeave);
            l.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwButtonItem_Click);

            pb.Image = global::cwContextGenerator.Properties.Resources.image_config_small;
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Size = new System.Drawing.Size(96, 70);
            pb.Location = new System.Drawing.Point(20, 0);
            //pb.Dock = DockStyle.Top;
            pb.MouseEnter += new System.EventHandler(this.cwButtonItem_MouseEnter);
            pb.MouseLeave += new System.EventHandler(this.cwButtonItem_MouseLeave);
            pb.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwButtonItem_Click);

            Controls.Add(pb);
            Controls.Add(l);
        }


        private void cwButtonItem_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            //pb.Image = mainGUI.options.itemIcon;
        }

        private void cwButtonItem_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            //pb.Image = mainGUI.options.itemIconAnimated;
        }


        private void cwButtonItem_Click(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                this.mainGUI.LoadConfigurationInGUI(this._sourceObject);
            }
        }


    }
}

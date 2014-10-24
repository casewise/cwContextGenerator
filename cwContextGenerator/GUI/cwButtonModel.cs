using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Casewise.GraphAPI.API;
using System.Drawing;
using Casewise.GraphAPI.PSF;

namespace cwContextGenerator.GUI
{
    class cwButtonModel : Panel
    {
        private cwLightModel _model = null;
        PictureBox pb = new PictureBox();
        public Launcher mainGUI = null;
        public cwButtonModel(Launcher mainGUI, cwLightModel model)
        {
            this.mainGUI = mainGUI;
            _model = model;
            this.Font = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "buttonModel" + model.FileName;
            this.Size = new System.Drawing.Size(136, 116);

            Label l = new Label();
            l.Text = model.ToString();           
            l.Size = new System.Drawing.Size(136, 40);
            l.Location = new Point(0, 76);
            l.Font = new System.Drawing.Font("Trebuchet MS", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l.Dock = DockStyle.Bottom;
            l.ForeColor = Color.Black;
            l.AutoSize = false;
            l.TextAlign = ContentAlignment.TopCenter;
            l.MouseEnter += new System.EventHandler(this.cwButtonModel_MouseEnter);
            l.MouseLeave += new System.EventHandler(this.cwButtonModel_MouseLeave);
            l.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwButtonModel_Click);

            pb.Image = global::cwContextGenerator.Properties.Resources.image_model_small;
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            //pb.BackgroundImageLayout = ImageLayout.Stretch;
            pb.Size = new System.Drawing.Size(96, 70);
            pb.Location = new System.Drawing.Point(20, 0);
            //pb.Dock = DockStyle.Top;
            pb.MouseEnter += new System.EventHandler(this.cwButtonModel_MouseEnter);
            pb.MouseLeave += new System.EventHandler(this.cwButtonModel_MouseLeave);
            pb.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwButtonModel_Click);

            Controls.Add(pb);
            Controls.Add(l);
        }

        private void cwButtonModel_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            pb.Image = global::cwContextGenerator.Properties.Resources.image_model_small;
        }

        private void cwButtonModel_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            pb.Image = global::cwContextGenerator.Properties.Resources.image_model_hover;
        }

        private void cwButtonModel_Click(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                this._model.loadLightModelContent();
                this.mainGUI.GetCore().SelectedModel = this._model;
                
                this.mainGUI.DisplayConfigurations(this._model);
                this.mainGUI.optionBack.Visible = true;
            }
        }


    }
}

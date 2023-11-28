using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Agent 
{
    class ImageButton : PictureBox
    {
        private Image normal_img_on;
        private Image clicked_img_on;

        private Image normal_img_off;
        private Image clicked_img_off;

        private Image disable_img_on;
        private Image disable_img_off;

        public delegate void performClickDelegate(object sender);
        private performClickDelegate clickdelegate;

        private Boolean isOn;
        private Boolean isEnable;

        public ImageButton()
        {
            clickdelegate = null;

            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.performMouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.performMouseUp);
            this.Click += new System.EventHandler(this.performClick);
        }
        public void SetImages(Image normal_on, Image clicked_on, Image normal_off, Image clicked_off, Image disable_on, Image disable_off)
        {
            normal_img_on = normal_on;
            clicked_img_on = clicked_on;

            normal_img_off = normal_off;
            clicked_img_off = clicked_off;

            disable_img_on = disable_on;
            disable_img_off = disable_off;

            this.BackgroundImage = normal_img_off;
            this.BackgroundImageLayout = ImageLayout.Zoom;
            

            isOn = false;
            isEnable = true;
        }

        public Boolean GetState()
        {
            return isOn;
        }
        public void SetOnState(Boolean state, Boolean isEnable)
        {
            isOn = state;
            this.isEnable = isEnable;
            if (isEnable)
            {
                if (isOn)
                    this.BackgroundImage =normal_img_on;
                else
                    this.BackgroundImage = normal_img_off;
            }
            else
            {
                if (isOn)
                    this.BackgroundImage = disable_img_on;
                else
                    this.BackgroundImage = disable_img_off;
            }
        }

        public void SetOnState(Boolean state)
        {
            if (!isEnable) return;
            isOn = state;
           
            if (isOn)
                this.BackgroundImage = normal_img_on;
            else
                this.BackgroundImage = normal_img_off;
          
        }
        public void SetEnable(Boolean isEnable)
        {
            this.isEnable = isEnable;
            SetOnState(isOn, isEnable);
        }
        
        public void setClickDelegate(performClickDelegate clickdelegate)
        {
            this.clickdelegate = clickdelegate;
        }

        

        public void performClick(object sender, EventArgs e)
        {
            if (clickdelegate == null || !isEnable) return;
            clickdelegate(sender);
        }



        private void performMouseDown(object sender, MouseEventArgs e)
        {
            if (!isEnable) return;
            if (isOn)
                this.BackgroundImage = clicked_img_on;
            else
                this.BackgroundImage = clicked_img_off;

        }

        private void performMouseUp(object sender, MouseEventArgs e)
        {
            if (!isEnable) return;
            if (isOn)
                this.BackgroundImage = normal_img_on;
            else
                this.BackgroundImage = normal_img_off;

           // isOn = !isOn;
        }
    }
}

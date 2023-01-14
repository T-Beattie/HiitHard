﻿using HiitHard.Droid;
using HiitHard.Effects;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("HiitHard")]
[assembly: ExportEffect(typeof(AndroidLongPressedEffect), "LongPressedEffect")]
namespace HiitHard.Droid
{
    public class AndroidLongPressedEffect : PlatformEffect
    {
        private bool _attached;

        public static void Initialize() { }

        public AndroidLongPressedEffect()
        {
        }

        protected override void OnAttached()
        {
            //because an effect can be detached immediately after attached (happens in listview), only attach the handler one time.
            if (!_attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick += Control_LongClick;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick += Control_LongClick;
                }
                _attached = true;
            }
        }


        // Invoke the command if there is one
        private void Control_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            Console.WriteLine("Invoking long click command");
            var command = LongPressedEffect.GetCommand(Element);
            command?.Execute(LongPressedEffect.GetCommandParameter(Element));
        }


        protected override void OnDetached()
        {
            if (_attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick -= Control_LongClick;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick -= Control_LongClick;
                }
                _attached = false;
            }
        }
    }
}
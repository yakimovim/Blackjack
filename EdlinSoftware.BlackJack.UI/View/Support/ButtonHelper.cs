﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace EdlinSoftware.BlackJack.UI.View.Support
{
    public static class ButtonHelper
    {
        public static bool? GetDialogResult(DependencyObject obj)
        { return (bool?)obj.GetValue(DialogResultProperty); }

        public static void SetDialogResult(DependencyObject obj, bool? value)
        { obj.SetValue(DialogResultProperty, value); }

        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult",
            typeof(bool?),
            typeof(ButtonHelper),
            new UIPropertyMetadata(OnDialogResultChanged));

        private static void OnDialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (button == null)
                throw new InvalidOperationException("Can only use ButtonHelper.DialogResult on a Button control");
            button.Click += (sender, e2) =>
            {
                var window = Window.GetWindow(button);

                if (window != null)
                {
                    window.DialogResult = GetDialogResult(button);
                }
            };
        }
    }
}

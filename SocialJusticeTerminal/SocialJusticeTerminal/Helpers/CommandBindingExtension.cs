﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace SocialJusticeTerminal.Helpers
{
    [MarkupExtensionReturnType(typeof(ICommand))]
    public class BaseBindingExtension<TT> : MarkupExtension where TT : class 
    {
        public BaseBindingExtension()
        {
        }

        public BaseBindingExtension(string commandName)
        {
            this.CommandName = commandName;
        }

        [ConstructorArgument("commandName")]
        public string CommandName { get; set; }

        private object targetObject;
        private object targetProperty;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget != null)
            {
                targetObject = provideValueTarget.TargetObject;
                targetProperty = provideValueTarget.TargetProperty;
            }

            if (!string.IsNullOrEmpty(CommandName))
            {
                // The serviceProvider is actually a ProvideValueServiceProvider, which has a private field "_context" of type ParserContext
                ParserContext parserContext = GetPrivateFieldValue<ParserContext>(serviceProvider, "_context");
                if (parserContext != null)
                {
                    // A ParserContext has a private field "_rootElement", which returns the root element of the XAML file
                    FrameworkElement rootElement = GetPrivateFieldValue<FrameworkElement>(parserContext, "_rootElement");
                    if (rootElement != null)
                    {
                        // Now we can retrieve the DataContext
                        object dataContext = rootElement.DataContext;

                        // The DataContext may not be set yet when the FrameworkElement is first created, and it may change afterwards,
                        // so we handle the DataContextChanged event to update the Command when needed
                        if (!dataContextChangeHandlerSet)
                        {
                            rootElement.DataContextChanged += new DependencyPropertyChangedEventHandler(rootElement_DataContextChanged);
                            dataContextChangeHandlerSet = true;
                        }

                        if (dataContext != null)
                        {
                            TT command = GetCommand(dataContext, CommandName);
                            if (command != null)
                                return command;
                        }
                    }
                }
            }

            // The Command property of an InputBinding cannot be null, so we return a dummy extension instead
            return DummyCommand.Instance;
        }

        private TT GetCommand(object dataContext, string commandName)
        {
            PropertyInfo prop = dataContext.GetType().GetProperty(commandName);
            if (prop != null)
            {
                TT command = prop.GetValue(dataContext, null) as TT;
                if (command != null)
                    return command;
            }
            return null;
        }

        private void AssignCommand(TT command)
        {
            if (targetObject != null && targetProperty != null)
            {
                if (targetProperty is DependencyProperty)
                {
                    DependencyObject depObj = targetObject as DependencyObject;
                    DependencyProperty depProp = targetProperty as DependencyProperty;
                    depObj.SetValue(depProp, command);
                }
                else
                {
                    PropertyInfo prop = targetProperty as PropertyInfo;
                    prop.SetValue(targetObject, command, null);
                }
            }
        }

        private bool dataContextChangeHandlerSet = false;
        private void rootElement_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement rootElement = sender as FrameworkElement;
            if (rootElement != null)
            {
                object dataContext = rootElement.DataContext;
                if (dataContext != null)
                {
                    TT command = GetCommand(dataContext, CommandName);
                    if (command != null)
                    {
                        AssignCommand(command);
                    }
                }
            }
        }

        private T GetPrivateFieldValue<T>(object target, string fieldName)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
            {
                return (T)field.GetValue(target);
            }
            return default(T);
        }
    }

   [MarkupExtensionReturnType(typeof(ICommand))]
    public class CommandBindingExtension : BaseBindingExtension<ICommand>
    {
        public CommandBindingExtension()
        {
        }
 
        public CommandBindingExtension(string commandName) : base(commandName)
        {
        }
    }

   [MarkupExtensionReturnType(typeof(object))]
    public class ObjectBindingExtension : BaseBindingExtension<object>
    {
        public ObjectBindingExtension()
        {
        }

        public ObjectBindingExtension(string commandName)
            : base(commandName)
        {
        }
    }

   // A dummy command that does nothing...
   internal class DummyCommand : ICommand
   {

       #region Singleton pattern

       private DummyCommand()
       {
       }

       private static DummyCommand _instance = null;
       public static DummyCommand Instance
       {
           get
           {
               if (_instance == null)
               {
                   _instance = new DummyCommand();
               }
               return _instance;
           }
       }

       #endregion

       #region ICommand Members

       public bool CanExecute(object parameter)
       {
           return false;
       }

       public event EventHandler CanExecuteChanged;

       public void Execute(object parameter)
       {
       }

       #endregion
   }
}

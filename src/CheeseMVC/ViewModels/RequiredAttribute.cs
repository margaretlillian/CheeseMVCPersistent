using System;

namespace CheeseMVC.ViewModels
{
    internal class RequiredAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
    }
}
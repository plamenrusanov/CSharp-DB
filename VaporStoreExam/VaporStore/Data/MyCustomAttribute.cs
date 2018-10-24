using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.Data
{
    [AttributeUsage(AttributeTargets.Property |
  AttributeTargets.Field, AllowMultiple = false)]
    public sealed class MyCustomAttribute : ValidationAttribute
    {

    }
}

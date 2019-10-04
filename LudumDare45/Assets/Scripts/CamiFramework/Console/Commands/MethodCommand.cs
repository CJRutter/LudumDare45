using CamiFramwork.TypeConvertion;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CamiFramwork.ConsoleUtil
{
    public class MethodCommand : IConsoleCommand
    {
        public MethodCommand(MethodInfo method, object instance, TypeConverterCollection converterCollection, string description = null)
        {
            this.method = method;
            this.instance = instance;
            this.converterCollection = converterCollection;
            this.description = description;
        }

        public MethodCommand(string methodName, object instance, TypeConverterCollection converterCollection, string description = null)
        {
            method = instance.GetType().GetMethod(methodName);
            this.instance = instance;
            this.converterCollection = converterCollection;
            this.description = description;
        }

        public void Execute(Console console, IList<string> args)
        {
            ParameterInfo[] parameters = method.GetParameters();

            if(parameters.Length != args.Count - 1)
            {
                Console.Log("Incorrect number of parameters expected {0}, found {1}",
                    parameters.Length, args.Count - 1);

                foreach(ParameterInfo parameter in parameters)
                {
                    Console.Log("  {0} {1}", parameter.ParameterType.Name, parameter.Name);
                }
                return;
            }

            var parameterValues = new object[parameters.Length];
            for(int i = 0; i < parameters.Length; ++i)
            {
                ParameterInfo parameter = parameters[i];
                if(parameter.ParameterType == typeof(string))
                {
                    parameterValues[i] = args[i + 1];
                }
                else if(converterCollection.CanConvert(typeof(string), parameter.ParameterType))
                {
                    parameterValues[i] = converterCollection.Convert(args[i + 1], parameter.ParameterType);
                }
                else
                {
                    Console.Log(
                        "Failed to convert argument {0}({1}) to {2} type", 
                        i, parameter.Name, parameter.ParameterType.Name);
                    return;
                }
            }

            method.Invoke(instance, parameterValues);
        }
        
        #region Properties
        public string Description { get { return description; } }
        #endregion Properties

        #region Fields
        private MethodInfo method;
        private object instance;
        private TypeConverterCollection converterCollection;
        private string description;
        #endregion Fields
    }
}

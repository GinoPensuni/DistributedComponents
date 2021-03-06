﻿// ----------------------------------------------------------------------- 
// <copyright file="IComponent.cs" company="FHWN"> 
// Copyright (c) FHWN. All rights reserved. 
// </copyright> 
// <summary>Component implementation interface.</summary> 
// <author>Michael Sabransky</author> 
// -----------------------------------------------------------------------
namespace Core.Component
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// An atomic component assembly has to implement this interface.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Gets the unique component id.
        /// Must be generated by the Component Developer.
        /// DO NOT REUSE GUIDS.
        /// </summary>
        /// <value>A unique identifier.</value>
        Guid ComponentGuid { get; }

        /// <summary>
        /// Gets the display name for the component.
        /// </summary>
        /// <value>A name string.</value>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the collection of types that describe the input arguments.
        /// </summary>
        /// <value>Collection of strings.</value>
        IEnumerable<string> InputHints { get; }

        /// <summary>
        /// Gets the collection of types that describe the output arguments.
        /// </summary>
        /// <value>Collection of strings.</value>
        IEnumerable<string> OutputHints { get; }

        /// <summary>
        /// Gets or sets the collection of strings that describe the input arguments.
        /// </summary>
        /// <value>Collection of strings.</value>
        IEnumerable<string> InputDescriptions { get; set; }

        /// <summary>
        /// Gets or sets the collection of strings that describe the output arguments.
        /// </summary>
        /// <value>Collection of strings.</value>
        IEnumerable<string> OutputDescriptions { get; set; }

        /// <summary>
        /// Executes the implementation of the component.
        /// </summary>
        /// <param name="values">Collection of input arguments.</param>
        /// <returns>Collection of output arguments.</returns>
        IEnumerable<object> Evaluate(IEnumerable<object> values);
    }
}
